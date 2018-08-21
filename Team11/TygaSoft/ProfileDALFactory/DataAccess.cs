using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using TygaSoft.IProfile;

namespace TygaSoft.ProfileDALFactory
{
    public sealed class DataAccess
    {
        private static readonly string profilePath = ConfigurationManager.AppSettings["ProfileDAL"];

        public static ITygaSoftProfileProvider CreateProfileProvider()
        {
            string className = profilePath + ".TygaSoftProfileProvider";
            return (ITygaSoftProfileProvider)Assembly.Load(profilePath).CreateInstance(className);
        }
    }
}
