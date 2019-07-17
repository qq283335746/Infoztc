using System;
using System.Runtime.Serialization;

namespace TygaSoft.CustomExceptions
{
    [Serializable()]
    public class CustomException : Exception,ISerializable
    {
        public CustomException() { }

        public CustomException(string message)
            : base(message)
        {
            HnztcSysClient sysClient = new HnztcSysClient();
            TygaSoft.Services.HnztcSysService.SyslogInfo sysLogInfo = new Services.HnztcSysService.SyslogInfo();
            sysLogInfo.AppName = "海南直通车后台系统";
            sysLogInfo.MethodName = "CustomException";
            sysLogInfo.Message = message;
            sysLogInfo.LastUpdatedDate = DateTime.Now;
            sysClient.InsertSysLog(sysLogInfo);
        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        {
            HnztcSysClient sysClient = new HnztcSysClient();
            TygaSoft.Services.HnztcSysService.SyslogInfo sysLogInfo = new Services.HnztcSysService.SyslogInfo();
            sysLogInfo.AppName = "海南直通车后台系统";
            sysLogInfo.MethodName = innerException == null ? "" : innerException.TargetSite.Name;
            sysLogInfo.Message = message;
            if (innerException != null) sysLogInfo.Message += innerException.Source + innerException.StackTrace;
            sysLogInfo.LastUpdatedDate = DateTime.Now;
            sysClient.InsertSysLog(sysLogInfo);
        }

        protected CustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
