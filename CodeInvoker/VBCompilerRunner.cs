using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInvoker
{
    internal class VBCompilerRunner : CompilerRunner
    {
        public VBCompilerRunner(string code, string className, string methodName, int errLineOffset = 0, bool generateInMemory = true)
            : base(code, className, methodName, errLineOffset, generateInMemory)
        {
        }

        protected override CodeDomProvider GetCodeProvider()
        {
            return new VBCodeProvider();
        }
    }
}
