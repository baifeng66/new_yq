using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WS.Db.DAL.Base;
using WS.Db.IBLL;

namespace WS.Db.BLL
{
    /// <summary>
    /// 说明：
    /// 作者：llw
    /// 时间：2023/8/1 11:00:08
    /// </summary>
    public abstract class BaseBLL<TEntity> : IBaseBLL<TEntity> where TEntity : class, new()
    {
        public BaseBLL()
        {
            setBaseRepo();
        }
        public BaseDAL<TEntity> repo;

        /// <summary>
        /// 设置数据实现类
        /// </summary>
        public abstract void setBaseRepo();

        ///// <summary>
        ///// 数据实现层
        ///// </summary>
        //public Repo.BaseRepo<TEntity> repo
        //{
        //    get
        //    {
        //        if (_repo == null)
        //        {
        //            _repo = new Repo.BaseRepo<TEntity>();
        //        }
        //        return _repo;
        //    }
        //}

        ///// <summary>
        ///// 创建UnitOfWork事务
        ///// </summary>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public FreeSql.IRepositoryUnitOfWork GetBusUOW(string tenantid)
        //{
        //    return repo.GetBusUOW(tenantid);
        //}
        /// <summary>
        /// 插入Model
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int Insert(TEntity mode)
        {
            return repo.Insert(mode);
        }
        ///// <summary>
        ///// 插入Model 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public int BusInsert(TEntity mode, string tenantid)
        //{
        //    return repo.BusInsert(mode, tenantid);
        //}
        ///// <summary>
        ///// 插入Model 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public int BusInsert(List<TEntity> list, string tenantid)
        //{
        //    return repo.BusInsert(list, tenantid);
        //}
        ///// <summary>
        ///// 使用事务 插入Model 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public int BusInsertTrans(TEntity mode, string tenantid, IRepositoryUnitOfWork uow)
        //{
        //    return repo.BusInsertTrans(mode, tenantid, uow);
        //}
        ///// <summary>
        ///// 使用事务插入List 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //public int BusInsertTrans(List<TEntity> list, string tenantid, IRepositoryUnitOfWork uow)
        //{
        //    return repo.BusInsertTrans(list, tenantid, uow);
        //}
        /// <summary>
        /// 修改Model
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int Update(TEntity mode)
        {
            return repo.Update(mode);
        }
        /// <summary>
        /// 修改Model
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int UpdateCol(TEntity mode, Expression<Func<TEntity, object>> cols)
        {
            return repo.UpdateCol(mode, cols);
        }
        /// <summary>
        /// 删除数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} |  new{id=1}
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public int Delete(object obj)
        {
            return repo.Delete(obj);
        }

        /// <summary>
        /// 根据ID查询记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetModel(int id)
        {
            return repo.GetModel(id);
        }

        ///// <summary>
        ///// 根据ID查询记录
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public TEntity GetModelByCache(int id)
        //{
        //    string key = typeof(TEntity).Name + "_" + id;
        //    TEntity Tc = Core.CacheHelper.Get(key) as TEntity;
        //    if (Tc == null)
        //    {
        //        Tc = GetModel(id);
        //        Core.CacheHelper.Add(key, Tc);
        //    }
        //    return Tc;
        //}
        /// <summary>
        /// 根据条件查询记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetModel(Expression<Func<TEntity, bool>> exp)
        {
            return repo.GetModel(exp);
        }

        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> exp)
        {
            return repo.GetAll(exp);
        }

        /// <summary>
        /// 查询符合条件的记录总数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public long Total(Expression<Func<TEntity, bool>> exp)
        {
            return repo.Total(exp);
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
            return repo.PageOrderBy(exp, orderfied, startIndex, pagesize);
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
            return repo.PageOrderByDesc(exp, orderfied, startIndex, pagesize);
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public List<TEntity> All()
        {
            return repo.All();
        }

        public List<TEntity> expAll(Expression<Func<TEntity, bool>> exp)
        {
            return repo.expAll(exp);
        }

        public List<TEntity> AllOrderDes<TMember>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TMember>> orderfied)
        {
            return repo.AllOrderDes(exp, orderfied);
        }
    }
}
