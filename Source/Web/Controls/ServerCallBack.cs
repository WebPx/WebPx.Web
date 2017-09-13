using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPx.Web.Controls
{
    [DefaultEvent("CallBack")]
    [ToolboxData("<{0}:ServerCallBack runat=server></{0}:ServerCallBack>")]
    [ParseChildren(true), PersistChildren(false)]
    public class ServerCallBack : WebControl, IServerCallback
    {
        public ServerCallBack()
        {
            
        }

        protected override void Render(HtmlTextWriter writer)
        {

        }

        #region CallBack Members

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            var args = new CallBackEventArgs(eventArgument);
            CallBack?.Invoke(this, args);
            callBackResult = args.Result;
        }

        private string callBackResult = null;

        string ICallbackEventHandler.GetCallbackResult()
        {
            return callBackResult;
        }

        public event CallBackEventHandler CallBack;

        [Category("Client Behavior")]
        [DefaultValue("")]
        public string OnCallBackError { get; set; }

        [Category("Client Behavior")]
        [DefaultValue("")]
        public string OnCallBackSuccess { get; set; }

        [Category("Client Behavior")]
        [DefaultValue("")]
        public string CallBackContext { get; set; }

        [Category("Client Behavior")]
        [DefaultValue(false)]
        public bool CallBackAsync { get; set; }

        [Category("Client Behavior")]
        [DefaultValue(false)]
        public bool CallBackWithFields { get; set; }

        [Category("Client Behavior")]
        [DefaultValue(true)]
        public bool CallBackCausesValidation { get; set; }
        #endregion

        public string GetCallBackScript(string argument)
        {
            return base.Page.ClientScript.GetCallbackEventReference(this, argument, OnCallBackSuccess, CallBackContext, OnCallBackError, this.CallBackAsync);
        }
    }
}
