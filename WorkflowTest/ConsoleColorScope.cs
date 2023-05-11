using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkflowTest
{
    public sealed class ConsoleColorScope : NativeActivity
    {
        public ConsoleColorScope()
            : base()
        {
        }

        public ConsoleColor Color { get; set; }
        public Activity Body { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            context.Properties.Add(ConsoleColorProperty.Name, new ConsoleColorProperty(this.Color));

            if (this.Body != null)
            {
                context.ScheduleActivity(this.Body);
            }
        }
    }
}
