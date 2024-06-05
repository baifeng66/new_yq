using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.IBLL;
using WS.Db.DAL;
using Microsoft.AspNetCore.Mvc;
using WS.Db.Model;
using WS.Db.BLL;
using System.Linq;
using System.Linq.Expressions;

namespace WS.API.Controllers
{
    /// <summary>
    /// 数据字典控制
    /// </summary>
	public class baseDictController : BaseController
    {
        IHttpContextAccessor _accessor;
        IbaseDictBLL _service;

        ///<summary>
        /// 构造函数
        /// </summary>
        /// <param name="iservice"></param>
        /// <param name="ibaseLogBLL"></param>
        /// <param name="accessor"></param>
        public baseDictController(IbaseDictBLL iservice, IbaseLogBLL ibaseLogBLL, IHttpContextAccessor accessor) : base(ibaseLogBLL, accessor)
        {
            _service = iservice;
            _accessor = accessor;

        }

        /// <summary>
        /// 获取数据字典表所有记录
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(baseDict))]
        public IActionResult listall()
        {
            List<baseDict> list = _service.All().OrderByDescending(c => c.dictId).ToList();
            return success(list.Count, list);
        }

        /// <summary>
        /// 删除指定id的数据字典记录
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpDelete]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult delete([FromQuery] int id)
        {
            var b = _service.Delete(id);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Delete, $"字典删除成功，ID：{id}");
                return success("删除成功");
            }
            else
            {
                return fail("删除失败");
            }
        }

        /// <summary>
        /// 添加数据字典记录
        /// </summary>
        /// <param name="mode">字典的实体集合</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult add([FromBody] baseDict mode)
        {
            baseDict dict = _service.GetModel(c => c.dictCode == mode.dictCode && c.dictPcode == mode.dictPcode);
            if (dict != null)
            {
                return fail("该数据字典已存在");
            }
            if (string.IsNullOrEmpty(mode.dictName))
            {
                return fail("字典说明不可为空");
            }
            var b = _service.Insert(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Add, $"字典添加成功，字典ID：{mode.dictId}");
                return success("添加成功");
            }
            else
            {
                return fail("添加失败");
            }
        }
        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="mode">字典的实体集合</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult edit([FromBody] baseDict mode)
        {
            var b = _service.Update(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"字典修改成功，字典ID：{mode.dictId}");
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
        /// <param name="dictName">单位名称</param>
        /// <param name="dictCode">字典编码</param>
        /// <param name="dictPCode">上级编码</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(baseDict))]
        public IActionResult page(string? dictName = "", string? dictCode = "", string? dictPCode = "", int page = 1, int size = 20)
        {
            Expression<Func<baseDict, bool>> exp = c => c.dictId > 0;
            if (!string.IsNullOrEmpty(dictName))
            {
                exp = exp.And(c => c.dictName.Contains(dictName));
            }
            if (!string.IsNullOrEmpty(dictCode))
            {
                exp = exp.And(c => c.dictCode.Equals(dictCode));
            }
            if (!string.IsNullOrEmpty(dictPCode))
            {
                exp = exp.And(c => c.dictPcode.Equals(dictPCode));
            }
            var list = _service.PageOrderByDesc(exp, c => c.dictId, (page - 1) * size, size);
            return success(list.Count, list);
        }
    }
}