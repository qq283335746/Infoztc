﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.5485
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace="http://asp.net/ApplicationServices/v200", ConfigurationName="AuthenticationService")]
public interface AuthenticationService
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://asp.net/ApplicationServices/v200/AuthenticationService/ValidateUser", ReplyAction="http://asp.net/ApplicationServices/v200/AuthenticationService/ValidateUserRespons" +
        "e")]
    bool ValidateUser(string username, string password, string customCredential);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://asp.net/ApplicationServices/v200/AuthenticationService/Login", ReplyAction="http://asp.net/ApplicationServices/v200/AuthenticationService/LoginResponse")]
    bool Login(string username, string password, string customCredential, bool isPersistent);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://asp.net/ApplicationServices/v200/AuthenticationService/IsLoggedIn", ReplyAction="http://asp.net/ApplicationServices/v200/AuthenticationService/IsLoggedInResponse")]
    bool IsLoggedIn();
    
    [System.ServiceModel.OperationContractAttribute(Action="http://asp.net/ApplicationServices/v200/AuthenticationService/Logout", ReplyAction="http://asp.net/ApplicationServices/v200/AuthenticationService/LogoutResponse")]
    void Logout();
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface AuthenticationServiceChannel : AuthenticationService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class AuthenticationServiceClient : System.ServiceModel.ClientBase<AuthenticationService>, AuthenticationService
{
    
    public AuthenticationServiceClient()
    {
    }
    
    public AuthenticationServiceClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public AuthenticationServiceClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public AuthenticationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public AuthenticationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public bool ValidateUser(string username, string password, string customCredential)
    {
        return base.Channel.ValidateUser(username, password, customCredential);
    }
    
    public bool Login(string username, string password, string customCredential, bool isPersistent)
    {
        return base.Channel.Login(username, password, customCredential, isPersistent);
    }
    
    public bool IsLoggedIn()
    {
        return base.Channel.IsLoggedIn();
    }
    
    public void Logout()
    {
        base.Channel.Logout();
    }
}
