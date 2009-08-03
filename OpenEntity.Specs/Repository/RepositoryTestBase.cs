using NUnit.Framework;
using OpenEntity.DataProviders;
using OpenEntity.Mapping;
using OpenEntity.Repository;

namespace OpenEntity.Specs.Repository
{
    public abstract class RepositoryTestBase<TEntity>
    {
        protected IRepository<TEntity> Repository { get; private set; }

        [TestFixtureSetUp]
        public void SetupRepository()
        {
            MappingConfig.Clear();
            MappingConfig.AddAssembly(typeof(TestEnvironment).Assembly);

            Repository = new BaseRepository<TEntity>(GetDataProvider());
        }

        protected abstract IDataProvider GetDataProvider();
    }
}
