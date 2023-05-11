using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeInvoker
{
    internal abstract class CompilerRunner
    {
        private Assembly _assembly;

        private readonly string _className;

        private readonly string _methodName;

        private readonly string[] _defaultAssemblies = new string[2] { "System.Data.DataSetExtensions.dll", "System.IO.Compression.FileSystem.dll" };

        protected CompilerRunner(string code, string className, string methodName, int errLineOffset = 0, bool generateInMemory = true)
        {
            _className = className;
            _methodName = methodName;
            Compile(code, errLineOffset, generateInMemory);
        }

        public object Run(object[] args)
        {
            if (_assembly == null)
            {
                throw new InvalidOperationException("_assembly == null");
            }
            return _assembly.GetType(_className).InvokeMember(_methodName, BindingFlags.InvokeMethod, null, _assembly, args);
        }

        protected abstract CodeDomProvider GetCodeProvider();

        private void Compile(string code, int errLineOffset, bool generateInMemory)
        {
            CodeDomProvider codeProvider = GetCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.GenerateInMemory = generateInMemory;
            compilerParameters.GenerateExecutable = false;
            compilerParameters.TreatWarningsAsErrors = false;
            IEnumerable<string> source = from a in GetCurrentAssemblies()
                                         select a.Location;
            compilerParameters.ReferencedAssemblies.AddRange(_defaultAssemblies);
            compilerParameters.ReferencedAssemblies.AddRange(source.Where((string x) => !_defaultAssemblies.Any((string y) => x.Contains(y))).ToArray());
            CompilerResults compilerResults = codeProvider.CompileAssemblyFromSource(compilerParameters, code);
            if (!compilerResults.Errors.HasErrors)
            {
                if (generateInMemory)
                {
                    _assembly = compilerResults.CompiledAssembly;
                    return;
                }
                try
                {
                    File.Delete(compilerResults.PathToAssembly);
                    return;
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning(ex.ToString());
                    return;
                }
            }
            _assembly = null;
            throw new ArgumentException("compilerResults.Errors.HasErrors==true\n" + GetErrorText(compilerResults, errLineOffset));
        }

        private static string GetErrorText(CompilerResults res, int errLineOffset)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (CompilerError error in res.Errors)
            {
                if (!error.IsWarning)
                {
                    string value = string.Format("!error.IsWarning", error.ErrorNumber, error.ErrorText, error.Line - errLineOffset);
                    stringBuilder.Append(value);
                    stringBuilder.AppendLine();
                }
            }
            return stringBuilder.ToString();
        }

        public static Assembly[] GetCurrentAssemblies()
        {
            return (from a in (from a in AppDomain.CurrentDomain.GetAssemblies()
                               select new
                               {
                                   Name = a.GetName(),
                                   Asm = a
                               }).Where(a =>
                               {
                                   CultureInfo cultureInfo = a.Name.CultureInfo;
                                   return !a.Asm.IsDynamic && (cultureInfo == null || cultureInfo.Equals(CultureInfo.CurrentCulture) || cultureInfo.Equals(CultureInfo.InvariantCulture));
                               })
                    group a by a.Name.Name into g
                    select g.OrderByDescending(a => a.Name.Version).First() into a
                    where !string.IsNullOrWhiteSpace(a.Asm.Location)
                    select a.Asm).ToArray();
        }
    }
}
