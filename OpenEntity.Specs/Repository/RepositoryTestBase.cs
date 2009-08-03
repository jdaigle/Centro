using NUnit.Framework;
using OpenEntity.DataProviders;
using OpenEntity.Mapping;
using OpenEntity.Repository;
using OpenEntity.Model;

namespace OpenEntity.Specs.Repository
{
    public abstract class RepositoryTestBase<TModelType> where TModelType : IDomainObject
    {
        protected IRepository<TModelType> Repository { get; private set; }

        [TestFixtureSetUp]
        public void SetupRepository()
        {
            MappingConfig.Clear();
            MappingConfig.AddAssembly(typeof(TestEnvironment).Assembly);

            Repository = new BaseRepository<TModelType>(GetDataProvider());
        }

        protected abstract IDataProvider GetDataProvider();
    }
}
