using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.DAL;
namespace WS.Db.BLL
{
	public class baseRoleBLL : BaseBLL<Model.baseRole>, IBLL.IbaseRoleBLL
	{
		 readonly DAL.baseRoleDAL repo = new DAL.baseRoleDAL();
 		public override void setBaseRepo()
		{
			base.repo = repo;
		}
	}
}