using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 传智Studio.DAL
{
    /// 通过反射的形式创建数据操作类的实例
    public class AbstractFactory
    {
        private static readonly string AssemblyPath = ConfigurationManager.AppSettings["AssemblyPath"];
        private static readonly string NameSpace = ConfigurationManager.AppSettings["NameSpace"];
        public static UserInfoDal CreateUserInfoDal()
        {

            string fullClassName = NameSpace + ".UserInfoDal";
            return CreateInstance(fullClassName) as UserInfoDal;
        }
        public static BulletinDal CreateBulletinDal()
        {

            string fullClassName = NameSpace + ".BulletinDal";
            return CreateInstance(fullClassName) as BulletinDal;
        }
        public static RecruitDal CreateRecruitDal()
        {

            string fullClassName = NameSpace + ".RecruitDal";
            return CreateInstance(fullClassName) as RecruitDal;
        }
        public static ProjectDal CreateProjectDal()
        {

            string fullClassName = NameSpace + ".ProjectDal";
            return CreateInstance(fullClassName) as ProjectDal;
        }
        public static VoteDal CreateVoteDal()
        {

            string fullClassName = NameSpace + ".VoteDal";
            return CreateInstance(fullClassName) as VoteDal;
        }
        private static object CreateInstance(string className)
        {
            var assembly = Assembly.Load(AssemblyPath);
            return assembly.CreateInstance(className);
        }
    }
}
