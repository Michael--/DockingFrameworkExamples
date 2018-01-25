using System;
using Docking;
using Docking.Components;

namespace DockingExamples
{
	[System.ComponentModel.ToolboxItem(false)]
	public partial class HelloWorld : Component
	{
		public HelloWorld()
		{
			this.Build();
         this.Name = "Hello World";
		}

      protected void OnButton1Clicked (object sender, EventArgs e)
      {
         MessageBox.Show(Gtk.MessageType.Info ,"Hello :)");
      }
	}

   #region Starter / Entry Point
	public class HelloWorldFactory : ComponentFactory
	{
		public override Type       TypeOfInstance { get { return typeof(HelloWorld); } }
      public override String     MenuPath       { get { return @"View\Examples\Hello World"; } }
		public override String     Comment        { get { return "minimal dockable component example"; } }
      public override Mode       Options        { get { return Mode.MultiInstance; } }
      public override Gdk.Pixbuf Icon           { get { return ResourceLoader_DockingExamples.LoadPixbuf("Example-16.png"); } }
      public override string     LicenseGroup   { get { return "examples"; } }
   }
   #endregion

}
