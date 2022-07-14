global using IceCoffee.AspNetCore;
global using IceCoffee.AspNetCore.Controllers;
global using IceCoffee.AspNetCore.Models;
global using IceCoffee.Template.WebApi.Models;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using IceCoffee.Template.Data.IRepositories;
global using IceCoffee.Common.Extensions;
global using IceCoffee.AspNetCore.Extensions;
global using System.ComponentModel.DataAnnotations;
using IceCoffee.AspNetCore.Authentication;
using IceCoffee.AspNetCore.Authorization;
using IceCoffee.AspNetCore.Middlewares;
using IceCoffee.AspNetCore.Options;
using IceCoffee.Template.Data;
using IceCoffee.Template.WebApi.JsonConverters;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console()
                 .CreateBootstrapLogger();

            Log.Information("Starting up!");

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                var hostBuilder = builder.Host;

                hostBuilder.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext());

                // ���� Windows ������
                // hostBuilder.UseWindowsService();

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
                if (type.Namespace != null && type.Namespace.StartsWith("IceCoffee.Template.Data.Repositories"))
                {
                    var interfaceType = type.GetInterfaces().First(
                        p => p.Namespace != null && p.Namespace.StartsWith("IceCoffee.Template.Data.IRepositories"));
                    services.TryAddSingleton(interfaceType, type);
                }
            }

            #endregion ע�����ݿ�ִ�����

            #region ע�������

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    string messages = string.Join(Environment.NewLine,
                        context.ModelState.Values.SelectMany(s => s.Errors).Select(s => s.ErrorMessage));

                    IConvertToActionResult result = new Response()
                    {
                        Status = HttpStatus.BadRequest,
                        Error = new Error()
                        {
                            Message = "One or more model validation errors occurred",
                            Details = context.ModelState.Values.SelectMany(s => s.Errors).Select(s => s.ErrorMessage).ToArray()
                        }
                    };

                    return result.Convert();
                };
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                // options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
            services
                .AddJwtAuthentication(config.GetSection(nameof(JwtOptions)))
                .AddCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.Authentication." + builder.Environment.ApplicationName;   // Cookie��
                options.Cookie.HttpOnly = true;                             // ָʾ�ͻ��˽ű��Ƿ���Է���cookie��
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);              // Cookie�����֤����ʱ�䣨�ڷ������֤��
                options.SlidingExpiration = true;                           // ��������
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

            // �����Ȩ��������Ĭ�����΢�ź�PC�������ﲻ��ʹ�� TryAdd������ֻ�����һ�� IAuthorizationHandler
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IAuthorizationHandler, AuthorizationHandler>());

            var permissionRequirement = new PermissionRequirement()
            {
                RequireHttpMethods = true
            };
            services.AddSingleton(permissionRequirement);

            // ��� JWT �� Cookie �Ļ����Ȩ���Է���
            services.AddAuthorization(options =>
            {
                // https://blog.csdn.net/sD7O95O/article/details/105382881
                // InvokeHandlersAfterFailure Ϊ true ������£�Ĭ��Ϊ true ��������ע���˵� AuthorizationHandler ���ᱻִ��
                options.InvokeHandlersAfterFailure = false;

                // �����Դ�����κ� IAuthorizeData ʵ�����򽫶����ǽ��������������ǻ��˲���
                options.FallbackPolicy = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    AuthenticationSchemes.ApiKeyAuthenticationSchemeName)
                    .RequireAuthenticatedUser()
                    .AddRequirements(permissionRequirement)
                    .Build();
            });

            #endregion ��֤&��Ȩ

            #region Swagger�ĵ�

            bool enableSwagger = config.GetSection("EnableSwagger").Get<bool>();
            if (enableSwagger)
            {
                // ���ݷ����ServiceType��ImplementationType�����жϣ�����Ѵ��ڶ�Ӧ�ķ�������ӣ�������Ϊͬһ��������Ӷ����ͬ��ʵ�ֵĳ���
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

                    // �������ô�ע���ļ����أ����Ǽ��ص����ݿɱ� OpenApiTagAttribute ���Ը���
                    config.UseControllerSummaryAsTagDescription = true;

                    config.AddSecurity("ApiKey", new OpenApiSecurityScheme()
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Scheme = AuthenticationSchemes.ApiKeyAuthenticationSchemeName,
                        Name = ApiKeyAuthenticationHandler.HttpRequestHeaderName,
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: {your access-token}."
                    });

                    config.AddSecurity(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                    {
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT",
                        Description = "Type into the textbox: {your JWT token}."
                    });

                    config.OperationProcessors.Add(new AspNetCoreOperationFallbackPolicyProcessor(nameof(OpenApiSecuritySchemeType.ApiKey)));
                    config.OperationProcessors.Add(new AspNetCoreOperationFallbackPolicyProcessor(JwtBearerDefaults.AuthenticationScheme));
                });
            }

            #endregion Swagger�ĵ�

            #region ���䷴�����

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                // �� ForwardedHeadersMiddleware �м�������268�� CheckKnownAddress ����
                // ������ʵ�IP�Ƿ��� ForwardedHeadersOptions.KnownProxies ���� ForwardedHeadersOptions.KnownNetworks ֮��
                // ͨ����� KnownNetworks �� KnownProxies ��Ĭ��ֵ����ִ���ϸ�ƥ�䣬�������п����ܵ� IP��ƭ ����
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