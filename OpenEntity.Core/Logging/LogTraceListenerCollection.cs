using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace OpenEntity.Logging
{
    public sealed class LogTraceListenerCollection : Collection<TraceListener>, IList, ICollection, IEnumerable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogTraceListenerCollection"/> class.
        /// </summary>
        internal LogTraceListenerCollection()
        {
        }
    }
}
