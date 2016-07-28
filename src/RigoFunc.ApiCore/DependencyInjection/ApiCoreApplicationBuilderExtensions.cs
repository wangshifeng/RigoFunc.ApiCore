using System.IdentityModel.Tokens.Jwt;
using RigoFunc.ApiCore;

namespace Microsoft.AspNetCore.Builder {
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/> to add Api Core to the request execution pipeline.
    /// </summary>
    public static class ApiCoreApplicationBuilderExtensions {
        /// <summary>
        /// Adds MVC with OAuth to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="options">The <see cref="OAuthOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMvcWithOAuth(this IApplicationBuilder app, OAuthOptions options) {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions {
                Authority = options.HostUrl,
                RequireHttpsMetadata = false,

                ScopeName = options.ScopeName,
                ScopeSecret = options.ScopeSecret,
                AutomaticAuthenticate = true
            });

            app.UseMvc();

            return app;
        }
    }
}
