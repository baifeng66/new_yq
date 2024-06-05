using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.DAL;
namespace WS.Db.BLL
{
	public class roleMenuBLL : BaseBLL<Model.roleMenu>, IBLL.IroleMenuBLL
	{
		 readonly DAL.roleMenuDAL repo = new DAL.roleMenuDAL();
 		public override void setBaseRepo()
		{
			base.repo = repo;
		}
	}
}