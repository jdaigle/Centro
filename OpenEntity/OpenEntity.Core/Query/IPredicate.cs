using System.Collections.Generic;
using System.Data;
using Centro.OpenEntity.DataProviders;

namespace Centro.OpenEntity.Query
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
