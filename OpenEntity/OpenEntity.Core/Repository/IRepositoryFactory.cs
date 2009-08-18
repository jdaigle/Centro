using Centro.OpenEntity.Model;
using System;

namespace Centro.OpenEntity.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TModel> GetRepository<TModel>() where TModel : IDomainObject;
    }
}
