using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace CITT.Sinks.Email
{
    public class CredentialManager
    {
        public static string smtp;
        public static int port;

        // A static constructor is used to initialize any static data, or to perform a particular action that needs to be performed once only. 
        // It is called automatically before the first instance is created or any static members are referenced.
        static CredentialManager()
        {
            // Set the root path of the Web application that contains the Web.config file that you want to access.
            string configPath = "/TrackRE";

            // Get the configuration object to access the related Web.config file.
            Configuration config = WebConfigurationManager.OpenWebConfiguration(configPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            smtp = settings.Smtp.Network.Host;
            port = settings.Smtp.Network.Port;
        }
    }
}