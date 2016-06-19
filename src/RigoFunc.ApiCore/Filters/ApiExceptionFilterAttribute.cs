// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RigoFunc.ApiCore.Internal;
using RigoFunc.XDoc;

namespace RigoFunc.ApiCore.Filters {
    /// <summary>
    /// Represents a filter to handle Api exception.
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute {
        /// <summary>
        /// Called when [exception].
        /// </summary>
        /// <param name="context">The context.</param>
        public sealed override void OnException(ExceptionContext context) {
            try {
                var loggerFactory = context.HttpContext.RequestServices.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<ApiExceptionFilterAttribute>();
                var env = context.HttpContext.RequestServices.GetService<IHostingEnvironment>();

                var exception = context.Exception;
                // log error
                logger.LogError(exception.ToString());

                string device = context.HttpContext.Request.Headers["Device"];
                if (!string.IsNullOrEmpty(device)) {
                    if (device.Equals("ios", StringComparison.OrdinalIgnoreCase) && HandleExceptionForIOS(env, context)) {
                        return;
                    }
                    else if (device.Equals("android", StringComparison.OrdinalIgnoreCase) && HandleExceptionForAndroid(env, context)) {
                        return;
                    }
                }

                // Bad Request
                context.HttpContext.Response.StatusCode = 400;

                var json = new JObject();
                json.Add("ErrorMessage", exception.Message);
                if (exception is ArgumentNullException && env.IsDevelopment()) {
                    json.Add("ApiDoc", GetApiDoc(context));
                }

                // Json result.
                context.Result = new JsonResult(json);
            }
            catch {
                base.OnException(context);
            }
        }

        /// <summary>
        /// Special exception handle logic for Android.
        /// </summary>
        /// <param name="env">The hosting environment.</param>
        /// <param name="context">The exception context.</param>
        /// <returns><c>true</c> if had handle the exception, <c>false</c> otherwise.</returns>
        protected virtual bool HandleExceptionForAndroid(IHostingEnvironment env, ExceptionContext context) {
            var apiResult = ApiResult.BadRequest(context.Exception.Message);
            if (env.IsDevelopment()) {
                apiResult.Debug = GetApiDoc(context);
            }

            context.Result = new JsonResult(apiResult);

            return true;
        }

        /// <summary>
        /// Special exception handle logic for ios.
        /// </summary>
        /// <param name="env">The hosting environment.</param>
        /// <param name="context">The exception context.</param>
        /// <returns><c>true</c> if had handle the exception, <c>false</c> otherwise.</returns>
        protected virtual bool HandleExceptionForIOS(IHostingEnvironment env, ExceptionContext context) => false;

        /// <summary>
        /// Gets the API documentation.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A instance of <see cref="JObject"/> represents the Json style documentation.</returns>
        protected JObject GetApiDoc(ExceptionContext context) {
            var parameters = context.ActionDescriptor.Parameters;
            if (parameters.Count > 0) {
                if (parameters.Count == 1) {
                    var parameter = parameters[0];
                    if (parameter.ParameterType.IsPrimitive()) {
                        var json = new JObject();
                        json.Add(parameter.Name, parameter.ParameterType.Name);
                        return json;
                    }
                    else {
                        return parameter.ParameterType.GetXDoc();
                    }
                }
                else {
                    var json = new JObject();
                    foreach (var item in parameters) {
                        if (item.ParameterType.IsPrimitive()) {
                            json.Add(item.Name, item.ParameterType.Name);
                        }
                        else {
                            json.Add(item.Name, item.ParameterType.GetXDoc());
                        }
                    }

                    return json;
                }
            }

            return null;
        }
    }
}
