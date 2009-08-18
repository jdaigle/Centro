using System;
using OpenEntity.Query;
using OpenEntity.Model;
using OpenEntity.Schema;
using OpenEntity.DataProviders;

namespace OpenEntity.Entities
{
    public interface IProxyEntity : IEntity
    {
        bool Initialized { get; }
        void Initialize(ITable table, IEntityFields fields, IDataProvider dataProvider);
        IPredicateExpression GetPrimaryKeyPredicateExpression();
        void AddCustomTypeConverter(ICustomTypeConverter customTypeConverter, string propertyName);
        ICustomTypeConverter GetCustomTypeConverter(string propertyName);
        void Reload();
        event EventHandler Reloaded;
    }
}
