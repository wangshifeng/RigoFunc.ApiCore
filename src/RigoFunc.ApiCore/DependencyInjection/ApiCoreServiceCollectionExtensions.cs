using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using RigoFunc.ApiCore;
using RigoFunc.ApiCore.Default;
using RigoFunc.ApiCore.Filters;

namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// Extension methods for setting up API core services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ApiCoreServiceCollectionExtensions {
        /// <summary>
        /// Adds essential MVC services with API core feature to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="IMvcCoreBuilder"/> that can be used to further configure the MVC services.</returns>
        public static IMvcCoreBuilder AddCore(this IServiceCollection services) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddScoped<IApiResultHandler, DefaultApiResultHandler>();
            services.TryAddScoped<IApiExceptionHandler, DefaultApiExceptionHandler>();

            var builder = services.AddMvcCore().AddMvcOptions(options => {
                options.Filters.Add(typeof(ApiResultFilterAttribute));
                options.Filters.Add(typeof(ApiExceptionFilterAttribute));
            });

            builder.AddJsonFormatters(options => {
                options.ContractResolver = new DefaultContractResolver();
                options.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            return builder;
        }

        /// <summary>
        /// Adds essential MVC services with OAuth to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="IMvcCoreBuilder"/> that can be used to further configure the MVC services.</returns>
        public static IMvcCoreBuilder AddCoreWithOAuth(this IServiceCollection services) {
            var builder = services.AddCore();

            builder.AddMvcOptions(options => {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddAuthorization();

            return builder;
        }
    }
}
