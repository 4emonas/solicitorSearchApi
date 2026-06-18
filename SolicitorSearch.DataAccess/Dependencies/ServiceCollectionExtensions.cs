using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SolicitorSearch.DataAccess.Contexts;
using SolicitorSearch.DataAccess.Contexts.Abstract;
using SolicitorSearch.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitorSearch.DataAccess.Dependencies
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessLayers(this IServiceCollection services)
        {
            AddPostgres(services);
            return services;
        }

        private static IServiceCollection AddPostgres(IServiceCollection services)
        {
            //TODO: This MUST be coming from a secret manager layer. Leaving it for now as it is local db
            var conn = "User ID=postgres;Password=25471993;Host=localhost;Port=5432;Database=postgres;"; 

            services.AddDbContext<PostgresContext>(options =>
                options.UseNpgsql(conn));

            services.TryAddScoped<IPostgresContext, PostgresContext>();

            return services;
        }
    }
}
