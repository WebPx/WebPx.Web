using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.Design;
using System.Windows;

namespace WebPx.Web.Controls.Design
{
    class ButtonDesigner : ControlDesigner
    {
        public ButtonDesigner()
        {

        }

        public override void InitializeNewComponent(System.Collections.IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);
            var button1 = (Button)this.Component;
            MessageBox.Show(button1.ID);
            button1.Text = button1.ID;
        }
    }
}
