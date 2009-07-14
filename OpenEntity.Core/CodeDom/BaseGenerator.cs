using System;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace OpenEntity.CodeDom
{
    public abstract class BaseGenerator
    {
        private readonly CompilerParameters cp = new CompilerParameters();

        /// <summary>
        /// Set up the compiler options
        /// </summary>
        protected void InitCompiler()
        {
            cp.GenerateInMemory = true;
            cp.TreatWarningsAsErrors = true;
#if !DEBUG
				cp.CompilerOptions = "/optimize";
#endif

            AddAssembly(Assembly.GetExecutingAssembly().Location);
            AddAssembly(typeof(BaseGenerator).Assembly.Location);

            foreach (var referencedName in typeof(BaseGenerator).Assembly.GetReferencedAssemblies())
            {
                var referencedAssembly = Assembly.Load(referencedName);
                this.AddAssembly(referencedAssembly.Location);
            }

            this.OnInitCompiler();
        }

        protected abstract void OnInitCompiler();

        /// <summary>
        /// Add an assembly to the list of ReferencedAssemblies
        /// required to build the class
        /// </summary>
        /// <param name="name"></param>
        protected void AddAssembly(string name)
        {
            if (name.StartsWith("System."))
            {
                return;
            }

            if (!cp.ReferencedAssemblies.Contains(name))
            {
                cp.ReferencedAssemblies.Add(name);
            }
        }

        protected CompilerResults Compile(string code)
        {
            CodeDomProvider provider = new CSharpCodeProvider();
            return provider.CompileAssemblyFromSource(cp, new[] { code });
        }
    }
}
