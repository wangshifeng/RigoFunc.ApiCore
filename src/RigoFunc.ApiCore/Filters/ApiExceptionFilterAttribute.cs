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
        /// Calls when an exception occurs.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context) {
            try {
                var loggerFactory = context.HttpContext.RequestServices.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<ApiExceptionFilterAttribute>();
                var env = context.HttpContext.RequestServices.GetService<IHostingEnvironment>();

                var exception = context.Exception;

                string device = context.HttpContext.Request.Headers["Device"];
                if (!string.IsNullOrEmpty(device)) {
                    if (device.Equals("ios", StringComparison.OrdinalIgnoreCase)) {
                        // special logic for IOS
                    }
                    else if (device.Equals("android", StringComparison.OrdinalIgnoreCase)) {
                        var apiResult = ApiResult.BadRequest(exception.Message);
                        if (env.IsDevelopment()) {
                            apiResult.Debug = GetApiDoc(context);
                        }

                        context.Result = new JsonResult(apiResult);
                    }
                }
                else {
                    var json = new JObject();

                    // Error message.
                    json.Add("ErrorMessage", exception.Message);

                    // Api documentation.
                    if (exception is ArgumentNullException && env.IsDevelopment()) {
                        json.Add("ApiDoc", GetApiDoc(context));
                    }

                    // Bad Request
                    context.HttpContext.Response.StatusCode = 400;

                    // Json result.
                    context.Result = new JsonResult(json);
                }

                // log error
                logger.LogError(exception.ToString());
            }
            catch {
                base.OnException(context);
            }
        }

        private JObject GetApiDoc(ExceptionContext context) {
            var parameters = context.ActionDescriptor.Parameters;
            if (parameters.Count > 0) {
                if (parameters.Count == 1) {
                    return parameters[0].ParameterType.GetXDoc();
                }
                else {
                    var json = new JObject();
                    foreach (var item in parameters) {
                        json.Add(item.Name, item.ParameterType.GetXDoc());
                    }

                    return json;
                }
            }

            return null;
        }
    }
}
