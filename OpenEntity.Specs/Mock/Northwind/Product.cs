using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenEntity.Model;
using OpenEntity.Repository;
using OpenEntity.Query;

namespace OpenEntity.Specs.Mock.Northwind
{
    public class Product : IDomainObject
    {
        public virtual int Id { get { return default(int); } }
        public virtual string Name { get; set; }
        public virtual bool Discontinued { get; set; }
        public virtual object CategoryId { get; set; }

        public Category GetRelatedCategory(IRepository<Category> categoryRepository)
        {
            var predicate = new PredicateExpression().Where<Category>(c => c.Id).IsEqualTo(CategoryId);
            return categoryRepository.Fetch(predicate);
        }
    }
}
