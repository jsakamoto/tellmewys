using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TellMeWYS
{
    public static class AppSettings
    {
        public static bool Debug { get { return bool.Parse(ConfigurationManager.AppSettings["Debug"]); } }

        public static string Salt { get { return ConfigurationManager.AppSettings["Salt"]; } }

        public static string MicrosoftAccount_ClientId { get { return ConfigurationManager.AppSettings["MicrosoftAccount.ClientId"]; } }

        public static string MicrosoftAccount_ClientSecret { get { return ConfigurationManager.AppSettings["MicrosoftAccount.ClientSecret"]; } }

        //public static int InvitationExpirationDays { get { return ConfigurationManager.AppSettings["InvitationExpirationDays"].ToInt(); } }

        //public static int PINCodeExpirationMinutes { get { return ConfigurationManager.AppSettings["PINCodeExpirationMinutes"].ToInt(); } }

        //public static string MailFrom { get { return ConfigurationManager.AppSettings["MailFrom"]; } }

        //public static string SmtpHost { get { return ConfigurationManager.AppSettings["SmtpHost"]; } }

        //public static int SmtpPort { get { return ConfigurationManager.AppSettings["SmtpPort"].ToInt(); } }

        //public static string SmtpUser { get { return ConfigurationManager.AppSettings["SmtpUser"]; } }

        //public static string SmtpPassword { get { return ConfigurationManager.AppSettings["SmtpPassword"]; } }

        //public static string SmsFrom { get { return ConfigurationManager.AppSettings["SmsFrom"]; } }

        //public static string TwilioAccountSid { get { return ConfigurationManager.AppSettings["TwilioAccountSid"]; } }

        //public static string TwilioAuthToken { get { return ConfigurationManager.AppSettings["TwilioAuthToken"]; } }

        
    }
}