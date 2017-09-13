using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
//using WebPx.Security;

namespace WebPx.Web.Controls
{
    [DefaultProperty("Text")]
    //[ToolboxData("<{0}:Button runat=server></{0}:Button>")]
    //[ToolboxItem(false)]
    [Designer(typeof(Design.ButtonDesigner))]
    [ParseChildren(true, "Text"), PersistChildren(false)]
    [DefaultEvent("Click")]
    public class Button : WebControl, IPostBackEventHandler, IServerCallback
    {
        public Button() : base(HtmlTextWriterTag.Button)
        {
            this.CausesValidation = true;
            this.CallBackCausesValidation = true;
            //this.ButtonType = Web.Controls.ButtonType.Submit;
        }

        //protected override HtmlTextWriterTag TagKey
        //{
        //    get
        //    {
        //        switch (this.ButtonType)
        //        {
        //            case ButtonType.Button: return HtmlTextWriterTag.Button;
        //            case ButtonType.Submit: return HtmlTextWriterTag.Button;
        //            case ButtonType.Link: return HtmlTextWriterTag.A;
        //        }
        //        return base.TagKey;
        //    }
        //}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string value = this.Page.Request[this.ClientID];
            if (!string.IsNullOrEmpty(value))
                RaiseClick();
        }

        private void RaiseClick()
        {
            OnClick();
        }

        protected virtual void OnClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                if (!ShouldSerializeText())
                    this.Text = this.ID;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        //[DefaultValue("")]
        [Localizable(true)]
        [PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        public bool ShouldSerializeText()
        {
            return !string.Equals(this.ID, this.Text);
        }

        public void ResetText()
        {
            this.Text = this.ID;
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (this.Click != null)
            {
                var script = base.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this, "", null, true, false, false, true, this.CausesValidation, this.ValidationGroup));
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, $"{script};return false;");
            }
            else
            {
                var hasCallBack = CallBack != null;
                var hasClientOnClick = !string.IsNullOrEmpty(OnClientClick);
                if (hasCallBack)
                {
                    var callBackScript = this.Page.ClientScript.GetCallbackEventReference(this, null, OnCallBackSuccess, CallBackContext, OnCallBackError, CallBackAsync);
                    if (CallBackWithFields)
                    {
                        var sb = new StringBuilder();
                        if (CallBackCausesValidation)
                            sb.Append($"if (!Page_ClientValidate('{this.ValidationGroup}')) return false;");
                        sb.Append("__theFormPostData = '';__theFormPostCollection=new Array();WebForm_InitCallback();");
                        sb.Append(callBackScript);
                        callBackScript = sb.ToString();
                    }
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, $"{callBackScript};return false;");
                }
                else if (hasClientOnClick)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, $"{OnClientClick};return false;");
                }
                else
                {
                    //switch (this.ButtonType)
                    //{
                    //    case ButtonType.Button: 
                    //    case ButtonType.Submit: 
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, this.ID);
                }
                //        break;
                //    case ButtonType.Link:
                //        writer.AddAttribute(HtmlTextWriterAttribute.Href, this.NavigateUrl);
                //        break;
                //}
                writer.AddAttribute(HtmlTextWriterAttribute.Name, this.ClientID);
            }
            base.AddAttributesToRender(writer);
        }

        //[Category("Appearance")]
        //[DefaultValue(ButtonType.Submit)]
        //public ButtonType ButtonType { get; set; }


        //[Bindable(true)]
        //[Category("Navigation")]
        //[DefaultValue("")]
        //[Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))] 
        //[UrlProperty]
        //[Description("HyperLink_NavigateUrl")]
        //public string NavigateUrl { get; set; }

        //Policy _accessPolicy = null;

        //[Category("Behavior")]
        ////[XmlArray("access-policy")]
        ////[XmlArrayItem("policy-rule")]
        //[DisplayNameAttribute("AccessPolicy")]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        //[PersistenceMode(PersistenceMode.InnerProperty)]
        //public WebPx.Security.Policy AccessPolicy
        //{
        //    get
        //    {
        //        if (_accessPolicy == null)
        //            _accessPolicy = new WebPx.Security.Policy();
        //        return _accessPolicy;
        //    }
        //}

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(this.Text);
        }

        public event EventHandler Click;

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            RaiseClick();
        }

        public String OnClientClick { get; set; }

        [DefaultValue("true")]
        public bool CausesValidation { get; set; }

        [DefaultValue("")]
        public string ValidationGroup { get; set; }

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
    }
}
