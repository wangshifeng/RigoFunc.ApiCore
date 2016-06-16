using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RigoFunc.ApiCore.Services;

namespace Microsoft.Extensions.DependencyInjection {
    public static class SmsEmailServiceCollectionExtensions {
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
