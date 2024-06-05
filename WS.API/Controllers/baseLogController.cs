using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.IBLL;
using WS.Db.DAL;
using Microsoft.AspNetCore.Mvc;
using WS.Db.Model;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace WS.API.Controllers
{
    /// <summary>
    /// 日志控制
    /// </summary>
	public class baseLogController : BaseController
	{
        IHttpContextAccessor _accessor;
        IbaseLogBLL _service;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iservice"></param>
        /// <param name="ibaseLogBLL"></param>
        /// <param name="accessor"></param>
        public baseLogController(IbaseLogBLL iservice,IbaseLogBLL ibaseLogBLL, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor) : base(ibaseLogBLL, accessor)
		{
			_service = iservice;
            _accessor = accessor;
        }

        ///// <summary>
        ///// 获取日志表所有记录
        ///// </summary>
        ///// <returns></returns>
        //[Microsoft.AspNetCore.Authorization.Authorize]
        //[HttpGet]
        //[Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(baseLog))]
        //public IActionResult listall()
        //{
        //    var list = _service.All().OrderByDescending(c => c.logId).ToList();
        //    return success(list.Count, list);
        //}

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="id">日志的id</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpDelete]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult delete([FromQuery] int id)
        {
            var b = _service.Delete(id);
            if (b!=0)
            {
                base.addlog(baseLog.ApiLogType.Delete, $"日志删除成功，ID：{id}");
                return success("删除成功");
            }
            else
            {
                return fail("删除失败");
            }
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="mode">日志的实体集合</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult add([FromBody] baseLog mode)
        {
             if (string.IsNullOrEmpty(mode.content))
            {
                return fail("日志内容不可为空");
            }
            var b = _service.Insert(mode);
            if (b!=0)
            {
                base.addlog(baseLog.ApiLogType.Add, $"日志添加成功，日志ID：{mode.logId}");
                return success("添加成功");
            }
            else
            {
                return fail("添加失败");
            }
        }

        /// <summary>
        /// 修改日志
        /// </summary>
        /// <param name="mode">日志的实体集合</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult edit([FromBody]  baseLog mode)
        {
            var b = _service.Update(mode);
            if (b!=0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"日志修改成功，日志ID：{mode.logId}");
                return success("修改成功");
            }
            else
            {
                return fail("修改失败");
            }
        }
        /// <summary>
        /// 翻页查询数据
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="userId">用户ID</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(baseLog))]
        public IActionResult page(string content="", int userId = -1, int page = 1, int size = 20)
        {

            Expression<Func<baseLog, bool>> exp = c => c.logId > 0;
            if (!string.IsNullOrEmpty(content))
            {
                exp = exp.And(c => c.content.Contains(content));
            }
            if (userId > -1)
            {
                exp = exp.And(c => c.userId == userId);
            }
            var list = _service.PageOrderByDesc(exp, c => c.logId, (page - 1) * size, size);
            return success(list.Count, list);
        }
    }
}