using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Entities
{
    public interface IEntityCreator
    {
        object Create();
    }
}
