using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.DAL;
namespace WS.Db.BLL
{
	public class baseDictBLL : BaseBLL<Model.baseDict>, IBLL.IbaseDictBLL
	{
		 readonly DAL.baseDictDAL repo = new DAL.baseDictDAL();
 		public override void setBaseRepo()
		{
			base.repo = repo;
		}
	}
}