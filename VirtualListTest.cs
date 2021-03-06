using System;
using Docking.Components;
using Gtk;
using System.Collections.Generic;
using Docking;
using System.Xml.Serialization;

namespace DockingExamples
{
    [System.ComponentModel.ToolboxItem(false)]
    public partial class VirtualListTest : Component
    {
        public VirtualListTest ()
        {
            this.Build();
            this.Name = "Virtual List Test";
        }

        private String GetContent(int row, int column)
        {
            switch (column)
            {
                case 0:
                    return String.Format("{0}", row + 1);
                case 1:
                    return String.Format("{0}:{1}", row + 1, column + 1);
                case 2:
                    return String.Format("Content of row {0}", row + 1);
            }
            return "?";
        }

        public override void Loaded()
        {
            base.Loaded();

            virtuallistview1.ComponentManager = this.ComponentManager;

            // callback requesting data to display
            virtuallistview1.GetContentDelegate = GetContent;

            // add simple label columns
            virtuallistview1.AddColumn(0, "Index", 75, true);
            virtuallistview1.AddColumn(1, "Context", 75, true);

            // add a more complex custom made Column
            VBox box = new VBox();
            box.PackStart(new Label("Message"), false, false, 0);
            box.PackStart(new Entry(""), false, false, 0);
            virtuallistview1.AddColumn(2, "Message", box, 150, true);

            // set content size
            virtuallistview1.RowCount = 42000;
            virtuallistview1.TriggerRepaint();
        }
    }


#region Starter / Entry Point

public class VirtualListTestFactory : ComponentFactory
{
    public override Type TypeOfInstance { get { return typeof(VirtualListTest); } }
    public override String MenuPath { get { return @"View\Examples\Virtual List Test"; } }
    public override String Comment { get { return "Test widget for testing virtual list view"; } }
    public override Mode Options { get { return Mode.MultiInstance; } }
    public override Gdk.Pixbuf Icon { get { return ResourceLoader_DockingExamples.LoadPixbuf("VirtualListTest-16.png"); } }
    public override string LicenseGroup { get { return "examples"; } }
}

#endregion

}
