using System.Collections.Generic;
using System.Data;
using OpenEntity.DataProviders;

namespace OpenEntity.Query
{
    public interface IPredicate
    {
        string ToQueryText(IDataProvider dataProvider, ref int uniqueMarker);
        IList<IDataParameter> Parameters { get; }
        bool Negate { get; set; }
        void Clear();
        int Count { get; }
    }
}
