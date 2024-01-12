using OpenBanking_API_Service.Infrastructures.Implementation;
using OpenBanking_API_Service.Infrastructures.Interface;
using OpenBanking_API_Service.Service.Implementation;
using OpenBanking_API_Service.Service.Interface;

namespace OpenBanking_API_Service.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCore(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {

            });
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();


        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();





    }
}
