using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace OpenEntity.Entities
{
    public interface IEntityFields : IEnumerable<IEntityField>, IEditableObject
    {
        /// <summary>
        /// Gets the number of fields in this collection.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Gets / sets the flag if the contents of the IEntityFields object is 'dirty', which means that one or more fields are changed. 
        /// </summary>
        bool IsDirty { get; set; }
        /// <summary>
        /// Gets / sets the IEntityField on the specified Index. 
        /// </summary>
        /// <exception cref="System.IndexOutOfRangeException">When the index specified is not found in the internal datastorage.</exception>
        IEntityField this[int index] { get; }
        /// <summary>
        /// Gets the IEntityField with the specified name.
        /// </summary>
        /// <exception cref="System.ArgumentException">When the specified name is the empty string or contains only spaces</exception>
        IEntityField this[string name] { get; }
        /// <summary>
        /// List of IEntityField references which form the 'primary key', or uniquely identifying set of values for this set of fields, thus for the entity
        /// holding these fields.
        /// </summary>
        IList<IEntityField> PrimaryKeyFields { get; }
        /// <summary>
        /// The state of the EntityFields object, the heart and soul of every EntityObject.
        /// </summary>
        EntityState State { get; set; }
        /// <summary>
        /// Gets the identity field, if there is one. If not, it returns null.
        /// </summary>
        IEntityField IdentityField { get; }
    }
}
