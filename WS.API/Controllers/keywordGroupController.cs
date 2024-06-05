using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WS.Db.IBLL;
using WS.Db.Model;

namespace WS.API.Controllers;

/// <summary>
/// 关键字分组控制
/// </summary>
public class keywordGroupController : BaseController
{
    IkeywordGroupBLL _service;
    IHttpContextAccessor _accessor;

    public keywordGroupController(IbaseLogBLL ibaseLogBLL, IHttpContextAccessor accessor, IkeywordGroupBLL service) :
        base(ibaseLogBLL, accessor)
    {
        _service = service;
        _accessor = accessor;
    }

    /// <summary>
    /// 翻页查询数据
    /// </summary>
    /// <param name="name"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "数据列表", Type = typeof(keywordGroup))]
    public IActionResult page(string? name, int page = 1, int size = 20)
    {
        Expression<Func<keywordGroup, bool>> exp = k => k.id > 0;
        if (!string.IsNullOrEmpty(name))
        {
            exp = exp.And(k => k.name.Contains(name));
        }

        var list = _service.PageOrderByDesc(exp, k => k.id, (page - 1) * size, size);
        return success(list.Count, list);
    }

    /// <summary>
    /// 修改、新增关键字分组
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(200, "操作结果", Type = typeof(baseLog.ApiLogType))]
    public JsonResult save([FromBody] keywordGroup mode)
    {
        if (mode.id == 0) //新增
        {
            if (string.IsNullOrEmpty(mode.name))
            {
                return fail("关键字名称不能为空");
            }

            var b = _service.Insert(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Add, $"关键字分组添加成功，关键字分组ID:{mode.id}");
                return success("添加成功");
            }
            else
            {
                return fail("添加失败");
            }
        }
        else //修改
        {
            var b = _service.Update(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"关键字分组修改成功，关键字分组ID：{mode.id}");
                return success("修改成功");
            }
            else
            {
                return fail("修改失败");
            }
        }
    }
    
    /// <summary>
    /// 删除关键字分组
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [SwaggerResponse(200,"操作结果",Type = typeof(baseLog.ApiLogType))]
    public JsonResult delete([FromQuery] int id)
    {
        var b = _service.Delete(id);
        if (b != 0)
        {
            base.addlog(baseLog.ApiLogType.Delete, $"关键字删除成功,ID：{id}");
            return success("删除成功");
        }
        else
        {
            return fail("删除失败");
        }
    }
}