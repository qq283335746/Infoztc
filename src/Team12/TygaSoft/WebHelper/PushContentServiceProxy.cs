﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.33440
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// 此源代码由 wsdl 自动生成, Version=4.0.30319.1。
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="PushContentServiceSoap", Namespace="http://x.hna.net/mobile")]
public partial class PushContentService : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback ReceivePushContentOperationCompleted;
    
    /// <remarks/>
    public PushContentService() {
        this.Url = "http://220.174.246.93:20023/pushserver/PushContentService.asmx";
    }
    
    /// <remarks/>
    public event ReceivePushContentCompletedEventHandler ReceivePushContentCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://x.hna.net/mobile/ReceivePushContent", RequestNamespace="http://x.hna.net/mobile", ResponseNamespace="http://x.hna.net/mobile", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string ReceivePushContent(string xmlParameters) {
        object[] results = this.Invoke("ReceivePushContent", new object[] {
                    xmlParameters});
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginReceivePushContent(string xmlParameters, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("ReceivePushContent", new object[] {
                    xmlParameters}, callback, asyncState);
    }
    
    /// <remarks/>
    public string EndReceivePushContent(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void ReceivePushContentAsync(string xmlParameters) {
        this.ReceivePushContentAsync(xmlParameters, null);
    }
    
    /// <remarks/>
    public void ReceivePushContentAsync(string xmlParameters, object userState) {
        if ((this.ReceivePushContentOperationCompleted == null)) {
            this.ReceivePushContentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReceivePushContentOperationCompleted);
        }
        this.InvokeAsync("ReceivePushContent", new object[] {
                    xmlParameters}, this.ReceivePushContentOperationCompleted, userState);
    }
    
    private void OnReceivePushContentOperationCompleted(object arg) {
        if ((this.ReceivePushContentCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.ReceivePushContentCompleted(this, new ReceivePushContentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
public delegate void ReceivePushContentCompletedEventHandler(object sender, ReceivePushContentCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.1")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ReceivePushContentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal ReceivePushContentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public string Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((string)(this.results[0]));
        }
    }
}
