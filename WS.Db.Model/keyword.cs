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
	public partial class keyword {

		/// <summary>
		/// 关键字id
		/// </summary>
		[JsonProperty, Column(IsPrimary = true, IsIdentity = true, InsertValueSql = "nextval('keyword_id_seq'::regclass)")]
		public int id { get; set; }

		/// <summary>
		/// 关键字分组id
		/// </summary>
		[JsonProperty]
		public int gid { get; set; }

		/// <summary>
		/// 关键字名称
		/// </summary>
		[JsonProperty, Column(StringLength = 20, IsNullable = false)]
		public string keywordName { get; set; }

	}

}
