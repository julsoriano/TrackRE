#region USINGs
//using CITT.SemanticLogging;
//using CITT.Sinks.Email;
//using DevGuideExample.MenuSystem;
//using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;

using System;
using System.Data.SqlClient;
using System.Diagnostics.Tracing;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

//using TrackRE.Instrumentation;

using TrackRE.Models;
#endregion

#region LOG
/*
 * 2014
 * 28Apr: New
 * 29Apr: Encrypted web.config
 *        Installed owin.security.providers
 * 13May: Incorporating SignalR and KO
 * 10Jul: Installed ELMAH
 * 01Sep: Installed Bootstrap 3.2.0
 */
#endregion

#region PENDING
/* 
 * 
 * SECURITY
 * - Implement new Web API 2 security features                          Status: Pending
 * 
 * 
 */ 
#endregion

namespace TrackRE
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //private EventListener _azureListener;

        protected void Application_Start()
        {
            /* Run DB Initialization */
            //Action seedDB = delegate() { SeedDB();} ;
            //seedDB();

            //_azureListener = WindowsAzureTableLog.CreateListener("TrackRE", "UseDevelopmentStorage=true", "SLAB", TimeSpan.FromSeconds(1));

            // Profiling Azure Storage with Fiddler http://sepialabs.com/blog/2012/02/17/profiling-azure-storage-with-fiddler/
            //_azureListener = WindowsAzureTableLog.CreateListener("TrackRE", "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://ipv4.fiddler", "SLAB", TimeSpan.FromSeconds(1));
            
            /* Alternative approach to above */
            // _azureListener = new ObservableEventListener();
            // _azureListener.LogToWindowsAzureTable("TrackRE", "UseDevelopmentStorage=true", "SLAB", TimeSpan.FromSeconds(1));     // PM> Install-Package EnterpriseLibrary.SemanticLogging.WindowsAzure 
            /*
            _azureListener.EnableEvents(TrackREEvents.Log, EventLevel.LogAlways, Keywords.All);
            TrackREEvents.Log.Startup();
            TrackREEvents.Log.Failure("Couldn't connect to server.");

            NotifyViaEmail(); */

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            /* Add global authorization filter to Web ApiController, e.g., PropertiesApiController
             * http://blogs.msdn.com/b/rickandy/archive/2012/03/23/securing-your-asp-net-mvc-4-app-and-the-new-allowanonymous-attribute.aspx
             */
            //GlobalConfiguration.Configuration.Filters.Add(new System.Web.Http.AuthorizeAttribute());   // DNU: Prevents edits, deletes, adds
        }

        private void Application_Stop()
        {
            /*
            if (_azureListener != null)
            {
                // Disable the event listener - typically done when the application terminates
                _azureListener.DisableEvents(TrackREEvents.Log);
                _azureListener.Dispose();
            } */
        }

        /*
        void ErrorMail_Mailing(object sender, Elmah.ErrorMailEventArgs e)
        {
            
            //if (e.Error.Exception is ApplicationException ||
            //    (e.Error.Exception is HttpUnhandledException &&
            //        e.Error.Exception.InnerException != null &&
            //        e.Error.Exception.InnerException is ApplicationException))
            //{
            e.Mail.Priority = System.Net.Mail.MailPriority.High;
            e.Mail.Subject = "This is a high priority item!";
            //e.Mail.CC.Add("elijahpne@gmail.com");
            //}
        }*/

        #region EMAIL NOTIFICATION
        /*
        private void NotifyViaEmail()
        {
            new MenuDrivenApplication("Semantic Logging Block Developer's Guide Examples",
                EmailEventSource).Run();
        }

        static void EmailEventSource()
        {
            ObservableEventListener listener = new ObservableEventListener();
            listener.EnableEvents(TrackREEvents.Log,
              EventLevel.LogAlways, Keywords.All);

            listener.LogToEmail(CredentialManager.smtp, CredentialManager.port, "jul_soriano@yahoo.com", "TrackRE: App Initialization", "etw");

            // Log some messages
            //TrackREEvents.Log.Startup();
            TrackREEvents.Log.Failure("Unable to initialize app");

            // Disable the event listener - typically done when the application terminates
            listener.DisableEvents(TrackREEvents.Log);
            listener.Dispose();
        }
        */
        #endregion

        #region CREATE/SEED DATABASE
        bool SeedDB()
        {
            System.Data.Entity.Database.SetInitializer(new TrackRE.Models.SampleData());

            // Runs the registered IDatabaseInitializer<TContext> on this context. 
            // If the parameter force is set to true, then the initializer is run regardless of whether or not it has been run before. 
            // This can be useful if a database is deleted while an app is running and needs to be reinitialized.            
            //

            using (var context = new ApplicationDbContext())
            {
                try
                {
                    if (context.Database.Exists())
                        context.Database.Delete();

                    context.Database.Create();
                    context.Database.Initialize(force: true);
                    return true;

                }
                catch (SqlException ex)
                {
                    DisplaySqlErrors(ex);
                    return false;
                }
                catch (Exception e)
                {
                    //CITTLogging.CITTLogToEmail("TrackRE: DB Initialization", e.ToString());
                    Console.WriteLine("{0} Second exception caught.", e);
                    return false;
                }
            }
        }

        private static void DisplaySqlErrors(SqlException exception)
        {
            for (int i = 0; i < exception.Errors.Count; i++)
            {
                //CITTLogging.CITTLogToEmail("TrackRE: DB Initialization", "Index #" + i + "\n" +
                //    "Error: " + exception.Errors[i].ToString() + "\n");
                Console.WriteLine("Index #" + i + "\n" +
                    "Error: " + exception.Errors[i].ToString() + "\n");
            }
            //Console.ReadLine();
        }
        #endregion
    }

}

