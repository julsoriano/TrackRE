using CITT.Sinks.Email;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;       // EventListener
using System.Linq;
using System.Web;
using TrackRE.Instrumentation;

namespace CITT.SemanticLogging
{
    public static class CITTLogging
    {
        public static void CITTLogToEmail(string errsubject, string errmessage)
        {
            ObservableEventListener listener = new ObservableEventListener();
            listener.EnableEvents(TrackREEvents.Log,
                EventLevel.LogAlways, Keywords.All);

            //listener.LogToEmail("smtp.live.com", 587, "jul_soriano@yahoo.com", "In Proc Sample", "etw");
            listener.LogToEmail(CredentialManager.smtp, CredentialManager.port, "jul_soriano@yahoo.com", errsubject, "etw");

            // Log some messages
            TrackREEvents.Log.Failure(errmessage);

            // Disable the event listener - typically done when the application terminates
            listener.DisableEvents(TrackREEvents.Log);
            listener.Dispose();
        }
    }
}