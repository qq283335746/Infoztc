using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TygaSoft.WcfService
{
    [ServiceContract(Namespace = "TygaSoft.Services.HnztcQueueService")]
    public partial interface IUserBaseQueue
    {
        #region IUserBaseQueue Member

        [OperationContract(IsOneWay = true)]
        void SaveUserLevel(UserLevelInfo userLevelInfo);

        #endregion
    }
}
