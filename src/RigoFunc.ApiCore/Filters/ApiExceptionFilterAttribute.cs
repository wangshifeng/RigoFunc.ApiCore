// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace RigoFunc.ApiCore.Filters {
    /// <summary>
    /// Represents a filter to handle Api exception.
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute {
        private readonly IApiExceptionHandler _exceptionHandler;
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiExceptionFilterAttribute" /> class.
        /// </summary>
        /// <param name="exceptionHandler">The exception handler.</param>
        /// <param name="logger">The logger</param>
        public ApiExceptionFilterAttribute(IApiExceptionHandler exceptionHandler, ILogger<ApiExceptionFilterAttribute> logger) {
            _exceptionHandler = exceptionHandler;
            _logger = logger;
        }

        /// <summary>
        /// Called when [exception].
        /// </summary>
        /// <param name="context">The context.</param>
        public sealed override void OnException(ExceptionContext context) {
            try {
                // log error
                _logger.LogError(context.Exception.ToString());

                string device = context.HttpContext.Request.Headers["device"];
                if (!string.IsNullOrEmpty(device)) {
                    if (device.Equals("ios", StringComparison.OrdinalIgnoreCase)) {
                        _exceptionHandler.HandleExceptionForIOS(context);
                    }
                    else if (device.Equals("android", StringComparison.OrdinalIgnoreCase)) {
                        _exceptionHandler.HandleExceptionForAndroid(context);
                    }
                }

                if (!context.ExceptionHandled) {
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
            catch {
                base.OnException(context);
            }
        }
    }
}
