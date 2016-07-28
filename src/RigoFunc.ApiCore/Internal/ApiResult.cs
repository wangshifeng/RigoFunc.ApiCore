// Copyright (c) RigoFunc (xuyingting). All rights reserved.

namespace RigoFunc.ApiCore.Internal {
    internal class ApiResult {
        public int Code { get; set; }

        public string Message { get; set; }

        public static ApiResult NotFound(string message = null) => new ApiResult {
            Code = 404,
            Message = message ?? "NOT FOUND"
        };

        public static ApiResult BadRequest(string message) => new ApiResult {
            Code = 400,
            Message = message,
        };

        public static ApiResult True() => new ApiResult<bool> {
            Code = 0,
            Data = true
        };

        public static ApiResult BadRequest<T>(T data, string message) => new ApiResult<T> {
            Code = 400,
            Data = data,
            Message = message
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
