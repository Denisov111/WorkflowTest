using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities.Expressions;

namespace CodeInvoker
{
    public class InvokeCode : CodeActivity
    {
        private string _compilationError;

        private NetCodeInvoker _codeInvoker;

        internal virtual NetCodeInvoker CodeInvoker
        {
            get
            {
                if (_codeInvoker?.Language != Language)
                {
                    _codeInvoker = NetCodeInvoker.Create(Language);
                }
                return _codeInvoker;
            }
        }

        [RequiredArgument]
        public string Code { get; set; }

        public InArgument<bool> ContinueOnError { get; set; }

        [Browsable(true)]
        public Dictionary<string, Argument> Arguments { get; set; }

        [DefaultValue(NetLanguage.VBNet)]
        public NetLanguage Language { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (!string.IsNullOrWhiteSpace(_compilationError))
            {
                metadata.AddValidationError(_compilationError);
            }
        }

        public InvokeCode()
        {
            Language = NetLanguage.VBNet;
            Arguments = new Dictionary<string, Argument>();
        }

        private IList<string> GetImports(Activity workflow)
        {
            return TextExpression.GetNamespacesForImplementation(workflow) ?? Array.Empty<string>();
        }

        protected override void Execute(CodeActivityContext context)
        {
            IList<string> imports = GetImports(context.GetRootActivity());
            string code = Code;
            bool flag = ContinueOnError.Get(context);
            List<Tuple<string, Type, ArgumentDirection>> list = new List<Tuple<string, Type, ArgumentDirection>>(Arguments.Count);
            object[] array = new object[Arguments.Count];
            int num = 0;
            foreach (KeyValuePair<string, Argument> argument2 in Arguments)
            {
                list.Add(new Tuple<string, Type, ArgumentDirection>(argument2.Key, argument2.Value.ArgumentType, argument2.Value.Direction));
                array[num++] = argument2.Value.Get(context);
            }
            try
            {
                CodeInvoker.Run(code, list, imports, array);
                int num2 = 0;
                foreach (Tuple<string, Type, ArgumentDirection> item in list)
                {
                    Argument argument = Arguments[item.Item1];
                    if (argument.Direction != 0)
                    {
                        argument.Set(context, array[num2]);
                    }
                    num2++;
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex2)
            {
                Trace.TraceError(ex2.ToString());
                if (!flag)
                {
                    throw;
                }
            }
        }

        public void SetSuccessfulCompilation()
        {
            _compilationError = "";
        }

        public void SetCompilationError(string errorMessage)
        {
            _compilationError = errorMessage;
        }
    }
}
