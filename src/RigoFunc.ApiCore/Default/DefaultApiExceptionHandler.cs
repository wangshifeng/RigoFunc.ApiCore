using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RigoFunc.ApiCore.Internal;
using Love.Net.Core;

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
            if(context.Exception is Exception<InvokeError>) {
                var exception = context.Exception as Exception<InvokeError>;
                context.Result = new ObjectResult(ApiResult.BadRequest(exception.Error, exception.Error.Message));
            }
            else {
                context.Result = new ObjectResult(ApiResult.BadRequest(context.Exception.Message));
            }

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

            // ErrorMessage property is for old version APP compatibility
            if (context.Exception is Exception<InvokeError>) {
                var exception = context.Exception as Exception<InvokeError>;
                context.Result = new ObjectResult(new {
                    Error = exception.Error,
                    // comment out for compatibility
                    ErrorMessage = exception.Error.Message,
                });
            }
            else {
                context.Result = new ObjectResult(new {
                    Error = InvokeError.Caused(context.Exception.Message, "Exception"),
                    // comment out for compatibility
                    ErrorMessage = context.Exception.Message,
                });
            }

            context.ExceptionHandled = true;
        }
    }
}
