using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Centro.OpenEntity
{
    public static class SqlDbClientTypeName
    {
        /// <summary>
        /// Client name for the Microsoft SQL Server native client.
        /// </summary>
        public const string MSSQL = "System.Data.SqlClient";
        /// <summary>
        /// Client name for the Microsoft native client for Oracle.
        /// </summary>
        public const string MSORACLE = "System.Data.OracleClient";
        /// <summary>
        /// Client name for the Oracle Data Access provider.
        /// </summary>
        public const string ORACLE = "Oracle.DataAccess";
        /// <summary>
        /// Client name for the Microsoft native client for OleDb.
        /// </summary>
        public const string OLEDB = "System.Data.OleDb";
    }
}
