using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace 传智Studio.DAL
{
   //拿到线程内数据会话层的唯一对象
  public  class DBSessionFactory
    {
      public static DBSession CreateDBSession()
      {
          DBSession DbSession = (DBSession)CallContext.GetData("dbSession");
          if (DbSession == null)
          {
              DbSession = new DBSession();
              CallContext.SetData("dbSession",DbSession);
          }
          return DbSession;
      }
    }
}
