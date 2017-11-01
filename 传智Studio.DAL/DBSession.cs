using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 传智Studio.DAL
{
    //数据会话层，专门用于负责完成所有数据操作类实例的创建，然后业务层通过数据会话层来获取要操作数据类的实例。所以数据会话层将业务层与数据层解耦。
    public class DBSession
    {
        //EF对象线程内唯一
        public DbContext Db
        {
            get
            {
                return DBContextFactory.CreateDbContext();
            }
        }
        //统一保存
        public bool SaveChanges()
        {
            return Db.SaveChanges() > 0;
        }
        //拿到数据操作类
        private UserInfoDal _userInfoDal;
        public UserInfoDal userInfoDal
        {
            get
            {
                if (_userInfoDal == null)
                {
                    _userInfoDal = AbstractFactory.CreateUserInfoDal();
                }
                return _userInfoDal;
            }
            set { _userInfoDal = value; }
        }

        private BulletinDal _bulletinDal;
        public BulletinDal bulletinDal
        {
            get
            {
                if (_bulletinDal == null)
                {
                    _bulletinDal = AbstractFactory.CreateBulletinDal();
                }
                return _bulletinDal;
            }
            set { _bulletinDal = value; }
        }

        private RecruitDal _recruitDal;
        public RecruitDal recruitDal
        {
            get
            {
                if (_recruitDal == null)
                {
                    _recruitDal = AbstractFactory.CreateRecruitDal();
                }
                return _recruitDal;
            }
            set { _recruitDal = value; }
        }

        private ProjectDal _projectDal;
        public ProjectDal projectDal
        {
            get
            {
                if (_projectDal == null)
                {
                    _projectDal = AbstractFactory.CreateProjectDal();
                }
                return _projectDal;
            }
            set { _projectDal = value; }
        }

        private VoteDal _votedal;
        public VoteDal votedal
        {
            get
            {
                if (_votedal == null)
                {
                    _votedal = AbstractFactory.CreateVoteDal();
                }
                return _votedal;
            }
            set { _votedal = value; }
        }
    }
}
