using System.ComponentModel;
using OpenEntity.Schema;

namespace OpenEntity.Entities
{
    public interface IEntityField : IColumn, IEditableObject
    {
        object CurrentValue { get; set; }

        bool IsChanged { get; set; }

        bool IsNull { get; set; }
    }
}
