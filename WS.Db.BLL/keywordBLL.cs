using WS.Db.DAL;
using WS.Db.IBLL;
using WS.Db.Model;

namespace WS.Db.BLL;

public class keywordBLL : BaseBLL<keyword>, IkeywordBLL
{
    private static readonly keywordDAL repo = new keywordDAL();
    public override void setBaseRepo()
    {
        base.repo = repo;
    }
}