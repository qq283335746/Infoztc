using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Web.Security;

namespace TygaSoft.WcfSecurityService
{
    [ServiceContract(Namespace = "TygaSoft.Services.HnztcSecurityService")]
    public partial interface IWebSecurity
    {
        [OperationContract(Name = "Register")]
        string Register(string username, string password);

        [OperationContract(Name = "Login")]
        string Login(string username, string password);

        [OperationContract(Name = "GetUserId")]
        object GetUserId(string username);

        [OperationContract(Name = "ValidateUser")]
        bool ValidateUser(string username, string password);

    }
}
