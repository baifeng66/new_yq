using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Core;

namespace WS.API
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {

        readonly IWebHostEnvironment _env;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            Log4Helper.Error(" Message:" + context.Exception.Message
                + " headers=" + Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Request.Headers)
                + " QueryString=" + Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Request.QueryString)
                + " Form=" + Newtonsoft.Json.JsonConvert.SerializeObject(context.HttpContext.Request.Form)
                , context.Exception);

            ApiResponse json = new ApiResponse() { code = 2, msg = context.Exception.Message };
            context.Result = new ApplicationErrorResult(json);
            //context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            context.ExceptionHandled = true;
        }

        public class ApplicationErrorResult : ObjectResult
        {
            public ApplicationErrorResult(object value) : base(value)
            {
                //StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            }
        }
    }
}
