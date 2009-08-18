using System.ComponentModel;
using Centro.OpenEntity.Schema;

namespace Centro.OpenEntity.Entities
{
    public interface IEntityField : IColumn, IEditableObject
    {
        object CurrentValue { get; set; }

        bool IsChanged { get; set; }

        bool IsNull { get; set; }
    }
}
