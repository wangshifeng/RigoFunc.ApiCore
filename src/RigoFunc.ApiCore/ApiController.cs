using System;
using System.Security.Claims;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using RigoFunc.OAuth;

namespace RigoFunc.ApiCore {
    /// <summary>
    /// A base class for an Api controller without view support.
    /// </summary>
    [Controller]
    public abstract class ApiController {
        private ControllerContext _controllerContext;
        private IUrlHelper _url;

        /// <summary>
        /// Gets the <see cref="HttpContext"/> for the executing action.
        /// </summary>
        public HttpContext HttpContext => ControllerContext.HttpContext;

        /// <summary>
        /// Gets the <see cref="HttpRequest"/> for the executing action.
        /// </summary>
        public HttpRequest Request => HttpContext?.Request;

        /// <summary>
        /// Gets the <see cref="HttpResponse"/> for the executing action.
        /// </summary>
        public HttpResponse Response => HttpContext?.Response;

        /// <summary>
        /// Gets or sets the <see cref="ControllerContext"/>.
        /// </summary>
        [ControllerContext]
        public ControllerContext ControllerContext {
            get {
                if (_controllerContext == null) {
                    _controllerContext = new ControllerContext();
                }

                return _controllerContext;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                _controllerContext = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="IUrlHelper"/>.
        /// </summary>
        public IUrlHelper Url {
            get {
                if (_url == null) {
                    var factory = HttpContext?.RequestServices?.GetRequiredService<IUrlHelperFactory>();
                    _url = factory?.GetUrlHelper(ControllerContext);
                }

                return _url;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                _url = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="ClaimsPrincipal"/> for user associated with the executing action.
        /// </summary>
        public ClaimsPrincipal User => HttpContext?.User;

        /// <summary>
        /// Gets the <see cref="OAuthUser"/>.
        /// </summary>
        public OAuthUser OAuthUser => OAuthUser.FromUser(User);

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>The access token.</value>
        public string AccessToken => TokenRetriever(Request);

        /// <summary>
        /// Gets the token retriever.
        /// </summary>
        /// <value>The token retriever.</value>
        protected virtual Func<HttpRequest, string> TokenRetriever => TokenRetrieval.FromAuthorizationHeader();
    }
}
