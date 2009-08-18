using System;
using System.Collections.Generic;
using System.ComponentModel;
using Centro.OpenEntity.Schema;

namespace Centro.OpenEntity.Entities
{
    public interface IEntity : IEditableObject
    {
        ITable Table { get; }

        object GetCurrentFieldValue(string fieldName);

        object GetCurrentFieldValue(int fieldIndex);

        bool SetNewFieldValue(string fieldName, object value);

        bool SetNewFieldValue(int fieldIndex, object value);

        bool IsNew { get; set; }

        bool IsDirty { get; set; }

        Guid EntityObjectID { get; }

        IEntityFields Fields { get; }

        IList<IEntityField> PrimaryKeyFields { get; }
    }
}
