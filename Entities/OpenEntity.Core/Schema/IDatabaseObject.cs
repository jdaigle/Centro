
namespace OpenEntity.Schema
{
    /// <summary>
    /// General interface for any database object
    /// </summary>
    public interface IDatabaseObject
    {
        /// <summary>
        /// The name of the corresponding object (column, table, view, stored procedure).
        /// Name cannot be of zero length nor can they consist of solely spaces. Leading and trailing spaces are trimmed.
        /// Used to generate SQL on the fly.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The name of the schema which holds <see cref="Name"/>. Schema is used to generate SQL on the fly. 
        /// A common schema name in SqlServer is f.e. 'dbo'.
        /// </summary>
        string SchemaName { get; }
    }
}
