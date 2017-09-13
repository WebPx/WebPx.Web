using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WebPx.Web.Controls
{
    public interface IServerCallback : ICallbackEventHandler
    {
        event CallBackEventHandler CallBack;
        string OnCallBackError { get; set; }
        string OnCallBackSuccess { get; set; }
        string CallBackContext { get; set; }
        bool CallBackAsync { get; set; }
        bool CallBackWithFields { get; set; }
        bool CallBackCausesValidation { get; set; }
    }
}
