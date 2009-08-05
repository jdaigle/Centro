using System;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using OpenEntity.Mapping;

namespace OpenEntity.CodeDom
{
    public class ProxyGenerator : BaseGenerator
    {
        private const string headerCodeSegment =
            @"using System;
              using System.Collections.Generic;
              using System.ComponentModel;
              using OpenEntity;
              using OpenEntity.Entities;
              using OpenEntity.Schema;
              using OpenEntity.Query;
              namespace OpenEntity.Proxies {";

        private const string classDefCodeSegment =
            @"public class Proxy_{0} : {1}, IProxyEntity {{
                private EntityDataObject proxy;
                // ctor
                public Proxy_{0}(ITable table) {{ this.proxy = new EntityDataObject(table); }}
                // IProxyEntity
                bool IProxyEntity.Initialized {{ get {{ return this.proxy.Initialized; }} }}
                void IProxyEntity.Initialize(IEntityFields fields) {{ this.proxy.Initialize(fields); }}
                IPredicateExpression IProxyEntity.GetPrimaryKeyPredicateExpression() {{ return this.proxy.GetPrimaryKeyPredicateExpression(); }}
                // IEntity
                ITable IEntity.Table {{ get {{ return this.proxy.Table; }} }}
                bool IEntity.SetNewFieldValue(string fieldName, object value) {{ return proxy.SetNewFieldValue(fieldName, value); }}
                bool IEntity.SetNewFieldValue(int fieldIndex, object value) {{ return proxy.SetNewFieldValue(fieldIndex, value); }}
                object IEntity.GetCurrentFieldValue(string fieldName) {{ return proxy.GetCurrentFieldValue(fieldName); }}
                bool IEntity.IsNew {{ get {{ return proxy.IsNew; }} set {{ proxy.IsNew = value; }} }}
                bool IEntity.IsDirty {{ get {{ return proxy.IsDirty; }} set {{ proxy.IsDirty = value; }} }}
                Guid IEntity.EntityObjectID {{ get {{ return proxy.EntityObjectID; }} }}
                IEntityFields IEntity.Fields {{ get {{ return proxy.Fields; }} }}
                IList<IEntityField> IEntity.PrimaryKeyFields {{ get {{ return proxy.PrimaryKeyFields; }} }}
                object IEntity.GetCurrentFieldValue(int fieldIndex) {{ return proxy.GetCurrentFieldValue(fieldIndex); }}
                //IEditableObject
                void IEditableObject.BeginEdit() {{ proxy.BeginEdit(); }}
                void IEditableObject.CancelEdit() {{ proxy.CancelEdit(); }}
                void IEditableObject.EndEdit() {{ proxy.EndEdit(); }}
                ";

        private const string propertyOverrideCodeSegment =
            @"public override {0} {1} {{
                get {{ return ({0})proxy.GetCurrentFieldValue({2}); }}
                set {{ proxy.SetNewFieldValue({2}, value); base.{1} = value; }} }}";
        
        private readonly IClassConfiguration classConfiguration;

        public ProxyGenerator(IClassConfiguration classConfiguration)
        {
            this.classConfiguration = classConfiguration;
        }

        public Type Build()
        {
            this.InitCompiler();
            return this.Build(this.GenerateCode());
        }

        protected override void OnInitCompiler()
        {
            var classAssembly = this.classConfiguration.ClassType.Assembly;
            this.AddAssembly(classAssembly.Location);

            foreach (var referencedName in classAssembly.GetReferencedAssemblies())
            {
                var referencedAssembly = Assembly.Load(referencedName);
                this.AddAssembly(referencedAssembly.Location);
            }
        }

        /// <summary>
        /// Build the generated code
        /// </summary>
        /// <param name="code">Generated code</param>
        /// <returns>An instance of the generated class</returns>
        private Type Build(string code)
        {
            CompilerResults res = this.Compile(code);

            if (res.Errors.HasErrors)
                throw new InvalidOperationException(res.Errors[0].ErrorText);

            var assembly = res.CompiledAssembly;
            System.Type[] types = assembly.GetTypes();
            if (types.Length == 0)
                throw new InvalidOperationException("Could not find compiled proxy type.");
            return types[0];
        }

        /// <summary>
        /// Check if the property is public
        /// </summary>
        /// <remarks>
        /// <para>If IsPublic==true I can directly set the property</para>
        /// <para>If IsPublic==false I need to use the setter/getter</para>
        /// </remarks>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private bool IsPublic(string propertyName)
        {
            return this.classConfiguration.ClassType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public) != null;
        }

        /// <summary>
        /// Generate the required code
        /// </summary>
        /// <returns>C# code</returns>
        private string GenerateCode()
        {
            var sb = new StringBuilder();

            sb.Append(headerCodeSegment + Environment.NewLine);
            sb.AppendFormat(classDefCodeSegment, 
                            this.classConfiguration.ClassType.FullName.Replace('.', '_').Replace("+", "__"),
                            this.classConfiguration.ClassType.FullName);

            foreach (var property in this.classConfiguration.Properties)
            {
                sb.AppendFormat(propertyOverrideCodeSegment,
                                property.PropertyInfo.PropertyType.FullName,
                                property.Name,
                                "\""+property.Column+"\"");
                sb.Append(Environment.NewLine);
            }

            sb.Append("}\n}\n"); // Close class and namespace

            return sb.ToString();
        }

    }
}
