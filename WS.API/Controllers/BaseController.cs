using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WS.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WS.Db.IBLL;
using WS.Db.Model;

namespace WS.API.Controllers
{
    /// <summary>
    /// 基础类
    /// </summary>
    //[Microsoft.AspNetCore.Authorization.Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    [EnableCors("any")]
    public class BaseController : Controller
    {
        //readonly public IServices.Iuser_loginService _iloginservice;
        /// <summary>
        /// 
        /// </summary>
        readonly public IbaseLogBLL _ibaseLogBLL;
        readonly public Microsoft.AspNetCore.Http.IHttpContextAccessor _accessor;
        
        /// <summary>
        /// 基础类创建
        /// </summary>
        ///// <param name="iloginservice"></param>
        /// <param name="ibaseLogBLL"></param>
        /// <param name="accessor"></param>
        public BaseController(IbaseLogBLL ibaseLogBLL, IHttpContextAccessor accessor)
        {
            _ibaseLogBLL = ibaseLogBLL;
            _accessor = accessor;
        }
        /// <summary>
        /// 0-web,1-安卓，2-ios
        /// </summary>
        public int LoginType
        {
            get
            {
                string type = Request.Headers["type"];
                if (!string.IsNullOrEmpty(type) && type != "null")
                {
                    bool b = int.TryParse(type, out int t);
                    if (b)
                    {
                        return t;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// 用户的ID
        /// </summary>
        public int uid
        {
            get
            {
                var id = _accessor?.HttpContext?.User?.FindFirst("id");
                if (id != null && !string.IsNullOrEmpty(id.Value))
                {
                    return Convert.ToInt32(id.Value);
                }
                return 0;
            }

        }

        /// <summary>
        /// 获取传入的token
        /// </summary>
        public string Token
        {
            get
            {
                return Request.Headers["token"];
            }
        }
        
        protected JsonResult success()
        {
            ApiResponse res = new ApiResponse() { code = 0, msg = "成功。" };
            return Json(res);
        }

        protected JsonResult success(string msg)
        {
            ApiResponse res = new ApiResponse() { code = 0, msg = msg };
            return Json(res);
        }

        protected JsonResult success(int total, object val)
        {
            ApiResponse res = new ApiResponse() { code = 0, msg = "成功", count = total, data = val };
            Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
            setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            setting.NullValueHandling = NullValueHandling.Ignore;

            //System.Text.Json.JsonSerializerOptions setting = new System.Text.Json.JsonSerializerOptions();
            //setting.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());//应时间格式
            //setting.Converters.Add(new SystemTextJsonConvert.DateTimeNullableConverter()); //应时间格式
            return Json(res, setting);
        }


        protected JsonResult fail()
        {
            ApiResponse res = new ApiResponse() { code = 1, msg = "失败。" };
            return Json(res);
        }

        protected JsonResult fail(string msg)
        {
            ApiResponse res = new ApiResponse() { code = 1, msg = msg };
            return Json(res);
        }

        protected JsonResult error()
        {
            ApiResponse res = new ApiResponse() { code = 2, msg = "内部异常。" };
            return Json(res);
        }

        protected JsonResult loginout()
        {
            ApiResponse res = new ApiResponse() { code = 100, msg = "登录已过期。" };
            return Json(res);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="msg">消息内容，拼接在类型字符串后面</param>
        /// <returns></returns>
        protected int addlog(baseLog.ApiLogType type, string msg)
        {
            baseLog log = new baseLog();
            log.createTime = DateTime.Now;
            log.content = $"{type}，{msg}";
            log.logtype = type.ToString();
            log.userId = uid;
            return _ibaseLogBLL.Insert(log);
        }

    }
}
