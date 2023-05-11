using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInvoker
{
    internal class CSharpInvoker : NetCodeInvoker
    {
        public override NetLanguage Language => NetLanguage.CSharp;

        public override string GetCodeFooter()
        {
            return "\r\n}}";
        }

        public override string GetCodeHeader(Dictionary<string, Argument> args, IEnumerable<string> imports)
        {
            List<Tuple<string, Type, ArgumentDirection>> inArgs = args.Select((KeyValuePair<string, Argument> a) => new Tuple<string, Type, ArgumentDirection>(a.Key, a.Value.ArgumentType, a.Value.Direction)).ToList();
            string imports2 = GetImports(imports);
            string arguments = GetArguments(inArgs);
            return imports2 + "namespace UiPath.Code{ public class UiPathCodeRunner{\r\npublic static void Run(" + arguments + "){\r\n";
        }

        protected override string GetArguments(List<Tuple<string, Type, ArgumentDirection>> inArgs)
        {
            Func<ArgumentDirection, string> getDirection = (ArgumentDirection d) => (d != 0) ? "ref " : string.Empty;
            return string.Concat(inArgs.Select((Tuple<string, Type, ArgumentDirection> a) => getDirection(a.Item3) + GetTypeName(a.Item2) + " " + a.Item1 + ", ")).TrimEnd(' ', ',');
        }

        protected override CompilerRunner GetCompilerRunner(string code, string className, string methodName, int errLineOffset, bool inMemory)
        {
            return new CSharpCompilerRunner(code, className, methodName, errLineOffset);
        }

        protected override string GetFunctionCode(string userCode, List<Tuple<string, Type, ArgumentDirection>> inArgs)
        {
            string arguments = GetArguments(inArgs);
            return "public static void Run(" + arguments + "){\r\n" + userCode + "\r\n}";
        }

        protected override string GetImport(string imp)
        {
            return "using " + imp + ";";
        }

        protected override Tuple<string, string, int> GetModuleCode(string funcCode, string imps)
        {
            int num = imps.Count((char c) => c == '\n');
            string text = GenerateRandomSufix();
            string text2 = "UiPath.CodeRunner";
            string text3 = "UiPathCodeRunner_" + text;
            return new Tuple<string, string, int>("\r\n" + imps + "namespace " + text2 + " { public class " + text3 + "{\r\n" + funcCode + "\r\n}}", text2 + "." + text3, 4 + num);
        }

        protected override string GetTypeName(Type t)
        {
            if (!t.IsGenericType)
            {
                return "global::" + t.FullName;
            }
            if (t.IsNested && t.DeclaringType.IsGenericType)
            {
                throw new NotImplementedException();
            }
            StringBuilder stringBuilder = new StringBuilder("global::");
            stringBuilder.Append(t.FullName.Substring(0, t.FullName.IndexOf('`')));
            stringBuilder.Append("<");
            Type[] genericArguments = t.GetGenericArguments();
            foreach (Type t2 in genericArguments)
            {
                stringBuilder.Append(GetTypeName(t2));
                stringBuilder.Append(", ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.Append(">").ToString();
        }
    }
}
