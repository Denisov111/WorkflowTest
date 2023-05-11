using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInvoker
{
    internal static class ActivityContextExtensions
    {
        public static Activity GetRootActivity(this ActivityContext context)
        {
            IWorkflowRuntime workflowRuntime = context.GetWorkflowRuntime();
            if (workflowRuntime == null)
            {
                return null;
            }
            return workflowRuntime.GetRootActivity(context.WorkflowInstanceId);
        }

        public static IWorkflowRuntime GetWorkflowRuntime(this ActivityContext context)
        {
            return context.GetExtension<IWorkflowRuntime>();
        }
    }
}
