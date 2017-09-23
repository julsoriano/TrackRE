using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

// Note: To send message asynchronously see http://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=EN-US&k=k(System.Net.Mail.SmtpClient);k(SmtpClient);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5);k(DevLang-csharp)&rd=true
// To send attachment, see http://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=EN-US&k=k(System.Net.Mail.MailMessage);k(MailMessage);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5);k(DevLang-csharp)&rd=true
// See UsingMailMessage project

namespace CITT.Sinks.Email
{
    public sealed class EmailSink : IObserver<EventEntry>
    {
        private const string DefaultSubject = "Email Sink Extension";
        private IEventTextFormatter formatter;
        private MailAddress sender;
        private MailAddressCollection recipients = new MailAddressCollection();
        private string subject;
        private string host;
        private int port;

        public EmailSink(string host, int port,
          string recipients, string subject, string credentials,
          IEventTextFormatter formatter)
        {
            this.formatter = formatter ?? new EventTextFormatter();
            this.host = host;
            this.port = GuardPort(port);

            //Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/TrackRE");
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            this.sender = new MailAddress(settings.Smtp.Network.UserName);  // see http://dotnetslackers.com/Community/blogs/kaushalparik/archive/2008/09/06/accessing-web-config-file-smtp-mail-settings-programmatically.aspx

            this.recipients.Add(GuardRecipients(recipients));
            this.subject = subject ?? DefaultSubject;
        }

        public void OnNext(EventEntry entry)
        {
            if (entry != null)
            {
                using (var writer = new StringWriter())
                {
                    this.formatter.WriteEvent(entry, writer);
                    Post(writer.ToString());
                }
            }
        }

        private async void Post(string body)
        {
            /* Comment out to use credentials in code - not a good idea
            // using (var client = new SmtpClient(this.host, this.port) { Credentials = this.credentials, EnableSsl = true })
            */

            /* Using the credentials in web.config which can be encrypted */
            using (var client = new SmtpClient(this.host, this.port) )

            using (var message = new MailMessage(this.sender, this.recipients[0]) { Body = body, Subject = this.subject })
            {
                for (int i = 1; i < this.recipients.Count; i++)
                    message.CC.Add(this.recipients[i]);

                try
                {
                    await client.SendMailAsync(message).ConfigureAwait(false);
                }
                catch (SmtpException e)
                {
                    SemanticLoggingEventSource.Log.CustomSinkUnhandledFault(
                      "SMTP error sending email: " + e.Message);
                }
                catch (InvalidOperationException e)
                {
                    SemanticLoggingEventSource.Log.CustomSinkUnhandledFault(
                      "Configuration error sending email: " + e.Message);
                }
            }
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        private static int GuardPort(int port)
        {
            if (port < 0)
                throw new ArgumentOutOfRangeException("port");

            return port;
        }

        private static string GuardRecipients(string recipients)
        {
            if (recipients == null)
                throw new ArgumentNullException("recipients");

            if (string.IsNullOrWhiteSpace(recipients))
                throw new ArgumentException(
                  "The recipients cannot be empty", "recipients");

            return recipients;
        }
    }
}