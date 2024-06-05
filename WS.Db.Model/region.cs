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
	public partial class region {

		/// <summary>
		/// 城市编码
		/// </summary>
		[JsonProperty, Column(StringLength = 12)]
		public string city_code { get; set; }

		/// <summary>
		/// 区域唯一编码
		/// </summary>
		[JsonProperty, Column(StringLength = 12)]
		public string code { get; set; }

		/// <summary>
		/// 级别
		/// </summary>
		[JsonProperty]
		public short? level { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[JsonProperty, Column(StringLength = 256)]
		public string name { get; set; }

		/// <summary>
		/// 省份编码
		/// </summary>
		[JsonProperty, Column(StringLength = 12)]
		public string province_code { get; set; }

		/// <summary>
		/// 全称
		/// </summary>
		[JsonProperty, Column(StringLength = 256)]
		public string qc { get; set; }

	}

}
