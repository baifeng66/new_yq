using System;
using System.Collections.Generic;
using System.Text;

namespace WS.Core
{
    /// <summary>
    /// 功能描述    ：接口相应实体  
    /// 创 建 者    ：pc
    /// 创建日期    ：2020/11/11 16:27:23 
    /// 最后修改者  ：pc
    /// 最后修改日期：2020/11/11 16:27:23 
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 代码：0-成功，1-失败，2-异常，100-登录过期
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 错误信息描述
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 记录条数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 具体的记录
        /// </summary>
        public object data { get; set; }
        public ApiResponse() { }
        public ApiResponse(int code, string msg, int total, object value)
        {
            this.code = code;
            this.msg = msg;
            this.count = total;
            this.data = value;
        }
    }
}
