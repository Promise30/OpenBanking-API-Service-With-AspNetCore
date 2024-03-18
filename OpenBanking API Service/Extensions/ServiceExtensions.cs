using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenBanking_API_Service.Data;
using OpenBanking_API_Service.Infrastructures.Implementation;
using OpenBanking_API_Service.Infrastructures.Interface;
using OpenBanking_API_Service.Service.Implementation;
using OpenBanking_API_Service.Service.Interface;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

namespace OpenBanking_API_Service.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination"));
            });
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {

            });
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();


        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        //public static void ConfigureResponseCaching(this IServiceCollection services) =>
        //    services.AddResponseCaching();

        public static void ConfigureOutputCaching(this IServiceCollection services) =>
            services.AddOutputCache(options =>
            {
                //options.AddBasePolicy(bp => bp.Expire(TimeSpan.FromSeconds(30)));
                options.AddPolicy("120SecondsDuration", p => p.Expire(TimeSpan.FromSeconds(120)));
            });

        public static void ConfigureRateLimitingOptions(this IServiceCollection services) =>
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",
                    partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        QueueLimit = 2,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        Window = TimeSpan.FromMinutes(1)
                    }

                ));
                options.AddPolicy("SpecificPolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter("SpecificLimiter",
                    partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 3,
                        Window = TimeSpan.FromSeconds(10)
                    }));
                //options.RejectionStatusCode = 429;
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                        await context.HttpContext.Response
                            .WriteAsync($"Too many requests. Please try again after {retryAfter.TotalSeconds} second(s).", token);
                    else
                        await context.HttpContext.Response
                            .WriteAsync("Too many request. Please try again later.");
                };


            });


        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

                options.Tokens.ProviderMap["Email"] = new TokenProviderDescriptor(typeof(EmailTokenProvider<ApplicationUser>));
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidIssuer = jwtSettings["ValidIssuer"],
                        ValidAudience = jwtSettings["ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
                    };
                });
        }
        public static void ConfigureSwaggerGen(this IServiceCollection services) =>
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "OpenBanking API Service",
                    Description = "An ASP.NET Core Web API Service for a Banking System.",
                });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }

                        },
                        new string[]{}
                    }
                });
            });




    }
}
