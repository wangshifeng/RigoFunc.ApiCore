﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RigoFunc.ApiCore.Internal;

namespace RigoFunc.ApiCore.Default {
    /// <summary>
    /// Represents the default implementation of <see cref="IApiResultHandler"/> interface.
    /// </summary>
    public class DefaultApiResultHandler : IApiResultHandler {
        /// <summary>
        /// Called when [result executing] for Android system.
        /// </summary>
        /// <param name="context">The instance of <see cref="ResourceExecutedContext" />.</param>
        public void OnResultExecutingForAndroid(ResultExecutingContext context) {
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

        /// <summary>
        /// Called when [result executing] for IOS system.
        /// </summary>
        /// <param name="context">The instance of <see cref="ResourceExecutedContext" />.</param>
        public void OnResultExecutingForIOS(ResultExecutingContext context) {
            // do nothing here.
        }
    }
}