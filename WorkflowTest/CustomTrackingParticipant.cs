﻿using System;
using System.Activities.Tracking;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WorkflowTest
{
    /// <summary>
    /// Workflow Tracking Participant - Custom Implementation
    /// </summary>
    internal class CustomTrackingParticipant : TrackingParticipant
    {
        public string TrackData = string.Empty;
        public event EventHandler<TrackingEventArgs> TrackingRecordReceived;

        public Dictionary<string, Activity> ActivityIdToWorkflowElementMap { get; set; }

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            OnTrackingRecordReceived(record, timeout);
        }

        //On Tracing Record Received call the TrackingRecordReceived with the record received information from the TrackingParticipant. 
        //We also do not worry about Expressions' tracking data
        protected void OnTrackingRecordReceived(TrackingRecord record, TimeSpan timeout)
        {
            if (record is ActivityStateRecord recordEntry)
            {
                string mess = string.Format("[{0}] [{1}] [{2}]" + Environment.NewLine,
                    recordEntry.EventTime.ToLocalTime().ToString(),
                    recordEntry.Activity.Name,
                    recordEntry.State);
                TrackData += mess;
                Debug.WriteLine(mess);
            }

            if (TrackingRecordReceived != null)
            {
#pragma warning disable IDE0019 // Use pattern matching
                ActivityStateRecord activityStateRecord = record as ActivityStateRecord;
#pragma warning restore IDE0019 // Use pattern matching

                if (activityStateRecord != null && !activityStateRecord.Activity.TypeName.Contains("System.Activities.Expressions"))
                {
                    if (ActivityIdToWorkflowElementMap.ContainsKey(activityStateRecord.Activity.Id))
                    {
                        TrackingRecordReceived(this,
                            new TrackingEventArgs(record, timeout,
                            ActivityIdToWorkflowElementMap[activityStateRecord.Activity.Id]));
                    }
                }
                else
                {
                    TrackingRecordReceived(this, new TrackingEventArgs(record, timeout, null));
                }
            }
        }
    }

    //Custom Tracking EventArgs
    public class TrackingEventArgs : EventArgs
    {
        public TrackingRecord Record { get; set; }

        public TimeSpan Timeout { get; set; }

        public Activity Activity { get; set; }

        public TrackingEventArgs(TrackingRecord trackingRecord, TimeSpan timeout, Activity activity)
        {
            Record = trackingRecord;
            Timeout = timeout;
            Activity = activity;
        }
    }
}
