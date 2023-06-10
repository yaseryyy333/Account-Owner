using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Entities;
using Repository;
using Entities.Models;

namespace AccountOwnerServer.Extentsions
{
    public static class ServiceExtenstions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors( options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithHeaders("accept", "content-type"));
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(option =>
            {
                
            });
                
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["mysqlconnection:connectionString"];

            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString,
                MySqlServerVersion.LatestSupportedServerVersion));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<ISortHelper<Owner>, SortHelper<Owner>>();
            services.AddScoped<ISortHelper<Account>, SortHelper<Account>>();

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
