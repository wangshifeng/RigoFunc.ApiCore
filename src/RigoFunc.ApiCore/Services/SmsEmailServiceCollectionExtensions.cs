using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RigoFunc.ApiCore.Services;

namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring Sms and email services.
    /// </summary>
    public static class SmsEmailServiceCollectionExtensions {
        /// <summary>
        /// Adds the Sms and email service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddSmsEmailService(this IServiceCollection services, Action<SmsEmailOptions> setupAction) {
            if (setupAction != null) {
                services.Configure(setupAction);
            }

            services.TryAddTransient<IEmailSender, ApiInvokingSender>();
            services.TryAddTransient<ISmsSender, ApiInvokingSender>();

            return services;
        }
    }
}
