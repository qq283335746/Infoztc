using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using TygaSoft.ThreadProcessor;

namespace TygaSoft.ProcessorCA
{
    class Program
    {
        static void Main(string[] args)
        {
            //SysHelper.EnumHelper eh = new SysHelper.EnumHelper();
            //var i = eh.GetValue(typeof(SysHelper.EnumData.UserLevelSource), "fw",0);
            ////Type enumType = typeof(SysHelper.EnumData.UserLevelSource);
            ////var enumSource = Enum.Parse(enumType, "广告");
            //Console.ReadLine();
            //return;

            //DateTime currTime = DateTime.Parse("2015-09-10");
            //var ts = DateTime.Parse(string.Format("{0}-01", currTime.AddMonths(1).ToString("yyyy-MM"))) - DateTime.Parse(string.Format("{0}-01", currTime.ToString("yyyy-MM")));
            //var totalDay = (int)ts.TotalDays;

            //return;

            //RandomProcessor.Processor();

            //WeixinProcessor.Processor();

            //MachineKey mk = new MachineKey();
            //string sMachineKey = mk.GenerateKey();
            //Console.WriteLine(sMachineKey);

            Console.WriteLine("定时程序（随机数生成等）已启动就绪 /r/n");
            Console.WriteLine("按任意键结束");
            Console.ReadLine();
        }
    }
}
