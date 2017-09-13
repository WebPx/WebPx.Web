using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;

namespace WebPx.Web.Controls.Design
{
    class NoteDesigner : ControlDesigner
    {
        public NoteDesigner()
        {

        }

        Note note;

        public override void Initialize(IComponent component)
        {
            note = (Note)component;
            base.Initialize(component);
            SetViewFlags(ViewFlags.DesignTimeHtmlRequiresLoadComplete, true);
            SetViewFlags(ViewFlags.TemplateEditing, true);
        }

        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {
            IDesignerHost host = (IDesignerHost)Component.Site.GetService(typeof(IDesignerHost));
            if (host != null)
            {
                ITemplate contentTemplate;
                //if (_currentRegion == 0)
                {
                    contentTemplate = note.ContentTemplate;
                    return ControlPersister.PersistTemplate(contentTemplate, host);
                }
            }
            return String.Empty;
        }

        public override void SetEditableDesignerRegionContent(EditableDesignerRegion region, string content)
        {
            if (content == null)
                return;
            IDesignerHost host = (IDesignerHost)Component.Site.GetService(typeof(IDesignerHost));
            if (host != null)
            {
                ITemplate template = ControlParser.ParseTemplate(host, content);
                if (template != null)
                {
                    //if (_currentRegion == 0)
                    {
                        note.ContentTemplate = template;
                    }
                }
            }
        }

        protected override bool HidePropertiesInTemplateMode
        {
            get
            {
                return false;
            }
        }

        public override string GetDesignTimeHtml(DesignerRegionCollection regions)
        {
            var control = this.Component as Control;
            var contentRegionId = $"{control.ClientID}$Content";
            regions.Add(new EditableDesignerRegion(this, contentRegionId) { Highlight = true });

            return base.GetDesignTimeHtml(regions);
        }
    }
}
