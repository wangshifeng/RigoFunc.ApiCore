using RigoFunc.ApiCore;

namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// Extension methods for <see cref="IMvcCoreBuilder" />.
    /// </summary>
    public static class IMvcCoreBuilderExtensions {
        /// <summary>
        /// Adds the default API result handler.
        /// </summary>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="builder">
        /// An <see cref="IMvcCoreBuilder"/> that can be used to further configure the MVC services.
        /// </param>
        /// <returns>
        /// An <see cref="IMvcCoreBuilder"/> that can be used to further configure the MVC services.
        /// </returns>
        public static IMvcCoreBuilder AddApiResultHandler<THandler>(this IMvcCoreBuilder builder) 
            where THandler : class, IApiResultHandler {
            builder.Services.AddScoped<IApiResultHandler, THandler>();

            return builder;
        }

        /// <summary>
        /// Adds the default API exception handler.
        /// </summary>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="builder">
        /// An <see cref="IMvcCoreBuilder"/> that can be used to further configure the MVC services.
        /// </param>
        /// <returns>
        /// An <see cref="IMvcCoreBuilder"/> that can be used to further configure the MVC services.
        /// </returns>
        public static IMvcCoreBuilder AddApiExceptionHandler<THandler>(this IMvcCoreBuilder builder)
            where THandler : class, IApiExceptionHandler {
            builder.Services.AddScoped<IApiExceptionHandler, THandler>();

            return builder;
        }
    }
}
