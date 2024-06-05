using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.DAL;
namespace WS.Db.BLL
{
	public class regionBLL : BaseBLL<Model.region>, IBLL.IregionBLL
	{
		 readonly DAL.regionDAL repo = new DAL.regionDAL();
 		public override void setBaseRepo()
		{
			base.repo = repo;
		}
	}
}