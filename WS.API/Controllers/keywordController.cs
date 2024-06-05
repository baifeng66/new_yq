using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WS.Db.IBLL;
using WS.Db.Model;

namespace WS.API.Controllers;
/// <summary>
/// 关键字控制
/// </summary>
public class keywordController:BaseController
{
     IkeywordBLL _service;
     IHttpContextAccessor _accessor;

    public keywordController(IbaseLogBLL ibaseLogBLL, IHttpContextAccessor accessor, IkeywordBLL service) : base(ibaseLogBLL, accessor)
    {
        _service = service;
        _accessor = accessor;
    }

    /// <summary>
    /// 翻页查询数据
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="gid"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200,"数据列表",Type = typeof(keyword))]
    public IActionResult page(string? keyName, int? gid = null, int page = 1, int size = 20)
    {
        // 条件表达式树
        Expression<Func<keyword, bool>> exp = k => k.id > 0;
        
        // 模糊查询
        if (!string.IsNullOrEmpty(keyName))
        {
            exp = exp.And(k => k.keywordName.Contains(keyName));
        }
        // 判断关键字分组id是否合法
        if (gid > 0)
        {
            exp = exp.And(k => k.gid == gid);
        }
        // 分页查询数据
        var list = _service.PageOrderByDesc(exp, k => k.id, (page - 1) * size, size);
        return success(list.Count, list);
    }

    /// <summary>
    /// 修改、新增关键字
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(200,"操作结果",Type = typeof(baseLog.ApiLogType))]
    public JsonResult save([FromBody] keyword mode)
    {
        if (mode.id == 0)//新增
        {
            if (string.IsNullOrEmpty(mode.keywordName))
            {
                return fail("关键字名称不能为空");
            }

            var b = _service.Insert(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Add, $"关键字添加成功，关键字ID:{mode.id}");
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
                base.addlog(baseLog.ApiLogType.Update, $"关键字修改成功，关键字ID：{mode.id}");
                return success("修改成功");
            }
            else
            {
                return fail("修改失败");
            }
        }
        
    }

    /// <summary>
    /// 删除关键字
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