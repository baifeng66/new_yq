using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.DAL;
namespace WS.Db.BLL
{
	public class baseLogBLL : BaseBLL<Model.baseLog>, IBLL.IbaseLogBLL
	{
		 readonly DAL.baseLogDAL repo = new DAL.baseLogDAL();
 		public override void setBaseRepo()
		{
			base.repo = repo;
		}
	}
}