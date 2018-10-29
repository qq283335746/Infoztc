using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Messaging;
using TygaSoft.SysHelper;

namespace TygaSoft.ThreadProcessor
{
    public class RandomProcessor
    {
        static string randomGeneratorQueue = ConfigurationManager.AppSettings["RandomGeneratorQueue"];
        static string randomOrderQueue = ConfigurationManager.AppSettings["RandomOrderQueue"];

        static int threadCount = 1;

        static RandomGenerator rg = new RandomGenerator(randomGeneratorQueue);

        public static void Processor()
        {
            if (!MessageQueue.Exists(randomGeneratorQueue))
                MessageQueue.Create(randomGeneratorQueue, true);

            Thread thread;
            Thread[] producerThreads = new Thread[threadCount];
            Thread[] consumerThreads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                thread = new Thread(new ThreadStart(Producer));
                thread.IsBackground = true;
                thread.Start();
                producerThreads[i] = thread;

                thread = new Thread(new ThreadStart(Consumer));
                thread.IsBackground = true;
                thread.Start();
                consumerThreads[i] = thread;
            }
        }

        private static void Producer()
        {
            string randomGeneratorFilePath = ConfigurationManager.AppSettings["RandomGeneratorFilePath"];
            int randomCodeLen = int.Parse(ConfigurationManager.AppSettings["RandomCodeLen"]);
            int randomQueueLen = int.Parse(ConfigurationManager.AppSettings["RandomQueueLen"]);
            //RandomGenerator rg = new RandomGenerator(randomGeneratorQueue);
            rg.RandomGeneratorFilePath = randomGeneratorFilePath;
            rg.RandomCodeLen = randomCodeLen;
            rg.RandomQueueLen = randomQueueLen;
            rg.Prefix = "1";
            
            while (true)
            {
                rg.Producer();
            }
        }

        private static void Consumer()
        {
            while (true)
            {
                try
                {
                    rg.QueueDequeue();
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }

            //Thread.Sleep(10000);
            //int n = 0;
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //while (true)
            //{
            //    n++;

            //    try
            //    {
            //        string orderNum = rg.Consumer();
            //        if (!string.IsNullOrEmpty(orderNum))
            //        {
            //            if (dic.ContainsKey(orderNum))
            //            {
            //                Console.WriteLine("找到重复：{0}，循环第{1}次", orderNum, n);
            //                break;
            //            }
            //            else
            //            {
            //                dic.Add(orderNum, string.Empty);
            //            }
            //        }
            //        if (dic.Count > 0)
            //        {
            //            foreach (var item in dic)
            //            {
            //                Console.WriteLine("订单号：{0}", item.Key);

            //                rg.QueueDequeue();
            //            }

            //            dic.Clear();

            //            Console.WriteLine("准备下一轮消费--------------------------------------- \r\n");
            //        }
            //    }
            //    catch
            //    {
            //        Thread.Sleep(3000);
            //    }

            //}
        }
    }
}
