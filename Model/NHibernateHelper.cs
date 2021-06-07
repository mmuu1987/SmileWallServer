using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using SmileWallServer.AppCode;


/// <summary>
    /// 链接数据库和服务器，能对数据库进行增删改查,用来把对象模型表示的对象映射到基于SQL的关系模型数据结构中去
    /// </summary>
    class NHibernateHelper
    {


        private static ISessionFactory _sessionFactory;//单例模式

        private static void InitializeSessionFactory()
        {
            //_sessionFactory =
            //    Fluently.Configure().Database(MySQLConfiguration.Standard.ConnectionString(db => db.Server(Common.MySqlServer).Database(Common.Smilewall).Username(Common.Username).Password(Common.Password)))
            //        .Mappings(x => x.FluentMappings.AddFromAssemblyOf<NHibernateHelper>()).BuildSessionFactory();
            Log.Info("NHibernateHelper".ToString(), "表单数据是：" +string.Format("server = {0}; user id = {1}; password = {2}; database = {3};port={4};character set=utf8", Common.MySqlServer, Common.Username, Common.Password, Common.Smilewall, Common.MySqlPort)) ;
            _sessionFactory =
                Fluently.Configure().Database(MySQLConfiguration.Standard.ConnectionString(string.Format("server = {0}; user id = {1}; password = {2}; database = {3};port={4};character set=utf8", Common.MySqlServer, Common.Username, Common.Password, Common.Smilewall, Common.MySqlPort)))
                    .Mappings(x => x.FluentMappings.AddFromAssemblyOf<NHibernateHelper>()).BuildSessionFactory();

        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    InitializeSessionFactory();
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }


