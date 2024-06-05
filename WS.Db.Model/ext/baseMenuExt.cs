using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Db.Model.ext
{
    /// <summary>
    /// 说明：
    /// 作者：llw
    /// 时间：2023/8/15 10:19:54
    /// </summary>
    public class baseMenuExt:baseMenu
    {
        /// <summary>
        /// 角色
        /// </summary>
        [JsonProperty]
        public roleMenu role { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        [JsonProperty]
        public List<baseMenu> children { get; set; } = new List<baseMenu>();
    }
}
