using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using TygaSoft.IMessaging;

namespace TygaSoft.MessagingFactory
{
    public sealed class QueueAccess
    {
        // Look up the Messaging implementation we should be using
        private static readonly string[] path = ConfigurationManager.AppSettings["MsmqMessaging"].Split(new char[] { ',' });

        private QueueAccess() { }

        public static IRandomGenerator CreateRandomGenerator()
        {
            string className = path[0] + ".RandomGenerator";
            return (IRandomGenerator)Assembly.Load(path[1]).CreateInstance(className);
        }

    }
}
