using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TygaSoft.WcfService
{
    [ServiceContract(Namespace = "TygaSoft.Services.HnztcSysService")]
    public partial interface IHnztcSys
    {
        #region IHnztcSys Member

        [OperationContract(IsOneWay = true)]
        void InsertSysLog(SyslogInfo syslogInfo);

        #endregion
    }
}
