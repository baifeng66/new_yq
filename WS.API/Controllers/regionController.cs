using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.IBLL;
using WS.Db.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WS.Db.Model;

namespace WS.API.Controllers
{
    /// <summary>
    /// 行政区域控制
    /// </summary>
	public class regionController : BaseController
    {
        IregionBLL _service;
        IHttpContextAccessor _accessor;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iservice"></param>
        /// <param name="ibaseLogBLL"></param>
        /// <param name="accessor"></param>
        public regionController(IregionBLL iservice, IbaseLogBLL ibaseLogBLL, IHttpContextAccessor accessor) : base(ibaseLogBLL, accessor)
        {
            _service = iservice;
            _accessor = accessor;
        }

        ///// <summary>
        ///// 获取行政区域表所有记录
        ///// </summary>
        ///// <returns></returns>
        //[Microsoft.AspNetCore.Authorization.Authorize]
        //[HttpGet]
        //[Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(region))]

        //public IActionResult listall()
        //{
        //    var list = _service.All().ToList();
        //    return success(list.Count, list);
        //}

        /// <summary>
        /// 删除行政区域
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpDelete]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult delete([FromQuery] string name)
        {
            var b = _service.Delete(name);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Delete, $"删除行政区域成功");
                return success("删除成功");
            }
            else
            {
                return fail("删除失败");
            }

        }

        /// <summary>
        /// 添加行政区域
        /// </summary>
        /// <param name="mode">角色菜单关联的实体集合</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult add([FromBody] region mode)
        {
            if (mode.name == null)
            {
                return fail("区域名称不可为空");
            }
            var b = _service.Insert(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Add, $"添加行政区域成功");
                return success("添加成功");
            }
            else
            {
                return fail("添加失败");
            }
        }

        /// <summary>
        /// 修改行政区域
        /// </summary>
        /// <param name="mode">角色菜单关联的实体集合</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult edit([FromBody] region mode)
        {
            var b = _service.Update(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"修改行政区域成功");
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
        /// <param name="code">区域唯一编码，查询下级数据，level>1时有效</param>
        /// <param name="name">名称</param>
        /// <param name="level">0-全部级别，1-省级，2-市级，3-曲线级</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(region))]
        public IActionResult page(string? code = "", string? name = "", short level = 0, int page = 1, int size = 20)
        {

            Expression<Func<region, bool>> exp = c => c.level > 0;
            if (level > 0)
            {
                exp = exp.And(c => c.level == level);
                if (level == 2)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        exp = exp.And(c => c.province_code == code);
                    }
                }
                else if (level == 3)
                {
                    if (!string.IsNullOrEmpty(code))
                    {
                        exp = exp.And(c => c.city_code == code);
                    }
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                exp = exp.And(c => c.name.Contains(name));
            }
            var list = _service.PageOrderByDesc(exp, c => c.code, (page - 1) * size, size);
            return success(list.Count, list);
        }
    }
}