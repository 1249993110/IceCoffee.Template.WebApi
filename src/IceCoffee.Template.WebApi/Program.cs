using FastExpressionCompiler;
using IceCoffee.AspNetCore.Authentication;
using IceCoffee.AspNetCore.JsonConverters;
using IceCoffee.AspNetCore.Middlewares;
using IceCoffee.AspNetCore.Options;
using IceCoffee.DbCore.Repositories;
using IceCoffee.Template.Data;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSwag;
using Serilog;

[assembly: ApiController]
namespace IceCoffee.Template.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console()
                 .CreateBootstrapLogger();

            Log.Information("Starting up!");

            try
            {
                Directory.SetCurrentDirectory(AppContext.BaseDirectory);

                TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();
                MapsterTypeAdapter.ConfigEntityToModel();

                var builder = WebApplication.CreateBuilder(args);
                var hostBuilder = builder.Host;

                hostBuilder.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext());

                // 启用 Windows 服务部署
                hostBuilder.UseWindowsService();

                builder.ConfigureServices();

                var app = builder.Build();
                app.Configure();
                app.Run();

                Log.Information("Stopped cleanly");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
                Console.ReadKey();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="app"></param>
        private static void Configure(this WebApplication app)
        {
            var env = app.Environment;
            var config = app.Configuration;

            bool enableRequestLog = config.GetSection("EnableRequestLog").Get<bool>();
            if (enableRequestLog)
            {
                app.UseSerilogRequestLogging();
            }

            app.UseMiddleware<GlobalExceptionHandleMiddleware>();
            app.UseForwardedHeaders();

            string pathBase = config.GetSection("PathBase").Get<string>();
            if (string.IsNullOrEmpty(pathBase) == false)
            {
                app.UsePathBase(pathBase);
            }

            bool enableSwagger = config.GetSection("EnableSwagger").Get<bool>();
            if (enableSwagger)
            {
                // Register the Swagger endpoint and the Swagger UI middlewares
                app.UseOpenApi(config =>
                {
                    config.PostProcess = (document, httpRequest) =>
                    {
                        document.BasePath = httpRequest.PathBase;
                    };
                });
                app.UseSwaggerUi3();
            }

            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);

            app.UseStaticFiles();

            app.UseRouting();

            // UseCors 必须在 UseAuthorization 之前在 UseRouting 之后调用
            bool enableCors = config.GetSection("EnableCors").Get<bool>();
            if (enableCors)
            {
                app.UseCors("Cors");
            }

            app.UseAuthentication();
            app.UseAuthorization();

            // 使用请求本地化
            // app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="builder"></param>
        private static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var env = builder.Environment;
            var config = builder.Configuration;
            var services = builder.Services;

            services.Configure<AppSettings>(config);

            #region 注册数据库仓储服务

            var dbConnectionInfos = config.GetSection("DbConnectionInfos");
            var defaultDbConnectionInfo = dbConnectionInfos.GetSection(nameof(DefaultDbConnectionInfo)).Get<DefaultDbConnectionInfo>();
            services.TryAddSingleton(defaultDbConnectionInfo);

            foreach (var type in typeof(DefaultDbConnectionInfo).Assembly.GetExportedTypes())
            {
                if (type.IsSubclassOf(typeof(RepositoryBase)) && type.IsAbstract == false)
                {
                    var interfaceType = type.GetInterfaces().First(p => p.IsGenericType == false);
                    services.TryAddSingleton(interfaceType, type);
                }
            }

            #endregion 注册数据库仓储服务

            #region 注册控制器

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    IConvertToActionResult result = new Response()
                    {
                        Status = HttpStatus.BadRequest,
                        Error = new Error()
                        {
                            Message = "One or more model validation errors occurred",
                            Details = context.ModelState.Values.SelectMany(s => s.Errors).Select(s => s.ErrorMessage)
                        }
                    };

                    return result.Convert();
                };
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new GuidConverter());
                options.JsonSerializerOptions.Converters.Add(new GuidNullableConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                // options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // [JsonConverter(typeof(JsonStringEnumConverter))]
            });

            #endregion 注册控制器

            #region 全球化&本地化

            //.AddDataAnnotationsLocalization(options =>
            //{
            //    options.DataAnnotationLocalizerProvider = (type, factory) =>
            //    {
            //        return factory.Create(typeof(IceCoffee.DataAnnotations.Resource));
            //    };
            //});

            //services.AddLocalization();
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    var supportedCultures = new CultureInfo[]
            //    {
            //        new CultureInfo(Shared.Constants.Cultures.ZhCn),
            //        new CultureInfo(Shared.Constants.Cultures.En),
            //    };

            //    // State what the default culture for your application is. This will be used if no specific culture
            //    // can be determined for a given request.
            //    options.DefaultRequestCulture = new RequestCulture(supportedCultures[0].Name);

            //    // You must explicitly state which cultures your application supports.
            //    // These are the cultures the app supports for formatting numbers, dates, etc.
            //    options.SupportedCultures = supportedCultures;

            //    // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
            //    options.SupportedUICultures = supportedCultures;

            //    options.ApplyCurrentCultureToResponseHeaders = true;
            //});

            #endregion 全球化&本地化

            #region 认证&授权

            services.AddUserInfo();

            string accessToken = config.GetSection("AccessToken").Get<string>();
            string cookieName = ".AspNetCore.Authentication." + builder.Environment.ApplicationName;

            services
                .AddJwtAuthentication(config.GetSection(nameof(JwtOptions)))
                .AddCookie(options =>
            {
                options.Cookie.Name = cookieName;                   // Cookie名
                options.Cookie.HttpOnly = true;                     // 指示客户端脚本是否可以访问cookie。
                options.Cookie.IsEssential = true;                  // 指示此 Cookie 是否对应用程序正常运行至关重要, 如果为 true, 则可以绕过同意策略检查, 默认值为 false
                options.ExpireTimeSpan = TimeSpan.FromDays(7);      // Cookie身份验证到期时间（在服务端验证）
                options.SlidingExpiration = true;                   // 滑动过期
                options.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = (context) =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            }).AddApiKeyAuthentication(options => options.AccessToken = accessToken);

            services.AddAreaAuthorization();

            #endregion 认证&授权

            #region Swagger文档

            bool enableSwagger = config.GetSection("EnableSwagger").Get<bool>();
            if (enableSwagger)
            {
                // 根据服务的ServiceType和ImplementationType进行判断, 如果已存在对应的服务则不添加, 适用于为同一个服务添加多个不同的实现的场景
                // 注册响应状态码及类型提供者
                services.TryAddEnumerable(ServiceDescriptor.Singleton<IApplicationModelProvider, ResponseTypeModelProvider>());

                // Register the Swagger services
                services.AddOpenApiDocument(config =>
                {
                    config.GenerateEnumMappingDescription = true;
                    config.PostProcess = document =>
                    {
                        document.Info.Version = "v1";
                        document.Info.Title = "IceCoffee.Template.WebApi Documentation";
                        document.Info.Description = "一个基于 RABC 的简单 WebApi 模板";
                        document.Info.Contact = new OpenApiContact()
                        {
                            Name = "IceCoffee",
                            Email = "1249993110@qq.com",
                            Url = "https://github.com/1249993110"
                        };
                        document.Info.License = new OpenApiLicense()
                        {
                            Name = "LICENSE",
                            Url = "https://github.com/1249993110/IceCoffee.Template.WebApi/blob/main/LICENSE"
                        };
                    };

                    // 可以设置从注释文件加载, 但是加载的内容可被 OpenApiTagAttribute 特性覆盖
                    config.UseControllerSummaryAsTagDescription = true;

                    config.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                    {
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT",
                        Description = "Type into the textbox: {your JWT token}."
                    });
                    config.OperationProcessors.Add(new AspNetCoreOperationFallbackPolicyProcessor(JwtBearerDefaults.AuthenticationScheme));

                    config.AddSecurity("ApiKey", new OpenApiSecurityScheme()
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Scheme = AuthenticationSchemes.ApiKeyAuthenticationSchemeName,
                        Name = ApiKeyAuthenticationHandler.HttpRequestHeaderName,
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: {your access-token}."
                    });
                    config.OperationProcessors.Add(new AspNetCoreOperationFallbackPolicyProcessor(nameof(OpenApiSecuritySchemeType.ApiKey)));
                });
            }

            #endregion Swagger文档

            #region 适配反向代理

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                // 在 ForwardedHeadersMiddleware 中间件代码第268行 CheckKnownAddress 方法
                // 会检查访问的IP是否在 ForwardedHeadersOptions.KnownProxies 或者 ForwardedHeadersOptions.KnownNetworks 之中
                // 通过清空 KnownNetworks 和 KnownProxies 的默认值来不执行严格匹配, 这样做有可能受到 IP欺骗 攻击
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            #endregion 适配反向代理

            #region 配置跨域

            bool enableCors = config.GetSection("EnableCors").Get<bool>();
            if (enableCors)
            {
                var allowedOrigins = config.GetSection("AllowedOrigins").Get<string[]>();

                services.AddCors(options =>
                {
                    options.AddPolicy("Cors", builder =>
                    {
                        if (allowedOrigins == null || allowedOrigins.Length == 0)
                        {
                            builder.AllowAnyOrigin();
                        }
                        else
                        {
                            builder.WithOrigins(allowedOrigins);
                        }
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
                });
            }

            #endregion 配置跨域
        }
    }
}