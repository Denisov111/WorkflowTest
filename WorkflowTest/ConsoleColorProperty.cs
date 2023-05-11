using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTest
{
    internal class ConsoleColorProperty : IExecutionProperty
    {
        public const string Name = "ConsoleColorProperty";

        ConsoleColor original;
        ConsoleColor color;

        public ConsoleColorProperty(ConsoleColor color)
        {
            this.color = color;
        }

        void IExecutionProperty.SetupWorkflowThread()
        {
            original = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }

        void IExecutionProperty.CleanupWorkflowThread()
        {
            Console.ForegroundColor = original;
        }
    }
}
