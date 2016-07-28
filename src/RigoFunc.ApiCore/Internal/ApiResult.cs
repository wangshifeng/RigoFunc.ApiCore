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
    }

    internal class ApiResult<T> : ApiResult {
        public static ApiResult<bool> True() {
            return new ApiResult<bool> {
                Code = 0,
                Data = true
            };
        }

        public static ApiResult<T> BadRequest(T data, string message) {
            return new ApiResult<T> {
                Data = data,
                Message = message
            };
        }

        public ApiResult() {

        }

        public ApiResult(T data) {
            Data = data;
        }

        public T Data { get; set; }
    }
}
