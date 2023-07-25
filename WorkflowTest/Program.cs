//using NumberGuessWorkflowActivities;
using System;
using System.Activities;
using System.Activities.Debugger;
//using System.Activities.Presentation.Debug;
using System.Activities.Statements;
using System.Activities.Tracking;
using System.Activities.XamlIntegration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WorkflowTest
{
    internal class Program
    {
        private static CustomTrackingParticipant _trackingParticipant;
        private static Dictionary<object, SourceLocation> _wfElementToSourceLocationMap;
        private static readonly Dictionary<object, SourceLocation> _designerSourceLocationMapping = new Dictionary<object, SourceLocation>();
        private static AutoResetEvent syncEvent;
        
        static void Main(string[] args)
        {
            syncEvent = new AutoResetEvent(false);

            //RegenerateSourceDebuggerMappings();
            //MemoryStream workflowStream = new MemoryStream(Encoding.Default.GetBytes(ActiveWorkflowDesigner.Text));

            //ActivityXamlServicesSettings settings = new ActivityXamlServicesSettings()
            //{
            //    CompileExpressions = true
            //};

            //DynamicActivity activityExecute = ActivityXamlServices.Load(workflowStream, settings) as DynamicActivity;

            //configure workflow application
            //Mapping between the Object and the Instance Id
            //Dictionary<string, Activity> activityIdToWfElementMap = BuildActivityIdToWfElementMap(_wfElementToSourceLocationMap);

            const string all = "*";
            _trackingParticipant = new CustomTrackingParticipant
            {
                TrackingProfile = new TrackingProfile()
                {
                    Name = "CustomTrackingProfile",
                    Queries =
                    {
                        new CustomTrackingQuery
                        {
                            Name = all,
                            ActivityName = all
                        },
                        new WorkflowInstanceQuery
                        {
                            // Limit workflow instance tracking records for started and completed workflow states
                            //States = { WorkflowInstanceStates.Started, WorkflowInstanceStates.Completed },
                            States = {all}
                        },
                        new ActivityStateQuery
                        {
                            // Subscribe for track records from all activities for all states
                            ActivityName = all,
                            States = { all },
                            Arguments = {all},

                            // Extract workflow variables and arguments as a part of the activity tracking record
                            // VariableName = "*" allows for extraction of all variables in the scope
                            // of the activity
                            Variables ={all }
                        }
                    }
                }
                //ActivityIdToWorkflowElementMap = activityIdToWfElementMap
            };

            ////As the tracking events are received
            _trackingParticipant.TrackingRecordReceived += TrackingParticipant_TrackingRecordReceived;

            WorkflowApplication _wfApp = new WorkflowApplication(new Workflow1());
            _trackingParticipant.Instance = _wfApp;
            _wfApp.Extensions.Add(_trackingParticipant);
            _wfApp.Completed = WfExecutionCompleted;

            ////execute 
            _wfApp.Run();

            syncEvent.WaitOne();

            //Activity workflow1 = new Workflow1();
            //WorkflowInvoker.Invoke(workflow1);

            //AutoResetEvent syncEvent = new AutoResetEvent(false);
            //AutoResetEvent idleEvent = new AutoResetEvent(false);

            //var inputs = new Dictionary<string, object>() { { "MaxNumber", 100 } };

            //WorkflowApplication wfApp =
            //    new WorkflowApplication(new FlowchartNumberGuessWorkflow(), inputs);

            //wfApp.Run();
        }

        private static void WfExecutionCompleted(WorkflowApplicationCompletedEventArgs obj)
        {
            syncEvent.Set();
        }

        //private static void RegenerateSourceDebuggerMappings()
        //{
        //    // Updating the mapping between Model item and Source Location before we run the workflow so that BP setting can re-use that information from the DesignerSourceLocationMapping.
        //    _designerSourceLocationMapping.Clear();
        //    _wfElementToSourceLocationMap = UpdateSourceLocationMappingInDebuggerService();
        //    //_executionLog.ActivityIdToWorkflowElementMap = BuildActivityIdToWfElementMap(_wfElementToSourceLocationMap);
        //}

        /// <summary>
        /// Updating the mapping between Model item and Source Location before we run the workflow so that BP setting can re-use that information from the DesignerSourceLocationMapping.
        /// </summary>
        /// <returns></returns>
        //private static Dictionary<object, SourceLocation> UpdateSourceLocationMappingInDebuggerService()
        //{
        //    object rootInstance = GetRootInstance();
        //    Dictionary<object, SourceLocation> sourceLocationMapping = new Dictionary<object, SourceLocation>();

        //    if (rootInstance != null)
        //    {
        //        Activity documentRootElement = GetRootWorkflowElement(rootInstance);
        //        Activity rootRuntimeWorkflowElement = GetRootRuntimeWorkflowElement();
        //        string loadedFilePath = ActiveWorkflowDesigner.Context.Items.GetValue<WorkflowFileItem>().LoadedFile;

        //        SourceLocationProvider.CollectMapping(rootRuntimeWorkflowElement, documentRootElement, sourceLocationMapping,
        //            loadedFilePath);
        //        SourceLocationProvider.CollectMapping(documentRootElement, documentRootElement, _designerSourceLocationMapping,
        //           ActiveWorkflowDesigner.Context.Items.GetValue<WorkflowFileItem>().LoadedFile);

        //    }

        //    // Notify the DebuggerService of the new sourceLocationMapping.
        //    // When rootInstance == null, it'll just reset the mapping.
        //    System.Activities.pr DebuggerService debuggerService = (DebuggerService)ActiveWorkflowDesigner.DebugManagerView;
        //    debuggerService.UpdateSourceLocations(_designerSourceLocationMapping);

        //    return sourceLocationMapping;
        //}

        private static void TrackingParticipant_TrackingRecordReceived(object trackingParticpant, TrackingEventArgs trackingEventArgs)
        {
            if (trackingEventArgs.Activity != null)
            {
                System.Diagnostics.Debug.WriteLine(
                    string.Format("<+=+=+=+> Activity Tracking Record Received for ActivityId: {0}, record: {1} ",
                    trackingEventArgs.Activity.Id,
                    trackingEventArgs.Record));

                //ContextItemManager res = ActiveWorkflowDesigner.Context.Items;
                //ModelService modelService = ActiveWorkflowDesigner.Context.Services.GetService<ModelService>();
                //Activity a = modelService.Root.GetCurrentValue() as Activity;

                //ModelItem mi = ActiveWorkflowDesigner.Context.Items.GetValue<Selection>().PrimarySelection;
                //Activity activity = mi.GetCurrentValue() as Activity;
                //string res_ = GetDefaultValueOfScopedVariable<string>(mi, "variable1");
            }
        }

        private static Dictionary<string, Activity> BuildActivityIdToWfElementMap(Dictionary<object, SourceLocation> wfElementToSourceLocationMap)
        {
            Dictionary<string, Activity> map = new Dictionary<string, Activity>();

            Activity wfElement;
            foreach (object instance in wfElementToSourceLocationMap.Keys)
            {
                wfElement = instance as Activity;
                if (wfElement != null)
                {
                    map.Add(wfElement.Id, wfElement);
                }
            }

            return map;
        }
    }
}
