using Microsoft.CSharp;
using System;
using System.Activities;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGuessWorkflowActivities
{
    public class CSharpExecute : CodeActivity
    {
        public InArgument<string> CodeString { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            //// Создание нового объекта CSharpCodeProvider
            //var codeProvider = new CSharpCodeProvider();

            //// Создание нового объекта CompilerParameters
            //var compilerParams = new CompilerParameters
            //{
            //    GenerateInMemory = true
            //};

            //// Добавление ссылок на необходимые сборки
            //compilerParams.ReferencedAssemblies.Add("System.dll");

            //// Компиляция исходного кода C#
            //var compilerResults = codeProvider.CompileAssemblyFromSource(compilerParams, CodeString.Get(context));

            //// Получение скомпилированного объекта
            //var compiledAssembly = compilerResults.CompiledAssembly;

            //// Создание нового объекта вашего класса
            //var myCode = compiledAssembly..CreateInstance("MyNamespace.MyCodeClass");

            //// Вызов метода вашего класса
            //myCode.GetType().GetMethod("MyMethod").Invoke(myCode, null);
        }
    }
}
