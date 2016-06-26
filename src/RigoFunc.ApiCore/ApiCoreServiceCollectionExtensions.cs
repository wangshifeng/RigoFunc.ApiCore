using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Newtonsoft.Json.Serialization;
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

            var builder = services.AddMvcCore().AddMvcOptions(options => {
                var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(new ApiResultFilterAttribute());
                options.Filters.Add(new ApiExceptionFilterAttribute());
            });

            builder.AddJsonFormatters(options => {
                options.ContractResolver = new DefaultContractResolver();
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

            services.AddAuthorization();

            if (setupAction != null) {
                services.Configure(setupAction);
            }

            return builder;
        }
    }
}
