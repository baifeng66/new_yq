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
	public partial class baseLog {

		/// <summary>
		/// 日志ID
		/// </summary>
		[JsonProperty, Column(IsPrimary = true, IsIdentity = true, InsertValueSql = "nextval('\"baseLog_logId_seq\"'::regclass)")]
		public long logId { get; set; }

		/// <summary>
		/// 日志内容
		/// </summary>
		[JsonProperty, Column(StringLength = 512, IsNullable = false)]
		public string content { get; set; }

		/// <summary>
		/// 添加时间
		/// </summary>
		[JsonProperty]
		public DateTime createTime { get; set; }

		/// <summary>
		/// 日志类型
		/// </summary>
		[JsonProperty, Column(StringLength = 8, IsNullable = false)]
		public string logtype { get; set; }

		/// <summary>
		/// 用户ID
		/// </summary>
		[JsonProperty]
		public int userId { get; set; }
		
		/// <summary>
		/// 数据接口日志类型
		/// </summary>
		public enum ApiLogType
		{
			Login=0,
			Exit=1,
			Add=2,
			Update=3,
			Delete=4,
		}
	}

}
