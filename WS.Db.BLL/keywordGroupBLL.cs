using WS.Db.DAL;
using WS.Db.IBLL;
using WS.Db.Model;

namespace WS.Db.BLL;

public class keywordGroupBLL:BaseBLL<keywordGroup>,IkeywordGroupBLL
{
    private static readonly keywordGroupDAL repo = new keywordGroupDAL();
    public override void setBaseRepo()
    {
        base.repo = repo;
    }
}