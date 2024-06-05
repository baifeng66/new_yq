using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WS.Core;

namespace WS.Db.DAL.Base
{
    /// <summary>
    /// 功能描述    ：数据实现  
    /// 创 建 者    ：pc
    /// 创建日期    ：2022/5/16 17:53:47 
    /// 最后修改者  ：pc
    /// 最后修改日期：2022/5/16 17:53:47 
    /// </summary>
    public class BaseDAL<TEntity> where TEntity : class, new()// public class baseRepo<TEntity> : IServices.IBaseServices<TEntity> where TEntity : class, new()
    {
        ///// <summary>
        ///// dleBus 空闲对象管理容器
        ///// </summary>
        //static IdleBus<IFreeSql> idleBus = new IdleBus<IFreeSql>(TimeSpan.FromMinutes(10));

        /// <summary>
        /// 主库管理
        /// </summary>
        public IFreeSql freesql
        {
            get
            {
                return FreeSqlInstance.fresql();
            }
        }

        ///// <summary>
        ///// 创建分库的UnitOfWork事务
        ///// </summary>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public IRepositoryUnitOfWork GetBusUOW(string tenantid)
        //{
        //    IFreeSql _freesql = busGet(tenantid);
        //    return _freesql.CreateUnitOfWork();
        //}

        ///// <summary>
        ///// 分库连接
        ///// </summary>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //private IFreeSql busGet(string tenantid)
        //{
        //    if (!idleBus.Exists(tenantid))//不存在则注册
        //    {
        //        //SysTenant modetenant = freesql.Select<SysTenant>().Where(c => c.TenantId == tenantid).First();
        //        //if (modetenant == null)
        //        //{
        //        //    Log4Helper.Error("租户不存在，添加到分库失败");
        //        //    return 0;
        //        //}
        //        SysTenantStorage modetenantstorg = freesql.Select<SysTenantStorage>().Where(c => c.TenantId == tenantid).First();
        //        if (modetenantstorg == null)
        //        {
        //            Log4Helper.Error("租户关联存储不存在，添加到分库失败");
        //            return null;
        //        }
        //        SysStorage modestorage = freesql.Select<SysStorage>().Where(c => c.StorageId == modetenantstorg.StorageId).First();
        //        if (modestorage == null)
        //        {
        //            Log4Helper.Error("存储不存在，添加到分库失败");
        //            return null;
        //        }
        //        int idxs = modestorage.StorageUrl.IndexOf("//");
        //        if (idxs == -1) { idxs = 0; }
        //        string[] addr = modestorage.StorageUrl.Substring(idxs).Split(new char[] { ':', '/' }, StringSplitOptions.RemoveEmptyEntries);
        //        if (addr.Length < 3)
        //        {
        //            Log4Helper.Error("存储格式错误，添加到分库失败");
        //            return null;
        //        }
        //        idleBus.Register(tenantid, () => new FreeSqlBuilder().UseConnectionString(DataType.MySql, $"data source={addr[0]};port={addr[1]};user id={modestorage.Username};password={modestorage.Password};initial catalog={addr[2]};charset=utf8;sslmode=none;max pool size=2").Build());
        //    }
        //    return idleBus.Get(tenantid);
        //}

        ///// <summary>
        ///// 插入Model 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public int BusInsert(TEntity mode, string tenantid)
        //{
        //    try
        //    {
        //        IFreeSql _freesql = busGet(tenantid);
        //        if (_freesql == null)
        //        {
        //            return 0;
        //        }
        //        return _freesql.Insert(mode).ExecuteAffrows();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log4Helper.Error("保存数据失败", ex);
        //    }
        //    return 0;
        //}
        ///// <summary>
        ///// 插入Model 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public int BusInsert(List<TEntity> list, string tenantid)
        //{
        //    try
        //    {
        //        IFreeSql _freesql = busGet(tenantid);
        //        if (_freesql == null)
        //        {
        //            return 0;
        //        }
        //        return _freesql.Insert(list).ExecuteAffrows();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log4Helper.Error("保存数据失败", ex);
        //    }
        //    return 0;
        //}
        ///// <summary>
        ///// 使用事务 插入Model 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public int BusInsertTrans(TEntity mode, string tenantid, IRepositoryUnitOfWork uow)
        //{
        //    try
        //    {
        //        IFreeSql _freesql = busGet(tenantid);
        //        _freesql.CreateUnitOfWork();
        //        if (_freesql == null)
        //        {
        //            return 0;
        //        }
        //        return _freesql.Insert(mode).WithTransaction(uow.GetOrBeginTransaction()).ExecuteAffrows();
        //    }
        //    catch (Exception ex)
        //    {
        //        uow.Rollback();
        //        Log4Helper.Error("保存数据失败", ex);
        //    }
        //    return 0;
        //}
        ///// <summary>
        ///// 使用事务插入List 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public int BusInsertTrans(List<TEntity> list, string tenantid, IRepositoryUnitOfWork uow)
        //{
        //    try
        //    {
        //        IFreeSql _freesql = busGet(tenantid);
        //        if (_freesql == null)
        //        {
        //            return 0;
        //        }
        //        return _freesql.Insert(list).WithTransaction(uow.GetOrBeginTransaction()).ExecuteAffrows();
        //    }
        //    catch (Exception ex)
        //    {
        //        uow.Rollback();
        //        Log4Helper.Error("保存数据失败", ex);
        //    }
        //    return 0;
        //}

