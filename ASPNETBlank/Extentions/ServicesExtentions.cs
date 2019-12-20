using ASPNETBlank.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETBlank.Extentions
{
    public static class ServicesExtentions
    {
        public static IServiceCollection AddHashService(this IServiceCollection services)
        {
            services.AddScoped<IHashGeneratorService, HashGeneratorService>();
            return services;
        }

        public static IServiceCollection AddSQLConnectionService(this IServiceCollection services)
        {
            services.AddScoped<IDbConnectionService, SQLConnectionService>();
            return services;
        }
    }
}
