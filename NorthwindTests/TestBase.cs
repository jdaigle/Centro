using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using FluentNHibernate.Cfg.Db;
using Centro.Data;
using System.Reflection;

namespace NorthwindTests
{
    public class TestBase
    {
        private ISession session;
        private ISessionFactory sessionFactory;
        private NHibernate.Cfg.Configuration nhibernateConfiguration;

        protected ISession GetNewSession()
        {
            if (sessionFactory == null)
            {
                var configurer = MsSqlConfiguration.MsSql2005.ConnectionString(c => c.Server(@".\SQLEXPRESS").Database("Northwind").TrustedConnection());
                var configuration = FluentConfigurationBuilder.CreateFluentConfiguration(configurer, new List<Assembly> { typeof(TestBase).Assembly }, ref nhibernateConfiguration);
                sessionFactory = configuration.BuildSessionFactory();
            }
            return sessionFactory.OpenSession();
        }

        protected ISession GetSession()
        {
            if (session == null)
            {
                session = GetNewSession();                               
            }
            return session;
        }
    }
}
