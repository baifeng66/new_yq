using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.DAL;
namespace WS.Db.BLL
{
	public class baseMenuBLL : BaseBLL<Model.baseMenu>, IBLL.IbaseMenuBLL
	{
		 readonly DAL.baseMenuDAL repo = new DAL.baseMenuDAL();
 		public override void setBaseRepo()
		{
			base.repo = repo;
		}
	}
}