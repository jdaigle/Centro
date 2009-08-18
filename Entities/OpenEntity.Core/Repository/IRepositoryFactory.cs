using OpenEntity.Model;
using System;

namespace OpenEntity.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TModel> GetRepository<TModel>() where TModel : IDomainObject;
    }
}
