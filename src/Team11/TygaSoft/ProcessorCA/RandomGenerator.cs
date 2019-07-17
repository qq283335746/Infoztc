using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.ProcessorCA
{
    public class RandomGenerator
    {
        private static Queue<string> queue = new Queue<string>();
        private static Dictionary<string, string> dic;
        private static Random rnd;
        //private static RandomGenerator rg;

        public RandomGenerator()
        {
            if (rnd == null) rnd = new Random();
            if (dic == null) dic = new Dictionary<string, string>();
        }

        //public static RandomGenerator GetRandom()
        //{
        //    if (rg == null)
        //    {
        //        rg = new RandomGenerator();
        //        if (rnd == null) rnd = new Random();
        //    }
        //    return rg;
        //}

        public string GetRandomNumber(int n)
        {
            string key = "";
            while (true)
            {
                key = (rnd.NextDouble() * int.MaxValue).ToString();
                if (key.Length < n)
                {
                    int rn = n - key.Length;
                    for (int i = 0; i < rn; i++)
                    {
                        key += rnd.Next(0, 10);
                    }
                }
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, "");
                    if (dic.Count() > 1000000)
                    {
                        dic.Clear();
                    }
                    break;
                }
            }

            return key;

            //DateTime a = DateTime.Parse("2015-12-14 10:15");
            //string s = a.ToString("yyMMddHHmmssffff");
            //int ln = int.Parse(DateTime.Now.ToString("HHmmss"));

            //int ln = int.MaxValue;

            //string s = "";
            //ln = (int)(rnd.NextDouble() * ln);
            //s = ln.ToString();
            //if (s.Length < n)
            //{
            //    int rn = n - s.Length;
            //    string sr = "";
            //    for(int i = 0;i<rn;i++)
            //    {
            //        sr += "9";
            //    }
            //    s += ((int)(rnd.NextDouble() * (int.Parse(sr)))).ToString();
            //}
            //if (s.Length < n) s.PadRight(n, 'p');
            //return s;
        }
    }
}
