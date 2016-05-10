using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace realtime_idea_space
{
    public static class Config
    {
        public static string BuyNumberCountryCode {
            get
            {
                return WebConfigurationManager.AppSettings["Nexmo.BuyNumberCountry"] ?? "GB";
            }
        }

        public static string NexmoFromNumber
        {
            get
            {
                return WebConfigurationManager.AppSettings["Nexmo.FromNumber"];
            }
        }
    }
}