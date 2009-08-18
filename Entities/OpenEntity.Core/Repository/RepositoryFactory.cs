using System;
using System.Collections.Generic;
using Centro.OpenEntity.DataProviders;
using Centro.OpenEntity.Model;

namespace Centro.OpenEntity.Repository
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private static Dictionary<IDataProvider, RepositoryFactory> cachedFactories = new Dictionary<IDataProvider, RepositoryFactory>();

        public static RepositoryFactory GetRepositoryFactoryFor(IDataProvider dataProvider)
        {
            if (cachedFactories.ContainsKey(dataProvider))
                return cachedFactories[dataProvider];
            var factory = new RepositoryFactory(dataProvider);
            if (!cachedFactories.ContainsKey(dataProvider))
                cachedFactories.Add(dataProvider, factory);
            return factory;
        }

        private IDataProvider dataProvider;
        private Dictionary<Type, object> cachedRepositories = new Dictionary<Type, object>();

        public RepositoryFactory(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            if (!cachedFactories.ContainsKey(dataProvider))
                cachedFactories.Add(dataProvider, this);
        }

        public IRepository<TModel> GetRepository<TModel>() where TModel : Centro.OpenEntity.Model.IDomainObject
        {
            if (cachedRepositories.ContainsKey(typeof(TModel)))
                return (IRepository<TModel>)cachedRepositories[typeof(TModel)];
            var repository = new RepositoryBase<TModel>(dataProvider);
            cachedRepositories.Add(typeof(TModel), repository);
            return repository;
        }

        internal IRepositoryInternal GetRepository(Type modelType)
        {
            if (cachedRepositories.ContainsKey(modelType))
                return (IRepositoryInternal)cachedRepositories[modelType];
            var repositoryType = typeof(RepositoryBase<IDomainObject>).GetGenericTypeDefinition().MakeGenericType(modelType);
            var repository = Activator.CreateInstance(repositoryType, dataProvider);
            cachedRepositories.Add(modelType, repository);
            return (IRepositoryInternal)repository;
        }
    }
}
