using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RigoFunc.ApiCore.Services;

namespace Microsoft.Extensions.DependencyInjection {
    public static class SmsEmailServiceCollectionExtensions {
        public static IServiceCollection AddSmsEmailService(this IServiceCollection services, Action<ServiceOptions> setupAction) {
            if (setupAction != null) {
                services.Configure(setupAction);
            }

            services.TryAddTransient<IEmailSender, MessageSender>();
            services.TryAddTransient<ISmsSender, MessageSender>();

            return services;
        }
    }
}
