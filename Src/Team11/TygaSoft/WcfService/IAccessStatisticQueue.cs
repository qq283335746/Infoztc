using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TygaSoft.WcfService
{
    [ServiceContract(Namespace = "TygaSoft.Services.HnztcQueueService")]
    public partial interface IAccessStatisticQueue
    {
        #region IAccessStatisticQueue Member

        [OperationContract(IsOneWay = true)]
        void SaveAccessStatistic(AccessStatisticInfo accessStatisticInfo);

        #endregion
    }
}
