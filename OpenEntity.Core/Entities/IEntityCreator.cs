using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenEntity.Entities
{
    /// <summary>
    /// Responsible for creating blank IEntities.
    /// </summary>
    public interface IEntityCreator
    {
        IProxyEntity Create();
        IEntityFields CreateEntityFields();
    }
}
