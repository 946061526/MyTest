﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace MyTest.WebReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="upLoadFilesSoap", Namespace="http://tempuri.org/")]
    public partial class upLoadFiles : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback upLoadOneChunkOperationCompleted;
        
        private System.Threading.SendOrPostCallback upLoadFirstChunkOperationCompleted;
        
        private System.Threading.SendOrPostCallback upLoadOtherChunkOperationCompleted;
        
        private System.Threading.SendOrPostCallback upLoadLastChunkOperationCompleted;
        
        private System.Threading.SendOrPostCallback testOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public upLoadFiles() {
            this.Url = global::MyTest.Properties.Settings.Default.MyTest_WebReference_upLoadFiles;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event upLoadOneChunkCompletedEventHandler upLoadOneChunkCompleted;
        
        /// <remarks/>
        public event upLoadFirstChunkCompletedEventHandler upLoadFirstChunkCompleted;
        
        /// <remarks/>
        public event upLoadOtherChunkCompletedEventHandler upLoadOtherChunkCompleted;
        
        /// <remarks/>
        public event upLoadLastChunkCompletedEventHandler upLoadLastChunkCompleted;
        
        /// <remarks/>
        public event testCompletedEventHandler testCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/upLoadOneChunk", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string upLoadOneChunk([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] bytes, string fileName, string savepath) {
            object[] results = this.Invoke("upLoadOneChunk", new object[] {
                        bytes,
                        fileName,
                        savepath});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void upLoadOneChunkAsync(byte[] bytes, string fileName, string savepath) {
            this.upLoadOneChunkAsync(bytes, fileName, savepath, null);
        }
        
        /// <remarks/>
        public void upLoadOneChunkAsync(byte[] bytes, string fileName, string savepath, object userState) {
            if ((this.upLoadOneChunkOperationCompleted == null)) {
                this.upLoadOneChunkOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupLoadOneChunkOperationCompleted);
            }
            this.InvokeAsync("upLoadOneChunk", new object[] {
                        bytes,
                        fileName,
                        savepath}, this.upLoadOneChunkOperationCompleted, userState);
        }
        
        private void OnupLoadOneChunkOperationCompleted(object arg) {
            if ((this.upLoadOneChunkCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.upLoadOneChunkCompleted(this, new upLoadOneChunkCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/upLoadFirstChunk", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string upLoadFirstChunk([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] bytes, string fileName) {
            object[] results = this.Invoke("upLoadFirstChunk", new object[] {
                        bytes,
                        fileName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void upLoadFirstChunkAsync(byte[] bytes, string fileName) {
            this.upLoadFirstChunkAsync(bytes, fileName, null);
        }
        
        /// <remarks/>
        public void upLoadFirstChunkAsync(byte[] bytes, string fileName, object userState) {
            if ((this.upLoadFirstChunkOperationCompleted == null)) {
                this.upLoadFirstChunkOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupLoadFirstChunkOperationCompleted);
            }
            this.InvokeAsync("upLoadFirstChunk", new object[] {
                        bytes,
                        fileName}, this.upLoadFirstChunkOperationCompleted, userState);
        }
        
        private void OnupLoadFirstChunkOperationCompleted(object arg) {
            if ((this.upLoadFirstChunkCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.upLoadFirstChunkCompleted(this, new upLoadFirstChunkCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/upLoadOtherChunk", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string upLoadOtherChunk([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] bytes, int fileLen, string fileName) {
            object[] results = this.Invoke("upLoadOtherChunk", new object[] {
                        bytes,
                        fileLen,
                        fileName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void upLoadOtherChunkAsync(byte[] bytes, int fileLen, string fileName) {
            this.upLoadOtherChunkAsync(bytes, fileLen, fileName, null);
        }
        
        /// <remarks/>
        public void upLoadOtherChunkAsync(byte[] bytes, int fileLen, string fileName, object userState) {
            if ((this.upLoadOtherChunkOperationCompleted == null)) {
                this.upLoadOtherChunkOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupLoadOtherChunkOperationCompleted);
            }
            this.InvokeAsync("upLoadOtherChunk", new object[] {
                        bytes,
                        fileLen,
                        fileName}, this.upLoadOtherChunkOperationCompleted, userState);
        }
        
        private void OnupLoadOtherChunkOperationCompleted(object arg) {
            if ((this.upLoadOtherChunkCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.upLoadOtherChunkCompleted(this, new upLoadOtherChunkCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/upLoadLastChunk", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string upLoadLastChunk(string fileName, string savepath) {
            object[] results = this.Invoke("upLoadLastChunk", new object[] {
                        fileName,
                        savepath});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void upLoadLastChunkAsync(string fileName, string savepath) {
            this.upLoadLastChunkAsync(fileName, savepath, null);
        }
        
        /// <remarks/>
        public void upLoadLastChunkAsync(string fileName, string savepath, object userState) {
            if ((this.upLoadLastChunkOperationCompleted == null)) {
                this.upLoadLastChunkOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupLoadLastChunkOperationCompleted);
            }
            this.InvokeAsync("upLoadLastChunk", new object[] {
                        fileName,
                        savepath}, this.upLoadLastChunkOperationCompleted, userState);
        }
        
        private void OnupLoadLastChunkOperationCompleted(object arg) {
            if ((this.upLoadLastChunkCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.upLoadLastChunkCompleted(this, new upLoadLastChunkCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/test", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string test(string str_bytes, string fileName, string savepath) {
            object[] results = this.Invoke("test", new object[] {
                        str_bytes,
                        fileName,
                        savepath});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void testAsync(string str_bytes, string fileName, string savepath) {
            this.testAsync(str_bytes, fileName, savepath, null);
        }
        
        /// <remarks/>
        public void testAsync(string str_bytes, string fileName, string savepath, object userState) {
            if ((this.testOperationCompleted == null)) {
                this.testOperationCompleted = new System.Threading.SendOrPostCallback(this.OntestOperationCompleted);
            }
            this.InvokeAsync("test", new object[] {
                        str_bytes,
                        fileName,
                        savepath}, this.testOperationCompleted, userState);
        }
        
        private void OntestOperationCompleted(object arg) {
            if ((this.testCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.testCompleted(this, new testCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void upLoadOneChunkCompletedEventHandler(object sender, upLoadOneChunkCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class upLoadOneChunkCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal upLoadOneChunkCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void upLoadFirstChunkCompletedEventHandler(object sender, upLoadFirstChunkCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class upLoadFirstChunkCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal upLoadFirstChunkCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void upLoadOtherChunkCompletedEventHandler(object sender, upLoadOtherChunkCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class upLoadOtherChunkCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal upLoadOtherChunkCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void upLoadLastChunkCompletedEventHandler(object sender, upLoadLastChunkCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class upLoadLastChunkCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal upLoadLastChunkCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    public delegate void testCompletedEventHandler(object sender, testCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1055.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class testCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal testCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
}

#pragma warning restore 1591