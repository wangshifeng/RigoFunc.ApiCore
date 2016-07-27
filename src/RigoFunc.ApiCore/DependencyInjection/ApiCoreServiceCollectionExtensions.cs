using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using RigoFunc.ApiCore;
using RigoFunc.ApiCore.Default;
using RigoFunc.ApiCore.Filters;
using RigoFunc.OAuth;

namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// Extension methods for setting up API core services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ApiCoreServiceCollectionExtensions {
        /// <summary>
        /// Adds API core services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="IMvcCoreBuilder"/> that can be used to further configure the MVC services.</returns>
        public static IMvcCoreBuilder AddCore(this IServiceCollection services) {
            if (services == null) {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IApiResultHandler, DefaultApiResultHandler>();
            services.TryAddTransient<IApiExceptionHandler, DefaultApiExceptionHandler>();

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
        /// Adds API core services with OAuth to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{OAuthOptions}"/> to configure the provided <see cref="OAuthOptions"/>.</param>
        /// <returns>An <see cref="IMvcCoreBuilder"/> that can be used to further configure the MVC services.</returns>
        public static IMvcCoreBuilder AddCoreWithOAuth(this IServiceCollection services, Action<OAuthOptions> setupAction) {
            var builder = services.AddCore();

            builder.AddMvcOptions(options => {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddAuthorization();

            if (setupAction != null) {
                services.Configure(setupAction);
            }

            return builder;
        }
    }
}
