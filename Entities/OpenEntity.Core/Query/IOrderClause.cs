using System.Data.SqlClient;

namespace Centro.OpenEntity.Query
{
    public interface IOrderClause
    {
        string Table { get; }
        string Column { get; }
        SortOrder Direction { get; }
    }
}
