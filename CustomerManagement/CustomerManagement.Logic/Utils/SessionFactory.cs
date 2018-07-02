using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using NHibernate;

namespace CustomerManagement.Logic.Utils
{
    /// <summary>
    /// NHibernate session factory abstraction
    /// </summary>
    public static class SessionFactory
    {
        private static ISessionFactory _factory;

        /// <summary>
        /// Builds a new session factory.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        public static void Init(string connectionString)
        {
            _factory = BuildSessionFactory(connectionString);
        }

        /// <summary>
        /// Creates a database connection and opens a new session.
        /// </summary>
        /// <returns>NHibernate session</returns>
        internal static ISession OpenSession()
        {
            return _factory.OpenSession();
        }

        /// <summary>
        /// Builds a new session factory.
        /// </summary>
        /// <param name="connectionString">Database connection string</param>
        /// <returns>Session factory interface</returns>
        private static ISessionFactory BuildSessionFactory(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(Assembly.GetExecutingAssembly())
                    .Conventions.Add(
                        ForeignKey.EndsWith("ID"),
                        ConventionBuilder.Property.When(criteria => criteria.Expect(x => x.Nullable, Is.Not.Set), x => x.Not.Nullable()))
                    .Conventions.Add<OtherConventions>()
                    .Conventions.Add<TableNameConvention>()
                    .Conventions.Add<HiLoConvention>()
                );

            return configuration.BuildSessionFactory();
        }


        private class OtherConventions : IHasManyConvention, IReferenceConvention
        {
            public void Apply(IOneToManyCollectionInstance instance)
            {
                instance.LazyLoad();
                instance.AsBag();
                instance.Cascade.SaveUpdate();
                instance.Inverse();
            }

            public void Apply(IManyToOneInstance instance)
            {
                instance.LazyLoad(Laziness.Proxy);
                instance.Cascade.None();
                instance.Not.Nullable();
            }
        }


        public class TableNameConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.Table(instance.EntityType.Name);
            }
        }
        
        public class HiLoConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.Column(instance.EntityType.Name + "ID");
                instance.GeneratedBy.HiLo("Ids", "NextHigh", "9", "EntityName = '" + instance.EntityType.Name + "'");
            }
        }
    }
}
