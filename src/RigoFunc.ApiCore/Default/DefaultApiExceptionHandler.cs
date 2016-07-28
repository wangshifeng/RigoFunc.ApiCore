using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RigoFunc.ApiCore.Internal;

namespace RigoFunc.ApiCore.Default {
    /// <summary>
    /// Represents the default implementation of the <see cref="IApiExceptionHandler"/> interface.
    /// </summary>
    public class DefaultApiExceptionHandler : IApiExceptionHandler {
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApiExceptionHandler"/> class.
        /// </summary>
        /// <param name="env">The <see cref="IHostingEnvironment"/> instance.</param>
        public DefaultApiExceptionHandler(IHostingEnvironment env) {
            _env = env;
        }

        /// <summary>
        /// Handles the exception for android.
        /// </summary>
        /// <param name="context">The instance of <see cref="ExceptionContext" />.</param>
        /// <remarks>
        /// The implementation MUST set the ExceptionHandled property of <see cref="ExceptionContext"/> to indicate whether
        /// had handled the exception or not.
        /// </remarks>
        public virtual void HandleExceptionForAndroid(ExceptionContext context) {
            var apiResult = ApiResult.BadRequest(context.Exception.Message);
            if (_env.IsDevelopment()) {
            }

            context.Result = new JsonResult(apiResult);

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Handles the exception for ios.
        /// </summary>
        /// <param name="context">The instance of <see cref="ExceptionContext" />.</param>
        /// <remarks>
        /// The implementation MUST set the ExceptionHandled property of <see cref="ExceptionContext"/> to indicate whether
        /// had handled the exception or not.
        /// </remarks>
        public virtual void HandleExceptionForIOS(ExceptionContext context) {
            // Bad Request
            context.HttpContext.Response.StatusCode = 400;

            var json = new {
                // this is for backward compatibility
                ErrorMessage = context.Exception.Message,
                Error = context.Exception.Message
            };

            // Json result.
            context.Result = new JsonResult(json);

            context.ExceptionHandled = true;
        }
    }
}