        /// <summary>
        /// 插入Model
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int Insert(TEntity mode)
        {
            try
            {
                return freesql.Insert(mode).ExecuteAffrows();
            }
            catch (Exception ex)
            {
                Log4Helper.Error("保存数据失败", ex);
            }
            return 0;
        }

        /// <summary>
        /// 修改Model
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int Update(TEntity mode)
        {
            return freesql.Update<TEntity>().SetSource(mode).Where(mode).ExecuteAffrows();
        }

        /// <summary>
        /// 修改Model
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int UpdateCol(TEntity mode, Expression<Func<TEntity, object>> cols)
        {
            return freesql.Update<TEntity>().SetSource(mode).UpdateColumns(cols).Where(mode).ExecuteAffrows();
        }

        /// <summary>
        /// 删除数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} |  new{id=1}
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int Delete(object obj)
        {
            return freesql.Delete<TEntity>(obj).ExecuteAffrows();
        }

        /// <summary>
        /// 根据ID查询记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetModel(int id)
        {
            return freesql.Select<TEntity>(id).First();
        }

        /// <summary>
        /// 根据条件查询记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetModel(Expression<Func<TEntity, bool>> exp)
        {
            return freesql.Select<TEntity>().Where(exp).First();
        }
        /// <summary>
        /// 根据条件查询记录
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="orderfied"></param>
        /// <returns></returns>
        public TEntity GetModelDesc<TMember>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TMember>> orderfied)
        {
            return freesql.Select<TEntity>().Where(exp).OrderByDescending(orderfied).First();
        }
        /// <summary>
        /// 根据条件查询记录
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="orderfied"></param>
        /// <returns></returns>
        public TEntity GetModelAsc<TMember>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TMember>> orderfied)
        {
            return freesql.Select<TEntity>().Where(exp).OrderBy(orderfied).First();
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> exp)
        {
            return freesql.Select<TEntity>().Where(exp).ToList();
        }

        /// <summary>
        /// 查询符合条件的记录总数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public long Total(Expression<Func<TEntity, bool>> exp)
        {
            return freesql.Select<TEntity>().Where(exp).Count();
        }

        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="exp"></param>
        /// <param name="orderfied"></param>
        /// <param name="startIndex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public List<TEntity> PageOrderBy<TMember>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TMember>> orderfied, int startIndex, int pagesize)
        {
            return freesql.Select<TEntity>().Where(exp).OrderBy(orderfied).Skip(startIndex).Take(pagesize).ToList();
        }

        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="exp"></param>
        /// <param name="orderfied"></param>
        /// <param name="startIndex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public List<TEntity> PageOrderByDesc<TMember>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TMember>> orderfied, int startIndex, int pagesize)
        {
            return freesql.Select<TEntity>().Where(exp).OrderByDescending(orderfied).Skip(startIndex).Take(pagesize).ToList();
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public List<TEntity> All()
        {
            return freesql.Select<TEntity>().ToList();
        }

        /// <summary>
        /// 条件查询所有数据
        /// </summary>
        /// <returns></returns>
        public List<TEntity> expAll(Expression<Func<TEntity, bool>> exp)
        {
            return freesql.Select<TEntity>().Where(exp).ToList();
        }


        public List<TEntity> AllOrderDes<TMember>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TMember>> orderfied)
        {
            return freesql.Select<TEntity>().Where(exp).OrderByDescending(orderfied).ToList();
        }
    }

}
