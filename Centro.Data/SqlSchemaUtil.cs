﻿using System.Collections.Generic;
using NHibernate;
using NHibernate.Dialect;

namespace Centro.Data
{
    public class SqlSchemaUtil
    {
        public static void GenerateSchema(NHibernate.Cfg.Configuration cfg, ISession session)
        {
            var dialect = Dialect.GetDialect(cfg.Properties);
            var drops = cfg.GenerateDropSchemaScript(dialect);
            ExecuteSqlScripts(drops, session);
            var scripts = cfg.GenerateSchemaCreationScript(dialect);
            ExecuteSqlScripts(scripts, session);
        }

        public static void ExecuteSqlScripts(IEnumerable<string> scripts, ISession session)
        {
            foreach (var script in scripts)
            {
                ExecuteSqlScript(script, session);
            }
        }

        public static void ExecuteSqlScript(string script, ISession session)
        {
            var command = session.Connection.CreateCommand();
            command.CommandText = script;
            command.ExecuteNonQuery();
        }
    }
}
