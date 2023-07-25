using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeInvoker
{
    internal abstract class NetCodeInvoker
    {
        private static Dictionary<Assembly, IEnumerable<string>> _assemblyNamespaces = new Dictionary<Assembly, IEnumerable<string>>();

        private static Dictionary<string, CompilerRunner> codeRunnerCache = new Dictionary<string, CompilerRunner>(25);

        private static object codeRunnerCacheLock = new object();

        public abstract NetLanguage Language { get; }

        public static NetCodeInvoker Create(NetLanguage lang)
        {
            switch(lang)
            {
                case NetLanguage.VBNet:
                    return new VBCodeInvoker();
                case NetLanguage.CSharp:
                    return new CSharpInvoker();
                default:
                    throw new NotSupportedException(lang.ToString());
            }
        }

        protected string GetImports(IEnumerable<string> imports)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string import in imports)
            {
                if (!string.IsNullOrWhiteSpace(import))
                {
                    stringBuilder.AppendLine(GetImport(import));
                }
            }
            return stringBuilder.ToString();
        }

        protected abstract string GetImport(string imp);

        public void Run(string userCode, List<Tuple<string, Type, ArgumentDirection>> inArgs, IEnumerable<string> imps, object[] args)
        {
            imps = FilterImports(imps);
            GetCompilerRunner(userCode, inArgs, GetImports(imps), inMemory: true).Run(args);
        }

        private IEnumerable<string> FilterImports(IEnumerable<string> imps)
        {
            Assembly[] currentAssemblies = CompilerRunner.GetCurrentAssemblies();
            HashSet<string> namespaces = new HashSet<string>(currentAssemblies.SelectMany(a => GetNamespaces(a).Distinct()));
            return imps.Where((string a) => namespaces.Contains(a));
        }

        public static IEnumerable<string> GetNamespaces(Assembly asm)
        {
            lock (_assemblyNamespaces)
            {
                if (_assemblyNamespaces.TryGetValue(asm, out var value))
                {
                    return value;
                }
                try
                {
                    value = (from t in asm.GetExportedTypes()
                             select t.Namespace).Distinct();
                }
                catch
                {
                    value = Enumerable.Empty<string>();
                }
                _assemblyNamespaces[asm] = value;
                return value;
            }
        }

        public void Compile(string userCode, IEnumerable<string> imps, List<Tuple<string, Type, ArgumentDirection>> inArgs)
        {
            imps = FilterImports(imps);
            GetCompilerRunner(userCode, inArgs, GetImports(imps), inMemory: false);
        }

        internal virtual CompilerRunner GetCompilerRunner(string userCode, List<Tuple<string, Type, ArgumentDirection>> args, string imps, bool inMemory)
        {
            CompilerRunner value = null;
            string functionCode = GetFunctionCode(userCode, args);
            lock (codeRunnerCacheLock)
            {
                if (!codeRunnerCache.TryGetValue(functionCode, out value))
                {
                    Tuple<string, string, int> moduleCode = GetModuleCode(functionCode, imps);
                    value = GetCompilerRunner(moduleCode.Item1, moduleCode.Item2, "Run", moduleCode.Item3, inMemory);
                    codeRunnerCache.Add(functionCode, value);
                    return value;
                }
                return value;
            }
        }

        protected abstract CompilerRunner GetCompilerRunner(string code, string className, string methodName, int errLineOffset, bool inMemory);

        protected abstract Tuple<string, string, int> GetModuleCode(string funcCode, string imps);

        protected abstract string GetFunctionCode(string userCode, List<Tuple<string, Type, ArgumentDirection>> inArgs);

        protected string GenerateRandomSufix()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        protected abstract string GetArguments(List<Tuple<string, Type, ArgumentDirection>> inArgs);

        public abstract string GetCodeHeader(Dictionary<string, Argument> args, IEnumerable<string> imports);

        protected abstract string GetTypeName(Type t);

        public abstract string GetCodeFooter();
    }
}
