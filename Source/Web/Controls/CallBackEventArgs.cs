using System;

namespace WebPx.Web.Controls
{
    public class CallBackEventArgs : EventArgs
    {
        public CallBackEventArgs(string argument)
        {
            this.Argument = argument;
        }

        public string Argument { get; private set; }

        public string Result { get; set; }
    }
}