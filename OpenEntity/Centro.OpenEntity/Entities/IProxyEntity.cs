using System;
using Centro.OpenEntity.Query;
using Centro.OpenEntity.Model;
using Centro.OpenEntity.Schema;
using Centro.OpenEntity.DataProviders;

namespace Centro.OpenEntity.Entities
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
