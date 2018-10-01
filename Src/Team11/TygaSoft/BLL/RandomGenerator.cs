using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using TygaSoft.IMessaging;
using TygaSoft.MessagingFactory;

namespace TygaSoft.BLL
{
    public class RandomGenerator
    {
        private static readonly IRandomGenerator emailQueue = QueueAccess.CreateRandomGenerator();

        public void Insert(string rndCode)
        {
            emailQueue.Send(rndCode);
        }

        public string ReceiveFromQueue(int timeout)
        {
            return emailQueue.Receive(timeout);
        }

        public void WriteToReceiveFile(string randomGeneratorFilePath, string rndCode)
        {
            string dir = string.Format("{0}\\{1}", randomGeneratorFilePath, "Receive");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string filePath = string.Format("{0}\\{1}", dir, "" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            var task = Task.Factory.StartNew(() => WriteToFile(filePath, rndCode));
            task.Wait();
        }

        private void WriteToFile(string fileName, string rndCode)
        {
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(rndCode);
                sw.Close();
            }
        }
    }
}
