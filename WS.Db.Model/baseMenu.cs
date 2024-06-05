using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using NpgsqlTypes;
using Npgsql.LegacyPostgis;

namespace WS.Db.Model {

	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class baseMenu {

		/// <summary>
		/// 菜单ID
		/// </summary>
		[JsonProperty, Column(IsPrimary = true, IsIdentity = true, InsertValueSql = "nextval('\"baseMenu_menuId_seq\"'::regclass)")]
		public int menuId { get; set; }

		/// <summary>
		/// 添加时间
		/// </summary>
		[JsonProperty]
		public DateTime createTime { get; set; }

		/// <summary>
		/// 菜单图标
		/// </summary>
		[JsonProperty, Column(StringLength = 32, IsNullable = false)]
		public string menuIcon { get; set; }

		/// <summary>
		/// 菜单名称
		/// </summary>
		[JsonProperty, Column(StringLength = 32, IsNullable = false)]
		public string menuName { get; set; }

		/// <summary>
		/// 上级菜单
		/// </summary>
		[JsonProperty]
		public int menuPid { get; set; } = 0;

		/// <summary>
		/// 菜单链接
		/// </summary>
		[JsonProperty, Column(StringLength = 128, IsNullable = false)]
		public string menuUrl { get; set; }

		/// <summary>
		/// 打开方式，当前页面/新窗口
		/// </summary>
		[JsonProperty, Column(StringLength = 8, IsNullable = false)]
		public string openWay { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		[JsonProperty]
		public short orderBy { get; set; } = 0;

		/// <summary>
		/// 状态，0-禁用，1-启用
		/// </summary>
		[JsonProperty, Column(StringLength = 8, IsNullable = false)]
		public string state { get; set; }

	}

}
