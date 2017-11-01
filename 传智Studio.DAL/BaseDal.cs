using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 传智Studio.DAL
{
    public  class BaseDal<T>where T:class, new()
    {
        DbContext Db = DAL.DBContextFactory.CreateDbContext();

        #region 查询分页
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda)
        {
            var temp = Db.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            totalCount = GetPageCount(pageSize, totalCount);
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageIndex = pageIndex > totalCount ? totalCount : pageIndex;
            temp = temp.OrderByDescending<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            return temp;
        }
        #endregion
        #region 获取总页数
        public int GetPageCount(int pageSize, int totalCount)
        {
            int pageCount = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize));
            return pageCount;
        } 
        #endregion
        #region 查询过滤
       public  IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return Db.Set<T>().Where<T>(whereLambda);
        } 
        #endregion
        #region 删除
       public  bool DeleteEntity(T entity)
        {
            Db.Entry<T>(entity).State = EntityState.Deleted;
            return true;
        }
        #endregion
        #region 更新
       public  bool EditEntity(T entity)
        {
            Db.Entry<T>(entity).State = EntityState.Modified;
            return true;
        }
        #endregion
        #region 添加数据
       public  bool AddEntity(T entity)
        {
            Db.Set<T>().Add(entity);
            return true;
        }
        #endregion
    }
}
