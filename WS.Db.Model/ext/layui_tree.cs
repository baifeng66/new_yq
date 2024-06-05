using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Db.Model.ext
{
    /// <summary>
    /// 功能描述    ：树形控件数据  
    /// 创 建 者    ：pc
    /// 创建日期    ：2020/11/30 9:21:21 
    /// 最后修改者  ：pc
    /// 最后修改日期：2020/11/30 9:21:21 
    /// </summary>
    public class layui_tree
    {
        /// <summary>
        /// 显示的内容
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///  	节点唯一索引值，用于对指定节点进行各类操作
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 节点字段名
        /// </summary>
        public string field { get; set; }

        /// <summary>
        /// 点击节点弹出新窗口对应的 url。需开启 isJump 参数
        /// </summary>
        public string href { get; set; }

        /// <summary>
        /// 是否默认展开
        /// </summary>
        public bool spread { get; set; } = false;

        /// <summary>
        /// 节点是否初始为选中状态（如果开启复选框的话），默认 false
        /// </summary>
        public bool @checked { get; set; } = false;
        /// <summary>
        /// 节点是否为禁用状态。默认 false
        /// </summary>
        public bool disabled { get; set; } = false;

        /// <summary>
        /// 子节点
        /// </summary>
        public List<layui_tree> children { get; set; }
    }
}
