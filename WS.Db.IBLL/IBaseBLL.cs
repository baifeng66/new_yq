using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WS.Db.IBLL
{
    public interface IBaseBLL<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 插入Model
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        //List<TEntity> page(out long recordcount, int id);
        int Insert(TEntity mode);
        ///// <summary>
        ///// 创建UnitOfWork事务
        ///// </summary>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //FreeSql.IRepositoryUnitOfWork GetBusUOW(string tenantid);
        ///// <summary>
        ///// 插入Model 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //int BusInsert(TEntity mode, string tenantid);
        ///// <summary>
        ///// 插入Model 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //int BusInsert(List<TEntity> list, string tenantid);
        ///// <summary>
        ///// 使用事务 插入Model 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //int BusInsertTrans(TEntity mode, string tenantid, IRepositoryUnitOfWork uow);
        ///// <summary>
        ///// 使用事务插入List 到分库
        ///// </summary>
        ///// <param name="mode"></param>
        ///// <param name="tenantid"></param>
        ///// <returns></returns>
        //int BusInsertTrans(List<TEntity> list, string tenantid, IRepositoryUnitOfWork uow);
        /// <summary>
        /// 修改Model
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        int Update(TEntity mode);
        /// <summary>
        /// 修改Model
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        int UpdateCol(TEntity mode, Expression<Func<TEntity, object>> cols);
        /// <summary>
        /// 删除数据，传入动态条件，如：主键值 | new[]{主键值1,主键值2} | TEntity1 | new[]{TEntity1,TEntity2} |  new{id=1}
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        int Delete(object obj);

        /// <summary>
        /// 根据ID查询记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetModel(int id);

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        TEntity GetModel(Expression<Func<TEntity, bool>> exp);
        /// <summary>
        /// 查询符合条件的记录总数
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        long Total(Expression<Func<TEntity, bool>> exp);
        /// <summary>
        /// 升序分页查询
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="exp"></param>
        /// <param name="orderfied"></param>
        /// <param name="startIndex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        List<TEntity> PageOrderBy<TMember>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TMember>> orderfied, int startIndex, int pagesize);

        /// <summary>
        /// 降序分页查询
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="exp"></param>
        /// <param name="orderfied"></param>
        /// <param name="startIndex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        List<TEntity> PageOrderByDesc<TMember>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TMember>> orderfied, int startIndex, int pagesize);

        /// <summary>
        /// 查询所有列表
        /// </summary>
        /// <returns></returns>
        List<TEntity> All();

        /// <summary>
        /// 条件查询所有列表
        /// </summary>
        /// <returns></returns>
        List<TEntity> expAll(Expression<Func<TEntity, bool>> exp);
        /// <summary>
        /// 逆序条件查询所有列表
        /// </summary>
        /// <returns></returns>
        List<TEntity> AllOrderDes<TMember>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TMember>> orderfied);
    }
}
