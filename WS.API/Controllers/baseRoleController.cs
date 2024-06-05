using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.IBLL;
using WS.Db.DAL;
using Microsoft.AspNetCore.Mvc;
using WS.Db.Model;
using System.Linq.Expressions;

namespace WS.API.Controllers
{
    /// <summary>
    /// 角色控制
    /// </summary>
	public class baseRoleController : BaseController
	{
     
        IbaseRoleBLL _service;
        IHttpContextAccessor _accessor;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iservice"></param>
        /// <param name="ibaseLogBLL"></param>
        /// <param name="accessor"></param>
        public baseRoleController(IbaseRoleBLL iservice,IbaseLogBLL ibaseLogBLL, IHttpContextAccessor accessor) : base(ibaseLogBLL,accessor)
		{
			_service = iservice;
            _accessor = accessor;
        }

        /// <summary>
        /// 获取角色表所有记录
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(baseRole))]
        public IActionResult listall()
        {
            var list = _service.All().OrderByDescending(c => c.roleId).ToList();
            return success(list.Count, list);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色的id</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpDelete]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult delete([FromQuery] int id)
        {
            var b = _service.Delete(id);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Delete, $"角色删除成功，ID：{id}");
                return success("删除成功");
            }
            else
            {
                return fail("删除失败");
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="mode">角色的实体集合</param>
        /// <returns></returns>
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult add([FromBody] baseRole mode)
        {
            if (string.IsNullOrEmpty(mode.roleName))
            {
                return fail("角色名称不可为空");
            }
            var b = _service.Insert(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Add, $"角色添加成功，角色ID：{mode.roleId}");
                return success("添加成功");
            }
            else
            {
                return fail("添加失败");
            }
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="mode">角色的实体集合</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public JsonResult edit(baseRole mode)
        {
            var b = _service.Update(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"角色修改成功，角色ID：{mode.roleId}");
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
        /// <param name="roleName">角色名称</param>
        /// <param name="state">状态，0-禁用，1-启用</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(baseRole))]
        public IActionResult page(string roleName = "", string? state = "", int page = 1, int size = 20)
        {
            Expression<Func<baseRole, bool>> exp = c => c.roleId > 0;
            if (!string.IsNullOrEmpty(roleName))
            {
                exp = exp.And(c => c.roleName.Contains(roleName));
            }
            if (!string.IsNullOrEmpty(state))
            {
                exp = exp.And(c => c.state.Equals(state));
            }
            if (state == null)
            {
                exp = exp.And(c => c.state == 0 || c.state == 1);
            }
            var list = _service.PageOrderByDesc(exp, c => c.roleId, (page - 1) * size, size);
            return success(list.Count, list);
        }
    }
}