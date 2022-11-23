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

                // ���� Windows ������
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
        /// �����м��
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

            // UseCors ������ UseAuthorization ֮ǰ�� UseRouting ֮�����
            bool enableCors = config.GetSection("EnableCors").Get<bool>();
            if (enableCors)
            {
                app.UseCors("Cors");
            }

            app.UseAuthentication();
            app.UseAuthorization();

            // ʹ�����󱾵ػ�
            // app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// ע�����
        /// </summary>
        /// <param name="builder"></param>
        private static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var env = builder.Environment;
            var config = builder.Configuration;
            var services = builder.Services;

            services.Configure<AppSettings>(config);

            #region ע�����ݿ�ִ�����

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

            #endregion ע�����ݿ�ִ�����

            #region ע�������

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

            #endregion ע�������

            #region ȫ��&���ػ�

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

            #endregion ȫ��&���ػ�

            #region ��֤&��Ȩ

            services.AddUserInfo();

            string accessToken = config.GetSection("AccessToken").Get<string>();
            string cookieName = ".AspNetCore.Authentication." + builder.Environment.ApplicationName;

            services
                .AddJwtAuthentication(config.GetSection(nameof(JwtOptions)))
                .AddCookie(options =>
            {
                options.Cookie.Name = cookieName;                   // Cookie��
                options.Cookie.HttpOnly = true;                     // ָʾ�ͻ��˽ű��Ƿ���Է���cookie��
                options.Cookie.IsEssential = true;                  // ָʾ�� Cookie �Ƿ��Ӧ�ó�����������������Ҫ, ���Ϊ true, ������ƹ�ͬ����Լ��, Ĭ��ֵΪ false
                options.ExpireTimeSpan = TimeSpan.FromDays(7);      // Cookie�����֤����ʱ�䣨�ڷ������֤��
                options.SlidingExpiration = true;                   // ��������
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

            #endregion ��֤&��Ȩ

            #region Swagger�ĵ�

            bool enableSwagger = config.GetSection("EnableSwagger").Get<bool>();
            if (enableSwagger)
            {
                // ���ݷ����ServiceType��ImplementationType�����ж�, ����Ѵ��ڶ�Ӧ�ķ��������, ������Ϊͬһ��������Ӷ����ͬ��ʵ�ֵĳ���
                // ע����Ӧ״̬�뼰�����ṩ��
                services.TryAddEnumerable(ServiceDescriptor.Singleton<IApplicationModelProvider, ResponseTypeModelProvider>());

                // Register the Swagger services
                services.AddOpenApiDocument(config =>
                {
                    config.GenerateEnumMappingDescription = true;
                    config.PostProcess = document =>
                    {
                        document.Info.Version = "v1";
                        document.Info.Title = "IceCoffee.Template.WebApi Documentation";
                        document.Info.Description = "һ������ RABC �ļ� WebApi ģ��";
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

                    // �������ô�ע���ļ�����, ���Ǽ��ص����ݿɱ� OpenApiTagAttribute ���Ը���
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

            #endregion Swagger�ĵ�

            #region ���䷴�����

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                // �� ForwardedHeadersMiddleware �м�������268�� CheckKnownAddress ����
                // ������ʵ�IP�Ƿ��� ForwardedHeadersOptions.KnownProxies ���� ForwardedHeadersOptions.KnownNetworks ֮��
                // ͨ����� KnownNetworks �� KnownProxies ��Ĭ��ֵ����ִ���ϸ�ƥ��, �������п����ܵ� IP��ƭ ����
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            #endregion ���䷴�����

            #region ���ÿ���

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

            #endregion ���ÿ���
        }
    }
}