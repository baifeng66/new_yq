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
	public partial class keywordGroup {

		/// <summary>
		/// 关键字分组id
		/// </summary>
		[JsonProperty, Column(IsPrimary = true, IsIdentity = true, InsertValueSql = "nextval('\"keywordGroup_id_seq\"'::regclass)")]
		public int id { get; set; }

		/// <summary>
		/// 关键字分组名称
		/// </summary>
		[JsonProperty, Column(IsNullable = false)]
		public string name { get; set; }

	}

}
