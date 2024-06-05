using System;
using System.Collections.Generic;
using System.Text;
using WS.Db.DAL;
namespace WS.Db.BLL
{
	public class userInfoBLL : BaseBLL<Model.userInfo>, IBLL.IuserInfoBLL
	{
		 readonly DAL.userInfoDAL repo = new DAL.userInfoDAL();
 		public override void setBaseRepo()
		{
			base.repo = repo;
		}
	}
}