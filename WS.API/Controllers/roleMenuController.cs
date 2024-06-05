using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.IBLL;
using WS.Db.DAL;
using Microsoft.AspNetCore.Mvc;
using WS.Db.Model;
using WS.Db.DAL.Base;
using System.Linq.Expressions;
using System.Linq;
using System.Security.Cryptography;

namespace WS.API.Controllers
{
    /// <summary>
    /// 角色菜单关联控制
    /// </summary>
	public class roleMenuController : BaseController
	{
     
        IroleMenuBLL _service;
        IHttpContextAccessor _accessor;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iservice"></param>
        /// <param name="ibaseLogBLL"></param>
        /// <param name="accessor"></param>
        public roleMenuController(IroleMenuBLL iservice,IbaseLogBLL ibaseLogBLL , IHttpContextAccessor accessor) : base(ibaseLogBLL,accessor)
		{
			_service = iservice;
            _accessor= accessor;


        }

        ///// <summary>
        ///// 获取角色菜单关联表所有记录
        ///// </summary>
        ///// <returns></returns>
        //[Microsoft.AspNetCore.Authorization.Authorize]
        //[HttpGet]
        //[Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(roleMenu))]

        //public IActionResult listall()
        //{
        //    var list = _service.All().OrderByDescending(c => c.roleId).ToList();
        //    return success(list.Count, list);
        //}
        /// <summary>
        /// 删除角色菜单关联表中的一条记录
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpDelete]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult delete([FromQuery] int id)
        {
            var b = _service.Delete(id);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Delete, $"删除角色信息成功");
                return success("删除成功");
            }
            else
            {
                return fail("删除失败");
            }

        }

        /// <summary>
        /// 添加角色菜单关联记录
        /// </summary>
        /// <param name="mode">角色菜单关联的实体集合</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult add([FromBody] roleMenu mode)
        {
            if (mode.roleId==0)
            {
                return fail("角色ID不可为空");
            }
            var b = _service.Insert(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Add, $"添加角色菜单成功");
                return success("添加成功");
            }
            else
            {
                return fail("添加失败");
            }
        }

        /// <summary>
        /// 修改角色菜单关联记录
        /// </summary>
        /// <param name="mode">角色菜单关联的实体集合</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult edit([FromBody] roleMenu mode)
        {
            var b = _service.Update(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"修改角色菜单成功");
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
        /// <param name="roleId">角色ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(roleMenu))]
        public IActionResult page(int roleId = -1, int menuId = -1, int page = 1, int size = 20)
        {

            Expression<Func<roleMenu, bool>> exp = c => c.roleId > 0;
            if (roleId>-1)
            {
                exp = exp.And(c => c.roleId == roleId);
            }
            if (menuId > -1)
            {
                exp = exp.And(c => c.menuId == menuId);
            }
            var list = _service.PageOrderByDesc(exp, c => c.roleId, (page - 1) * size, size);
            return success(list.Count, list);
        }
    }
}