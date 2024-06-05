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
	public partial class userInfo {

		/// <summary>
		/// 用户ID
		/// </summary>
		[JsonProperty, Column(IsPrimary = true, IsIdentity = true, InsertValueSql = "nextval('\"userInfo_userId_seq\"'::regclass)")]
		public int userId { get; set; }

		/// <summary>
		/// 添加人
		/// </summary>
		[JsonProperty]
		public int createBy { get; set; }

		/// <summary>
		/// 添加时间
		/// </summary>
		[JsonProperty]
		public DateTime createTime { get; set; }

		/// <summary>
		/// 上一次登录时间
		/// </summary>
		[JsonProperty]
		public DateTime? lastLoginTime { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[JsonProperty, Column(StringLength = 16, IsNullable = false)]
		public string loginName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[JsonProperty, Column(StringLength = 64, IsNullable = false)]
		public string loginPwd { get; set; }

		/// <summary>
		/// 手机号
		/// </summary>
		[JsonProperty, Column(StringLength = 16)]
		public string phoneNum { get; set; }

		/// <summary>
		/// 角色ID
		/// </summary>
		[JsonProperty]
		public int rid { get; set; }

		/// <summary>
		/// 状态，0-禁用，1-启用
		/// </summary>
		[JsonProperty, Column(StringLength = 8, IsNullable = false)]
		public string state { get; set; }

		/// <summary>
		/// 真实姓名
		/// </summary>
		[JsonProperty, Column(StringLength = 16)]
		public string trueName { get; set; }

		/// <summary>
		/// 修改人
		/// </summary>
		[JsonProperty]
		public int? updateBy { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		[JsonProperty]
		public DateTime? updateTime { get; set; }

	}

}
