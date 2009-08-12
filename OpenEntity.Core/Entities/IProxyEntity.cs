using System;
using OpenEntity.Query;
using OpenEntity.Model;
using OpenEntity.Schema;

namespace OpenEntity.Entities
{
    public interface IProxyEntity : IEntity
    {
        bool Initialized { get; }

        void Initialize(ITable table, IEntityFields fields);

        IPredicateExpression GetPrimaryKeyPredicateExpression();

        void AddCustomTypeConverter(ICustomTypeConverter customTypeConverter, string propertyName);

        ICustomTypeConverter GetCustomTypeConverter(string propertyName);
    }
}
