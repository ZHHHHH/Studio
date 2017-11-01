using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 传智Studio.DAL;

namespace 传智Studio.BLL
{
    public abstract class BaseService<T> where T : class,new()
    {
        public abstract void SetCurrentDal();
        //拿到数据层的实体操作类
        public BaseDal<T> CurrentDal { get; set; }
        public BaseService()
        {
            SetCurrentDal();//子类一定要实现抽象方法。
        }
        //拿到数据会话层对象
        public DBSession CurrentDBSession
        {
            get
            {
                return DBSessionFactory.CreateDBSession();
            }
        }
        #region 查询分页
     public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda)
       {
           return CurrentDal.LoadPageEntities<s>(pageIndex, pageSize, out totalCount, whereLambda, orderbyLambda);
       }
        #endregion
        #region 查询
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
        }
        #endregion
        #region 删除
        public bool DeleteEntity(T entity)
        {
            CurrentDal.DeleteEntity(entity);
             return CurrentDBSession.SaveChanges();
        }
        #endregion
        #region 更新
        public bool EditEntity(T entity)
        {
            CurrentDal.EditEntity(entity);
             return CurrentDBSession.SaveChanges();
        }
        #endregion
        #region 添加
        public bool AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);
             return CurrentDBSession.SaveChanges();
        }
        #endregion
    }
}
