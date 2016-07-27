using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RigoFunc.ApiCore {
    /// <summary>
    /// Represents the abstraction for API result transformation.
    /// </summary>
    public interface IApiResultHandler {
        /// <summary>
        /// Called when [result executing] for Android system.
        /// </summary>
        /// <param name="context">The instance of <see cref="ResourceExecutedContext"/>.</param>
        void OnResultExecutingForAndroid(ResultExecutingContext context);
        /// <summary>
        /// Called when [result executing] for IOS system.
        /// </summary>
        /// <param name="context">The instance of <see cref="ResourceExecutedContext"/>.</param>
        void OnResultExecutingForIOS(ResultExecutingContext context);
    }
}
