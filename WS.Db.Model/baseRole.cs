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
	public partial class baseRole {

		[JsonProperty, Column(IsPrimary = true, IsIdentity = true, InsertValueSql = "nextval('\"baseRole_roleId_seq\"'::regclass)")]
		public int roleId { get; set; }

		[JsonProperty, Column(DbType = "date")]
		public DateTime createTime { get; set; }

		[JsonProperty, Column(StringLength = 20, IsNullable = false)]
		public string roleName { get; set; }

		[JsonProperty]
		public int state { get; set; }

	}

}
