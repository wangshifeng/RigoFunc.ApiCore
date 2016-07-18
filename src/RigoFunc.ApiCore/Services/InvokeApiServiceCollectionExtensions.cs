using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RigoFunc.ApiCore.Services;

namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring Sms and email services.
    /// </summary>
    public static class InvokeApiServiceCollectionExtensions {
        /// <summary>
        /// Adds the Api invoking services to application service.
        /// </summary>
        /// <param name="services">The application services.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddApiInvoker(this IServiceCollection services, Action<InvokeApiOptions> setupAction) {
            if (setupAction != null) {
                services.Configure(setupAction);
            }

            services.TryAddTransient<IEmailSender, InvokeApiSender>();
            services.TryAddTransient<IAppPush, InvokeApiSender>();
            services.TryAddTransient<ISmsSender, InvokeApiSender>();

            return services;
        }
    }
}
