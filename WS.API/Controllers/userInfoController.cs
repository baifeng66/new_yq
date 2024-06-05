using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.IBLL;
using WS.Db.DAL;
using Microsoft.AspNetCore.Mvc;
using WS.Db.Model;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WS.Core;
using System.Net;

namespace WS.API.Controllers
{
    /// <summary>
    /// 用户控制
    /// </summary>
	public class userInfoController : BaseController
    {

        IuserInfoBLL _service;
        IHttpContextAccessor _accessor;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iservice"></param>
        /// <param name="ibaseLogBLL"></param>
        /// <param name="accessor"></param>
        public userInfoController(IuserInfoBLL iservice, IbaseLogBLL ibaseLogBLL, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor) : base(ibaseLogBLL, accessor)
        {
            _service = iservice;
            _accessor = accessor;
        }

        ///// <summary>
        ///// 获取用户表所有信息
        ///// </summary>
        ///// <returns></returns>
        //[Microsoft.AspNetCore.Authorization.Authorize]
        //[HttpGet]
        //[Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(userInfo))]
        //public IActionResult listall()
        //{
        //    var list = _service.All().OrderByDescending(c => c.userId).ToList();
        //    return success(list.Count, list);
        //}

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户的id</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpDelete]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult delete([FromQuery] int id)
        {
            var b = _service.Delete(id);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Delete, "用户信息删除成功");
                return success("删除成功");
            }
            else
            {
                return fail("删除失败");
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="mode">用户实体</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult add([FromBody]userInfo mode)
        {

            userInfo user = _service.GetModel(c => c.loginName == mode.loginName);
            if (user != null)
            {
                return fail("该登录账户已存在！");
            }

            mode.lastLoginTime = null;

            // 随机生成盐，并对密码进行加密
            string salt = "abc1234567";
            mode.loginPwd = Md5Helper.MD5Encrypt32(mode.loginPwd, salt);
            var b = _service.Insert(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Add, $"添加用户信息成功");
                return success("添加成功");
            }
            else
            {
                return fail("添加失败");
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult edit([FromBody] userInfo mode)
        {
            userInfo user = _service.GetModel(c => c.userId == mode.userId);
            mode.loginPwd = user.loginPwd;
            mode.lastLoginTime = user.lastLoginTime; 

            var b = _service.Update(mode);
            if (b != 0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"修改用户信息成功");
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
        /// <param name="loginName">用户名</param>
        /// <param name="userId">用户ID</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(userInfo))]
        public IActionResult page(string? loginName = "", int? userId =null, int page = 1, int size = 20)
        {
            Expression<Func<userInfo, bool>> exp = c => c.userId > 0;
            if (!string.IsNullOrEmpty(loginName))
            {
                exp = exp.And(c => c.loginName.Contains(loginName));
            }
            if (userId > -1)
            {
                exp = exp.And(c => c.userId == userId);
            }
            var list = _service.PageOrderByDesc(exp, c => c.userId, (page - 1) * size, size);
            return success(list.Count, list);
        }

        /// <summary>
        /// 禁用/启用一个用户
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(WS.Core.ApiResponse))]
        public IActionResult forbidOrActive([FromQuery] int userId)
        {
            if (userId <= 0)
            {
                return fail("用户不存在");
            }
            userInfo user = _service.GetModel(c => c.userId == userId);
            if (user == null)
            {
                return fail("用户不存在");
            }
            if(user.state =="0" || user.state == "string")
                user.state = "1";
            else if (user.state =="1")
                user.state = "0";

            int count = _service.Update(user);
            if (count > 0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"修改用户状态成功");
                return success("修改成功");
            }
            else
            {
                return fail("修改失败");
            }
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "数据列表", Type = typeof(userInfo))]
        public IActionResult getCurrentUser()
        {
            userInfo user = _service.GetModel(uid);
            return success(1, user);
        }


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">用户Md5加密密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(string))]
        public IActionResult Authenticate([FromQuery] string username, [FromQuery] string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return fail("请输入用户名和密码");
            }
            string salt = "abc1234567";
            userInfo userinfo = _service.GetModel(c => c.loginName == username || c.phoneNum == username);
            if (userinfo != null && !string.IsNullOrEmpty(userinfo.loginName))
            {
                if (userinfo.loginPwd.ToLower() != password.ToLower())
                {
                    return fail("用户名密码错误");
                }
                else
                {
                    if (!userinfo.state.Equals("1"))
                    {
                        return fail("用户状态错误");
                    }

                    SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("hsj1234567890abcdefhsj1234567890abcdef"));
                    var clientIpAddress = _accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault(); // 解决 nginx、docker等 获取ip问题
                    if (string.IsNullOrEmpty(clientIpAddress))
                        clientIpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    string timestamp = DateTime.Now.DateTimeToTimestamp().ToString();
                    SecurityToken securityToken = new JwtSecurityToken(
                        signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                        //issuer: "Issuer", //发布者
                        //audience: "Audience",//订阅者
                        expires: DateTime.UtcNow.AddDays(1),
                        claims: new Claim[] { 
                             //这里就是定义的角色（比如这里定义了TestUser角色和test@qq.com邮箱）
                             new Claim(ClaimTypes.Role,userinfo.rid.ToString()),
                             new Claim(ClaimTypes.Name,userinfo.trueName),
                             new Claim("id",userinfo.userId.ToString()),
                             new Claim("timestamp",timestamp)
                        }
                        );

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

                    //_ilogin.Insert(new Model.user_login { addtime = DateTime.Now, loginip = clientIpAddress, logintype = type, token = timestamp, uid = userinfo.userId });
                    //Core.CacheHelper.Remove("lastlogin_" + uid);
                    //string cname = "未知";
                    //int theme = 0;
                    //Model.base_company companymode = _company.GetModelByCache(userinfo.cid);
                    //if (companymode != null && companymode.id > 0)
                    //{
                    //    cname = companymode.name;
                    //    theme = companymode.theme;
                    //}
                    base.addlog(baseLog.ApiLogType.Login, $"生态环保文书系统登录成功");
                    //logged("生产系统登录成功", Model.base_log.LogType.Login_out);
                    DateTime? tmp=DateTime.Now;
                    if (userinfo.lastLoginTime == null)//第一次登录，将上次登录时间设为现在时间
                    {
                        userinfo.lastLoginTime = DateTime.Now;
                        _service.Update(userinfo);
                    }
                    else if (userinfo.lastLoginTime != null)//非首次登录
                    {
                         tmp = userinfo.lastLoginTime;
                        userinfo.lastLoginTime = DateTime.Now;
                        _service.Update(userinfo);
                    }
                    return success(1, new { token = jwtToken, loginName = userinfo.loginName, uid = userinfo.userId ,lastLoginTime= tmp });
                }

            }
            else
            {
                return fail("用户不存在");
            }
        }
        /// <summary>
        /// 前台用户修改任意用户的密码
        /// </summary>
        /// <returns></returns>
        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerResponse(200, "操作结果", Type = typeof(Core.ApiResponse))]
        public IActionResult UpdateAnyPassword([FromQuery] int user_id, [FromQuery] string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                return fail("密码不可为空");
            }
            userInfo user = _service.GetModel(c => c.userId == user_id);
            if (user == null)
            {
                return fail("该用户不存在");
            }
            string salt = "abc1234567";
            user.loginPwd = Md5Helper.MD5Encrypt32(newPassword, salt);
            int count = _service.Update(user);

            if (count > 0)
            {
                base.addlog(baseLog.ApiLogType.Update, $"修改用户:{user.loginName}密码成功");
                return success("密码修改成功");
            }
            else
            {
                return fail("密码修改失败");
            }


        }
    }
}