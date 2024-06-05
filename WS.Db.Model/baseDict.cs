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
	public partial class baseDict {

		/// <summary>
		/// 字典ID
		/// </summary>
		[JsonProperty, Column(IsPrimary = true, IsIdentity = true, InsertValueSql = "nextval('\"baseDict_dictId_seq\"'::regclass)")]
		public int dictId { get; set; }

		/// <summary>
		/// 字典编码，唯一键索引
		/// </summary>
		[JsonProperty, Column(StringLength = 8, IsNullable = false)]
		public string dictCode { get; set; }

		/// <summary>
		/// 字典说明
		/// </summary>
		[JsonProperty, Column(StringLength = 256, IsNullable = false)]
		public string dictName { get; set; }

		/// <summary>
		/// 上级编码，base为一级编码
		/// </summary>
		[JsonProperty, Column(StringLength = 8, IsNullable = false)]
		public string dictPcode { get; set; }

		/// <summary>
		/// 状态，0-禁用，1-启用
		/// </summary>
		[JsonProperty, Column(StringLength = 8, IsNullable = false)]
		public string state { get; set; }

	}

}
