using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInvoker
{
    internal class CSharpCompilerRunner : CompilerRunner
    {
        public CSharpCompilerRunner(string code, string className, string methodName, int errLineOffset = 0, bool generateInMemory = true)
            : base(code, className, methodName, errLineOffset, generateInMemory)
        {
        }

        protected override CodeDomProvider GetCodeProvider()
        {
            return new CSharpCodeProvider();
        }
    }
}
