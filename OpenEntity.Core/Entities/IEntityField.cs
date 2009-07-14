using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using OpenEntity.Schema;

namespace OpenEntity.Entities
{
    /// <summary>
    /// An entity field which holds a value. Inheriets of IColumn to hold all of the readonly schema information about the field.
    /// </summary>
    public interface IEntityField : IColumn, IEditableObject
    {
        /// <summary>
        /// Gets the current value for this field and sets the new value for this field, by overwriting current value. The value in 
        /// currentValue is discarded, versioning control has to save the original value of currentValue before this property is called. 
        /// </summary>
        /// <remarks>
        /// Calling this property directly will not trigger versioning control,
        /// thus calling this property directly is not recommended. Call <see cref="IEntity.SetNewFieldValue(string, object)"/> instead.
        /// Type of the new value has to be the same as IFieldSchema.DataType, which is set in the
        /// constructor. If this field is set to readonly, an exception is raised. 
        /// </remarks>
        /// <exception cref="FieldIsReadonlyException">The field is set to readonly and can't be changed.</exception>
        /// <exception cref="ValueTypeMismatchException">The value specified is not of the same IFieldSchema.DataType as this field.</exception>
        object CurrentValue { get; set; }
        /// <summary>
        /// If the value of this field is changed, this property is set to true. Set when <see cref="CurrentValue"/> receives a valid value. 
        /// </summary>
        bool IsChanged { get; set; }
        /// <summary>
        /// If the original value in the column for this entityfield is DBNull (NULL), this parameter should be set to true, otherwise to false.
        /// In BL Logic, it's impractical to work with NULL values, so these are converted to handable values. The developer can still determine if
        /// the original value was DBNull by checking this field. Using NULL values is not recommended. 
        /// If <see cref="IColumn.IsNullable"/> is false, IsNull is undefined.
        /// </summary>
        bool IsNull { get; set; }
    }
}
