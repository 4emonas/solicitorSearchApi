using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SolicitorSearch.DataAccess.Dependencies;
using SolicitorSearch.Domain.Implementation.Requests;
using SolicitorSearch.Domain.Scraping.Interfaces;
using SolicitorSearch.Utilities.HttpAccess;
using SolicitorSearch.Utilities.HttpAccess.Interfaces;
using SolicitorSearch.Utilities.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitorSearch.Domain.Dependencies
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            AddHttpProxy(services);
            AddMediatr(services);
            AddServices(services);
            services.AddDataAccessLayers();
            return services;
        }

        private static IServiceCollection AddHttpProxy(IServiceCollection services)
        {
            services.TryAddScoped<IHttpProxy, HttpProxy>();

            return services;
        }

        private static IServiceCollection AddMediatr(IServiceCollection services)
        {
            services.AddMediatR(typeof(GetSolicitorRequest));
            services.AddMediatR(typeof(GetSolicitorsRequest));

            return services;
        }

        private static IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddScoped<IScraper, Scraper>();
            return services;
        }
    }
}
