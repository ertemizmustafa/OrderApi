using Microsoft.Extensions.DependencyInjection;
using Order.Data.Connection;
using System;
using System.Data;

namespace Order.Data.Configuration
{
    public static class DataConfigurationExtensions
    {
        public static IServiceCollection AddDatabase<TConnection>(this IServiceCollection services, Action<ConnectionOption> setupAction) where TConnection : IDbConnection, new()
        {

            if (setupAction == null) throw new Exception("ConnectionString bilgisini belirleyiniz.");

            services.AddSingleton<IDbConnectionFactory, DbConnection<TConnection>>();

            services.Configure(setupAction);
            return services;
        }
    }
}
