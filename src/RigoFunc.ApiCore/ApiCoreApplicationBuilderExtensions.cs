namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/> to add Api Core to the request execution pipeline.
    /// </summary>
    public static class ApiCoreApplicationBuilderExtensions {
        /// <summary>
        /// Adds Api Core with OAuth to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseCoreWithOAuth(this IApplicationBuilder app) {
            app.UseMvc();

            app.UseOAuth();

            return app;
        }
    }
}
