// Copyright (c) RigoFunc (xuyingting). All rights reserved.

using Newtonsoft.Json.Linq;

namespace RigoFunc.ApiCore.Internal {
    internal class ApiResult {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Debug { get; set; }

        public static ApiResult NotFound(string message = null) => new ApiResult {
            Code = 404,
            Message = message ?? "NOT FOUND"
        };

        public static ApiResult BadRequest(string message, string debug = null) => new ApiResult {
            Code = 400,
            Message = message,
            Debug = debug
        };
    }

    internal class ApiResult<T> : ApiResult {
        public ApiResult() {

        }

        public ApiResult(T data) {
            Data = data;
        }

        public T Data { get; set; }
    }
}
