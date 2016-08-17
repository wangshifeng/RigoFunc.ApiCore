// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RigoFunc.ApiCore.Filters {
    /// <summary>
    /// Provides Api result transform feature base http request header.
    /// </summary>
    public class ApiResultFilterAttribute : ResultFilterAttribute {
        private readonly IApiResultHandler _resultHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResultFilterAttribute" /> class.
        /// </summary>
        /// <param name="resultHandler">The result handler.</param>
        public ApiResultFilterAttribute(IApiResultHandler resultHandler) {
            _resultHandler = resultHandler;
        }

        /// <summary>
        /// Called when [result executing].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <inheritdoc />
        public sealed override void OnResultExecuting(ResultExecutingContext context) {
            string device = context.HttpContext.Request.Headers["device"];
            if (!string.IsNullOrEmpty(device)) {
                if (device.Equals("ios", StringComparison.OrdinalIgnoreCase)) {
                    _resultHandler.OnResultExecutingForIOS(context);
                }
                else if (device.Equals("android", StringComparison.OrdinalIgnoreCase)) {
                    _resultHandler.OnResultExecutingForAndroid(context);
                }
            }

            base.OnResultExecuting(context);
        }
    }
}
