using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TygaSoft.DBUtility;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.CustomExceptions;

namespace TygaSoft.ThreadProcessor
{
    public class ScratchLottoProcessor
    {
        public static void Processor()
        {
            Thread workThread = new Thread(new ThreadStart(WorkProcess));
            workThread.IsBackground = true;
            workThread.SetApartmentState(ApartmentState.STA);
            workThread.Start();
        }

        private static void WorkProcess()
        {
            try
            {
                while (true)
                {
                    DateTime currTime = DateTime.Now;
                    if (currTime.ToString("HH:mm:ss") == "23:59:59" || currTime.ToString("HH:mm:ss") == "00:00:00")
                    {
                        //休息5分钟，让时间不在指定执行范围内
                        Thread.Sleep(30000);
                        continue;
                    }
                    var startTime = DateTime.Parse(string.Format("{0} {1}", currTime.ToString("yyyy-MM-dd"), "23:59:59"));
                    var ts = startTime - currTime;
                    if (ts.TotalMilliseconds > 0)
                    {
                        Thread.Sleep(ts);
                    }

                    #region 定时执行的业务逻辑

                    //您的代码

                    ScratchLotto bll = new ScratchLotto();
                    bll.UpdateWinningTimes();

                    #endregion

                    //执行任务完后休息5分钟，让时间不在指定执行范围内
                    Thread.Sleep(30000);
                }
            }
            catch (Exception ex)
            {
                new CustomException(ex.Message, ex);
            }

        }
    }
}
