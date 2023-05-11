using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInvoker
{
    internal class VBCodeInvoker : NetCodeInvoker
    {
        public override NetLanguage Language => NetLanguage.VBNet;

        protected override CompilerRunner GetCompilerRunner(string code, string className, string methodName, int errLineOffset, bool inMemory)
        {
            return new VBCompilerRunner(code, className, methodName, errLineOffset, inMemory);
        }

        protected override string GetImport(string imp)
        {
            return "Imports " + imp;
        }

        protected override Tuple<string, string, int> GetModuleCode(string funcCode, string imps)
        {
            int num = imps.Count((char c) => c == '\n');
            string text = GenerateRandomSufix();
            return new Tuple<string, string, int>($"Option Explicit\r\nOption Strict\r\n{imps}Module UiPathCodeRunner_{text}\r\n{funcCode}\r\nEnd Module", "UiPathCodeRunner_" + text, 4 + num);
        }

        protected override string GetFunctionCode(string userCode, List<Tuple<string, Type, ArgumentDirection>> inArgs)
        {
            string arguments = GetArguments(inArgs);
            return $"Sub Run({arguments})\r\n{userCode}\r\nEnd Sub";
        }

        protected override string GetArguments(List<Tuple<string, Type, ArgumentDirection>> inArgs)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Tuple<string, Type, ArgumentDirection> inArg in inArgs)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(", ");
                }
                string text = "";
                switch (inArg.Item3)
                {
                    case ArgumentDirection.In:
                        text = "ByVal";
                        break;
                    case ArgumentDirection.Out:
                    case ArgumentDirection.InOut:
                        text = "ByRef";
                        break;
                }
                stringBuilder.Append(text + " " + inArg.Item1 + " As " + GetTypeName(inArg.Item2));
            }
            return stringBuilder.ToString();
        }

        protected override string GetTypeName(Type t)
        {
            if (!t.IsGenericType)
            {
                return t.FullName.Replace("[]", "()");
            }
            if (t.IsNested && t.DeclaringType.IsGenericType)
            {
                throw new NotImplementedException();
            }
            StringBuilder stringBuilder = new StringBuilder(t.FullName.Substring(0, t.FullName.IndexOf('`')));
            stringBuilder.Append("(Of ");
            Type[] genericArguments = t.GetGenericArguments();
            foreach (Type t2 in genericArguments)
            {
                stringBuilder.Append(GetTypeName(t2));
                stringBuilder.Append(", ");
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.Append(")").ToString();
        }

        public override string GetCodeHeader(Dictionary<string, Argument> args, IEnumerable<string> imports)
        {
            List<Tuple<string, Type, ArgumentDirection>> inArgs = args.Select((KeyValuePair<string, Argument> a) => new Tuple<string, Type, ArgumentDirection>(a.Key, a.Value.ArgumentType, a.Value.Direction)).ToList();
            string imports2 = GetImports(imports);
            string arguments = GetArguments(inArgs);
            return $"{imports2}Module UiPathCodeRunner\r\nSub Run({arguments})\r\n";
        }

        public override string GetCodeFooter()
        {
            return "\r\nEnd Sub\r\n End Module";
        }
    }
}
