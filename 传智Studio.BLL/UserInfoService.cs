using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 传智Studio.DAL;
using 传智Studio.Model;

namespace 传智Studio.BLL
{
    public class UserInfoService:BaseService<UserInfo>
    {
        //拿到数据层的实体操作对象
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.userInfoDal;
        }
    }
}
