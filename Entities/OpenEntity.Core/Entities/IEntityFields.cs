using System.Collections.Generic;
using System.ComponentModel;

namespace Centro.OpenEntity.Entities
{
    public interface IEntityFields : IEnumerable<IEntityField>, IEditableObject
    {
        int Count { get; }

        bool IsDirty { get; set; }

        IEntityField this[int index] { get; }

        IEntityField this[string name] { get; }

        IList<IEntityField> PrimaryKeyFields { get; }

        EntityState State { get; set; }

        IEntityField IdentityField { get; }
    }
}
