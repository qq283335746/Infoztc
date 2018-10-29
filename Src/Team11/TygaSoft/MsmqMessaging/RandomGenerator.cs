using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Configuration;
using TygaSoft.IMessaging;

namespace TygaSoft.MsmqMessaging
{
    public class RandomGenerator : TygaSoftQueue, IRandomGenerator
    {
        // Path example - FormatName:DIRECT=OS:MyMachineName\Private$\OrderQueueName
        private static readonly string queuePath = ConfigurationManager.AppSettings["RandomGeneratorQueue"];
        private static int queueTimeout = 20;

        public RandomGenerator()
            : base(queuePath, queueTimeout)
        {
            queue.Formatter = new BinaryMessageFormatter();
        }

        public new string Receive()
        {
            base.transactionType = MessageQueueTransactionType.Automatic;
            return (string)((Message)base.Receive()).Body;
        }

        public string Receive(int timeout)
        {
            base.timeout = TimeSpan.FromSeconds(Convert.ToDouble(timeout));
            return Receive();
        }

        public void Send(string rndCode)
        {
            base.transactionType = MessageQueueTransactionType.Single;
            base.Send(rndCode);
        }
    }
}
