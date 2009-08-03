using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenEntity.Schema;

namespace OpenEntity.Entities
{
    /// <summary>
    /// Represents a repository entity, which has a related table schema (the entity is not a table). It also
    /// has a collection of IEntityField which is editable.
    /// </summary>
    public interface IEntity : IEditableObject
    {
        /// <summary>
        /// Gets the schema for the table this entity is related to.
        /// </summary>
        ITable Table { get; }
        /// <summary>
        /// Gets the current value of the Field with the index fieldIndex. Will refetch the complete entity's fields
        /// from the database if necessary (i.e. the entity is outofsync.).
        /// </summary>
        /// <param name="fieldIndex">Index of Field to get the current value of</param>
        /// <returns>The current value of the Field specified</returns>
        /// <exception cref="EntityIsDeletedException">When the entity is marked as deleted.</exception>
        /// <exception cref="ArgumentOutOfRangeException">When fieldIndex is smaller than 0 or bigger than the amount of fields in the fields collection.</exception>
        object GetCurrentFieldValue(int fieldIndex);
        /// <summary>
        /// Sets the Field with the name fieldName to the new value value. Also marks the fields as dirty. Will also refetch the complete entity's fields
        /// from the database if necessary (i.e. the entity is outofsync.).
        /// </summary>
        /// <param name="fieldName">Name of Field to set the new value of</param>
        /// <param name="value">Value to set</param>
        /// <returns>true if the value is actually set, false otherwise.</returns>
        /// <exception cref="ValueTypeMismatchException">The value specified is not of the same IEntityField.DataType as the field.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value specified has a size that is larger than the maximum size defined for the related column in the databas</exception>
        bool SetNewFieldValue(string fieldName, object value);
        /// <summary>
        /// Sets the Field with the name fieldName to the new value value. Also marks the fields as dirty. Will refetch the complete entity's fields
        /// from the database if necessary (i.e. the entity is outofsync.).
        /// </summary>
        /// <param name="fieldIndex">Index of Field to set the new value of</param>
        /// <param name="value">Value to set</param>
        /// <returns>true if the value is actually set, false otherwise.</returns>
        bool SetNewFieldValue(int fieldIndex, object value);
        /// <summary>
        /// Gets the current value of the Field with the name fieldName. Will refetch the complete entity's fields
        /// from the database if necessary (i.e. the entity is outofsync.).
        /// </summary>
        /// <param name="fieldName">Name of Field to get the current value of</param>
        /// <returns>The current value of the Field specified</returns>
        /// <exception cref="EntityIsDeletedException">When the entity is marked as deleted.</exception>
        object GetCurrentFieldValue(string fieldName);
        /// <summary>
        /// Marker for the entity object if the object is new and should be inserted when saved (true) or read from the
        /// database (false).
        /// </summary>
        bool IsNew { get; set; }
        /// <summary>
        /// Marker for the entity object if the object is 'dirty' (changed, true) or not (false).
        /// </summary>
        bool IsDirty { get; set; }
        /// <summary>
        /// Gets the unique Entity Object ID which is created at runtime when the entity is instantiated. Can be used for external caches.
        /// </summary>
        Guid EntityObjectID { get; }
        /// <summary>
        /// Gets a collection of fields for this entity.
        /// </summary>
        IEntityFields Fields { get; }
        /// <summary>
        /// Gets a list of the fields which form the primary key for this entity.
        /// </summary>
        IList<IEntityField> PrimaryKeyFields { get; }
        ///// <summary>
        ///// Gets an object representing the relationships based off this entity.
        ///// </summary>
        //RelationshipGraphNode RelationshipGraph { get; }
        ///// <summary>
        ///// Fetches the related entities, and recursively any related entities.
        ///// </summary>
        ///// <param name="dataProvider">The data provider.</param>
        //void FetchRelatedEntities(IDataProvider dataProvider);
        ///// <summary>
        ///// Fetches the related entities, and optionally recursively any related entities.
        ///// </summary>
        ///// <param name="dataProvider">The data provider.</param>
        ///// <param name="recursive">if set to <c>true</c> recursively fetch the related entities.</param>
        //void FetchRelatedEntities(IDataProvider dataProvider, bool recursive);
    }
}
