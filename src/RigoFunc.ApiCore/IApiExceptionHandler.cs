using Microsoft.AspNetCore.Mvc.Filters;

namespace RigoFunc.ApiCore {
    /// <summary>
    /// Provides the interfaces for handling the API exception.
    /// </summary>
    public interface IApiExceptionHandler {
        /// <summary>
        /// Handles the exception for android.
        /// </summary>
        /// <param name="context">The instance of <see cref="ExceptionContext" />.</param>
        /// <remarks>
        /// The implementation MUST set the ExceptionHandled property of <see cref="ExceptionContext"/> to indicate whether
        /// had handled the exception or not.
        /// </remarks>
        void HandleExceptionForAndroid(ExceptionContext context);
        /// <summary>
        /// Handles the exception for ios.
        /// </summary>
        /// <param name="context">The instance of <see cref="ExceptionContext"/>.</param>
        /// <remarks>
        /// The implementation MUST set the ExceptionHandled property of <see cref="ExceptionContext"/> to indicate whether
        /// had handled the exception or not.
        /// </remarks>
        void HandleExceptionForIOS(ExceptionContext context);
    }
}
