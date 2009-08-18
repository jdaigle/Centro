using System.Data.SqlClient;

namespace OpenEntity.Query
{
    public interface IOrderClause
    {
        string Table { get; }
        string Column { get; }
        SortOrder Direction { get; }
    }
}
