using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            this.Success = true;
            this.Errors = new Error[] { };
        }

        public BaseResponse(params Error[] errors)
        {
            this.Success = false;
            this.Errors = errors;
        }

        public BaseResponse(bool success, Error[] errors)
        {
            this.Success = success;
            this.Errors = errors;
        }
        
        public bool Success { get; set; }
       
        public Error[] Errors { get; set; }
    }

    public class BaseResponse<TResult> : BaseResponse
    {
        public BaseResponse(TResult result)
            : base()
        {
            this.Data = result;
        }

        public BaseResponse(params Error[] errors)
            : base(errors)
        {
        }

        public BaseResponse(bool success, TResult? result, Error[] errors)
            : base(success, errors)
        {
            this.Data = result;
        }

        public TResult? Data { get; set; }
    }
}
