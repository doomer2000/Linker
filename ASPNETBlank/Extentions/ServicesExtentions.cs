﻿using ASPNETBlank.Services;
using ASPNETBlank.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETBlank.Extentions
{
    public static class ServicesExtentions
    {

        public static IServiceCollection AddDateTimeService(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
            return services;
        }
        public static IServiceCollection AddHashService(this IServiceCollection services)
        {
            services.AddScoped<IHashGeneratorService, HashGeneratorService>();
            return services;
        }

        public static IServiceCollection AddUrlManipulationService(this IServiceCollection services)
        {
            services.AddScoped<IUrlManipulationService, UrlManipulationService>();
            return services;
        }

        public static IServiceCollection AddSQLConnectionService(this IServiceCollection services)
        {
            services.AddScoped<IDbConnectionService, SQLConnectionService>();
            return services;
        }
    }
}
