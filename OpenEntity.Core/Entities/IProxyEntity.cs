using System;
using OpenEntity.Query;
using OpenEntity.Model;

namespace OpenEntity.Entities
{
    public interface IProxyEntity : IEntity
    {
        /// <summary>
        /// A value indicated whether the entity has been initialized with an IEntityFields collection.
        /// </summary>
        bool Initialized { get; }
        /// <summary>
        /// Initializes with a set of IEntityFields, which can be empty.
        /// The fields collection generally represents the "row" from a database.
        /// </summary>
        /// <param name="fields">The fields for the entity.</param>
        /// <exception cref="NotSupportedException">Thrown if the entity is already initialized.</exception>
        void Initialize(IEntityFields fields);
        /// <summary>
        /// Gets an IPredicateExpression that represents the primary keys for this entity.
        /// </summary>
        IPredicateExpression GetPrimaryKeyPredicateExpression();
        void AddCustomTypeConverter(ICustomTypeConverter customTypeConverter, string propertyName);
        ICustomTypeConverter GetCustomTypeConverter(string propertyName);
    }
}
