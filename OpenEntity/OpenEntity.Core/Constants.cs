

using System;

namespace Centro.OpenEntity
{

    /// <summary>
    /// 
    /// </summary>
    public static class AggregateFunctionName
    {
        public const string AVERAGE = "AVG";
        public const string COUNT = "COUNT";
        public const string MAX = "MAX";
        public const string MIN = "MIN";
        public const string SUM = "SUM";
    }

    /// <summary>
    /// Summary for the SqlFragment class
    /// </summary>
    public static class SqlFragment
    {
        public const string AND = " AND ";
        public const string AS = " AS ";
        public const string ASC = " ASC";
        public const string BETWEEN = " BETWEEN ";
        public const string DELETE_FROM = "DELETE FROM ";
        public const string DESC = " DESC";
        public const string DISTINCT = "DISTINCT ";
        public const string EQUAL_TO = " = ";
        public const string FROM = " FROM ";
        public const string IN = " IN ";
        
        public const string INNER_JOIN = " INNER JOIN ";
        public const string OUTER_JOIN = " OUTER JOIN ";
        public const string RIGHT_JOIN = " RIGHT JOIN ";
        public const string LEFT_JOIN = " LEFT JOIN ";
        public const string RIGHT_INNER_JOIN = " RIGHT INNER JOIN ";
        public const string LEFT_INNER_JOIN = " LEFT INNER JOIN ";
        public const string RIGHT_OUTER_JOIN = " RIGHT OUTER JOIN ";
        public const string LEFT_OUTER_JOIN = " LEFT OUTER JOIN ";
        public const string CROSS_JOIN = " CROSS JOIN ";
        public const string UNEQUAL_JOIN = " JOIN ";

        public const string INSERT_INTO = "INSERT INTO ";
        public const string JOIN_PREFIX = "J";
        public const string NOT_EQUAL_TO = " <> ";
        public const string NOT_IN = " NOT IN ";
        public const string ON = " ON ";
        public const string OR = " OR ";
        public const string ORDER_BY = " ORDER BY ";
        public const string SELECT = "SELECT ";
        public const string SET = " SET ";
        public const string SPACE = " ";
        public const string TOP = "TOP ";
        public const string UPDATE = "UPDATE ";
        public const string WHERE = " WHERE ";
        public const string HAVING = " HAVING ";
        public const string GROUP_BY = " GROUP BY ";
    }

    /// <summary>
    /// Summary for the SqlComparison class
    /// </summary>
    public static class SqlComparison
    {
        public const string BLANK = " ";
        public const string EQUAL = " = ";
        public const string GREATER = " > ";
        public const string GREATER_OR_EQUAL = " >= ";
        public const string IS = " IS ";
        public const string IS_NOT = " IS NOT ";
        public const string LESS = " < ";
        public const string LESS_OR_EQUAL = " <= ";
        public const string LIKE = " LIKE ";
        public const string NOT_EQUAL = " <> ";
        public const string NOT_LIKE = " NOT LIKE ";
    }

    /// <summary>
    /// Summary for the SqlSchemaVariable class
    /// </summary>
    public static class SqlSchemaVariable
    {
        public const string COLUMN_DEFAULT = "DefaultSetting";
        public const string COLUMN_NAME = "ColumnName";
        public const string CONSTRAINT_TYPE = "constraintType";
        public const string DATA_TYPE = "DataType";
        public const string DEFAULT = "DEFAULT";
        public const string FOREIGN_KEY = "FOREIGN KEY";
        public const string IS_COMPUTED = "IsComputed";
        public const string IS_IDENTITY = "IsIdentity";
        public const string IS_NULLABLE = "IsNullable";
        public const string MAX_LENGTH = "MaxLength";
        public const string MODE = "mode";
        public const string MODE_INOUT = "INOUT";
        public const string NAME = "Name";
        public const string PARAMETER_PREFIX = "@";
        public const string PRIMARY_KEY = "PRIMARY KEY";
        public const string TABLE_NAME = "TableName";
    }

    /// <summary>
    /// Summary for the OracleSchemaVariable class
    /// </summary>
    public static class OracleSchemaVariable
    {
        public const string COLUMN_NAME = "COLUMN_NAME";
        public const string CONSTRAINT_TYPE = "CONSTRAINT_TYPE";
        public const string DATA_TYPE = "DATA_TYPE";
        public const string IS_NULLABLE = "NULLABLE";
        public const string MAX_LENGTH = "CHAR_LENGTH";
        public const string MODE = "IN_OUT";
        public const string MODE_INOUT = "IN/OUT";
        public const string NAME = "ARGUMENT_NAME";
        public const string NUMBER_PRECISION = "DATA_PRECISION";
        public const string NUMBER_SCALE = "DATA_SCALE";
        public const string PARAMETER_PREFIX = ":";
        public const string TABLE_NAME = "TABLE_NAME";
    }

    
}
