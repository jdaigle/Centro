using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity.Entities
{
    /// <summary>
    /// Enum definition for the state an Entity can be in.
    /// </summary>
    public enum EntityState : int
    {
        /// <summary>
        /// Entity is new. It can be empty or filled, but is not saved (yet) to the persistent storage.
        /// </summary>
        New,
        /// <summary>
        /// Entity is filled with its data from the persistent storage. It can be changed since the fetch.
        /// </summary>
        Fetched,
        /// <summary>
        /// Entity is out of sync with its physical entity in the persistent storage. 
        /// An Entity is marked OutOfSync when a succesful Save action is performed. 
        /// An Entity will re-fetch itself when in the state OutOfSync when a property
        /// is read or Refetch() is called. The state will then be set to Fetched.
        /// </summary>
        OutOfSync,
        /// <summary>
        /// If an entity has the state Deleted, it is no longer
        /// available in the persistent storage.
        /// </summary>
        Deleted
    }
}
