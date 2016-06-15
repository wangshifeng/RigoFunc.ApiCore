// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RigoFunc.ApiCore.Internal;

namespace RigoFunc.ApiCore.Filters {
    /// <summary>
    /// Provides Api result transform feature base http request header.
    /// </summary>
    public class ApiResultFilterAttribute : ResultFilterAttribute {
        /// <summary>
        /// Called when [result executing].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <inheritdoc />
        public override void OnResultExecuting(ResultExecutingContext context) {
            string device = context.HttpContext.Request.Headers["Device"];
            if (!string.IsNullOrEmpty(device)) {
                if (device.Equals("ios", StringComparison.OrdinalIgnoreCase)) {
                    // special logic for IOS
                }
                else if (device.Equals("android", StringComparison.OrdinalIgnoreCase)) {
                    // special logic for Android
                    if (context.Result is ObjectResult) {
                        var objectResult = context.Result as ObjectResult;
                        if (objectResult.Value == null) {
                            context.Result = new ObjectResult(ApiResult.NotFound());
                        }
                        else {
                            var apiResult = Activator.CreateInstance(
                                typeof(ApiResult<>).MakeGenericType(objectResult.DeclaredType), objectResult.Value);
                            context.Result = new ObjectResult(apiResult);
                        }
                    }
                }
            }

            base.OnResultExecuting(context);
        }
    }
}
