using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Messaging;
using System.Transactions;

namespace TygaSoft.ThreadProcessor
{
    sealed class RandomGenerator
    {
        //MessageQueue mq;
        private static Queue<string> queue;
        private static Random rnd;

        //public string QueueName { get; set; }
        public string RandomGeneratorFilePath { get; set; }
        public int RandomCodeLen { get; set; }
        public int RandomQueueLen { get; set; }
        public bool IsFull { get; set; }
        public string Prefix { get; set; }

        public RandomGenerator(string queueName)
        {
            if (rnd == null) rnd = new Random();
            if (queue == null) queue = new Queue<string>();
        }

        public void Producer()
        {
            string dir = string.Format("{0}\\{1}", RandomGeneratorFilePath,"Send");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            while (true)
            {
                if (queue.Count() > RandomQueueLen)
                {
                    IsFull = true;
                    break;
                }
                string rndCode = Prefix + (rnd.NextDouble() * int.MaxValue).ToString();
                int rndCodeLen = rndCode.Length;
                if (rndCodeLen > RandomCodeLen)
                {
                    rndCode = rndCode.Substring(0, RandomCodeLen);
                }
                if (rndCodeLen < RandomCodeLen)
                {
                    rndCode = rndCode.PadRight(RandomCodeLen, char.Parse(rnd.Next(10).ToString()));
                }

                bool isExistCode = false;
                string filePath = string.Format("{0}\\{1}", dir, "" + DateTime.Now.ToString("yyyyMM") + ".txt");
                string[] files = Directory.GetFiles(RandomGeneratorFilePath);
                if (files != null && files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        IEnumerable<string> codes = File.ReadLines(file);
                        if (codes.Any(s => s == rndCode))
                        {
                            isExistCode = true;
                            break;
                        }
                    }
                }

                if (!isExistCode)
                {
                    var task = Task.Factory.StartNew(() => WriteToFile(filePath, rndCode));
                    task.Wait();
                    queue.Enqueue(rndCode);
                    BLL.RandomGenerator rgBll = new BLL.RandomGenerator();
                    rgBll.Insert(rndCode);

                    break;
                }
            }
        }

        public string Consumer()
        {
            BLL.RandomGenerator rgBll = new BLL.RandomGenerator();
            string rndCode = rgBll.ReceiveFromQueue(20);

            string dir = string.Format("{0}\\{1}", RandomGeneratorFilePath, "Receive");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string filePath = string.Format("{0}\\{1}", dir, "" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
            var task = Task.Factory.StartNew(() => WriteToFile(filePath, rndCode));
            task.Wait();

            return rndCode;
        }

        public void GetRandomNumber()
        {
            if (queue.Count > 0)
            {
                string filePath = string.Format("{0}\\{1}\\{2}", RandomGeneratorFilePath, "Send", "" + DateTime.Now.ToString("yyyyMM") + ".txt");
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string item = sr.ReadLine().Trim();
                            if (queue.Contains(item))
                            {
                                queue.Dequeue();
                            }
                            else
                            {
                                item.Remove(0);
                            }
                        }
                        sr.Close();
                    }
                }
            }
        }

        private void WriteToFile(string fileName, string rndCode)
        {
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(rndCode);
                sw.Close();
            }
        }

        public void QueueDequeue()
        {
            DateTime currTime = DateTime.Now;
            string[] files = Directory.GetFiles(string.Format("{0}\\{1}", RandomGeneratorFilePath, "Receive"));
            foreach (string file in files)
            {
                string dateString = Path.GetFileNameWithoutExtension(file);
                if (dateString != currTime.ToString("yyyyMMdd") && dateString != currTime.AddDays(-1).ToString("yyyyMMdd"))
                {
                    File.Delete(file);
                    break;
                }

                if (File.Exists(file))
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        while (sr.Peek() >= 0)
                        {
                            string item = sr.ReadLine().Trim();
                            if (queue.Contains(item))
                            {
                                if (queue.Count > 0)
                                {
                                    queue.Dequeue();
                                }
                            }
                        }
                        if (sr.Peek() < 0)
                        {
                            File.WriteAllText(file, "");
                        }
                        sr.Close();
                    }
                }
            }
        }
    }
}
