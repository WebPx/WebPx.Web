using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace WebPx.Web.Controls
{
    /*
        <div class="note note-success">
            <h4 class="block">Bootstrap Native Modals</h4>
            <p> Modals are streamlined, but flexible, dialog prompts with the minimum required functionality and smart defaults. If you need more control over the Bootstrap Modals please check out
                <a class="btn btn-outline red" href="ui_extended_modals.html">
                extended modals plugin</a>
            </p>
        </div>
     */
    [DefaultProperty("Caption")]
    [ToolboxData("<{0}:Modal runat=server></{0}:Modal>")]
    [ParseChildren(false), PersistChildren(false)]
    [Designer(typeof(Design.NoteDesigner))]
    public class Note : WebControl, INamingContainer
    {
        public Note() : base(HtmlTextWriterTag.Div)
        {
            this.CssClass = defaultCssClass;
            this.HeaderCssClass = defaultHeaderCssClass;
        }

        private const string defaultCssClass = "note";

        [DefaultValue(defaultCssClass)]
        public override string CssClass { get { return base.CssClass; } set { base.CssClass = value; } }

        private const string defaultHeaderCssClass = "block";

        [DefaultValue(defaultHeaderCssClass)]
        public virtual string HeaderCssClass { get; set; }

        public string Caption { get; set; }

        private ITemplate _contentTemplate;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [TemplateContainer(typeof(Note))]
        [Browsable(false)]
        [TemplateInstance(TemplateInstance.Single)]
        [Description("Template for the Content")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ITemplate ContentTemplate
        {
            get
            {
                return _contentTemplate;
            }
            set
            {
                _contentTemplate = value;
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            if (this.ContentTemplate != null)
            {
                this.ContentTemplate.InstantiateIn(this);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            this.RenderHeader(writer);
            if (DesignMode)
            {
                writer.AddAttribute(DesignerRegion.DesignerRegionAttributeName, $"{this.ClientID}$Content");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }
            base.RenderChildren(writer);
            if (DesignMode)
                writer.RenderEndTag();
        }

        private void RenderHeader(HtmlTextWriter output)
        {

            var headerClass = this.HeaderCssClass;

            if (!string.IsNullOrEmpty(headerClass))
                output.AddAttribute(HtmlTextWriterAttribute.Class, headerClass);
            output.RenderBeginTag(HtmlTextWriterTag.H4);

            output.Write(this.Caption);

            output.RenderEndTag();
        }
    }
}
