using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class BaseResponse
    {
        public BaseResponse(bool success, string? message = null)
        {
            this.Success = success;
            this.Message = message;
        }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class BaseResponse<TResult> : BaseResponse
    {
        public BaseResponse(bool success, TResult? result, string? message = null)
            : base(success, message)
        {
            this.Data = result;
        }

        public TResult? Data { get; set; }
    }
}
