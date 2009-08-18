using NUnit.Framework;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity.Mapping;
using Centro.OpenEntity.Repository;
using Centro.OpenEntity.Model;

namespace Centro.OpenEntity.Specs.Repository
{
    public abstract class RepositoryTestBase<TModelType> where TModelType : IDomainObject
    {
        protected IRepository<TModelType> Repository { get; private set; }

        [TestFixtureSetUp]
        public void SetupRepository()
        {
            MappingTable.Clear();
            MappingTable.AddAssembly(typeof(TestEnvironment).Assembly);

            Repository = new RepositoryBase<TModelType>(GetDataProvider());
        }

        protected abstract IDataProvider GetDataProvider();
    }
}
