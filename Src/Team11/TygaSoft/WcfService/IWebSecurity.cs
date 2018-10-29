using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Web.Security;

namespace TygaSoft.WcfService
{
    [ServiceContract(Namespace = "TygaSoft.Services.HnztcSecurityService")]
    public partial interface IWebSecurity
    {
        [OperationContract(Name = "Register")]
        string Register(string username, string password, string nickname);

        [OperationContract(Name = "Login")]
        string Login(string username, string password);

        [OperationContract(Name = "GetUserInfo")]
        string GetUserInfo(string username);

        [OperationContract(Name = "GetUserId")]
        object GetUserId(string username);

        [OperationContract(Name = "ValidateUser")]
        bool ValidateUser(string username, string password);

        [OperationContract(Name = "GetRandomNumber")]
        string GetRandomNumber(string prefix);

        [OperationContract(Name = "ChangePassword")]
        string ChangePassword(string username, string oldPassword, string newPassword);

        [OperationContract(Name = "UpdatePassword")]
        string UpdatePassword(string username, string newPassword);

    }
}
