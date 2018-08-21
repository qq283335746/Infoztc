using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Web;
using TygaSoft.SysHelper;
using TygaSoft.ThreadProcessor;

namespace CAProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            //刮刮奖定时执行程序
            ScratchLottoProcessor.Processor();
        }
    }
}
