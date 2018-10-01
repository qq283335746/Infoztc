using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TygaSoft.Model;
using TygaSoft.BLL;
using TygaSoft.CustomExceptions;
using TygaSoft.SysHelper;

namespace TygaSoft.WcfService
{
    public partial class HnztcSysService : IHnztcSys
    {
        #region IHnztcSys Member

        public void InsertSysLog(SyslogInfo syslogInfo)
        {
            try
            {
                SysLogInfo model = new SysLogInfo();
                model.AppName = syslogInfo.AppName;
                model.MethodName = syslogInfo.MethodName;
                model.Message = syslogInfo.Message;
                model.LastUpdatedDate = syslogInfo.LastUpdatedDate;

                SysLog bll = new SysLog();
                bll.Insert(model);
            }
            catch (Exception ex)
            {
                SysLogInfo model = new SysLogInfo();
                model.AppName = "应用程序日志服务系统";
                model.MethodName = "SysLogService.InsertSysLog";
                model.Message = "异常：" + ex.Message + "，请求参数：appName='" + syslogInfo.AppName + "',methodName='" + syslogInfo.MethodName + "',message='" + syslogInfo.Message + "'";
                model.LastUpdatedDate = syslogInfo.LastUpdatedDate;

                SysLog bll = new SysLog();
                bll.Insert(model);
            }
        }

        #endregion
    }
}
