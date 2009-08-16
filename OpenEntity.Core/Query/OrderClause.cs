using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using OpenEntity.Mapping;
using OpenEntity.Model;

namespace OpenEntity.Query
{
    public class OrderClause<TModelType> : IOrderClause where TModelType : IDomainObject
    {
        private string table;
        private string column;
        private SortOrder direction;

        public OrderClause(Expression<Func<TModelType, object>> columnExpression)
        {
            var classMapping = MappingTable.FindClassMapping(typeof(TModelType));
            table = classMapping.Table;
            column = classMapping.GetColumnName(columnExpression);
            direction = SortOrder.Ascending;
        }

        public IOrderClause Ascending()
        {
            direction = SortOrder.Ascending;
            return this;
        }

        public IOrderClause Descending()
        {
            direction = SortOrder.Descending;
            return this;
        }

        string IOrderClause.Table { get { return table; } }
        string IOrderClause.Column { get { return column; } }
        SortOrder IOrderClause.Direction { get { return direction; } }
    }
}
