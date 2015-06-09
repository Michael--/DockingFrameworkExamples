using System;
using Docking;
using Docking.Components;

namespace Examples
{
	[System.ComponentModel.ToolboxItem(false)]
	public partial class ExampleHelloWorld : Component
	{
		public ExampleHelloWorld ()
		{
			this.Build ();
            this.Name = "Hello World";
		}
	}


#region Starter / Entry Point

	public class ExampleHelloWorldFactory : ComponentFactory
	{
		public override Type TypeOfInstance { get { return typeof(ExampleHelloWorld); } }
      public override String MenuPath { get { return @"View\Examples\Hello World"; } }
		public override String Comment { get { return "minimal dockable component example"; } }
      public override Mode Options { get { return Mode.MultiInstance; } }
      public override Gdk.Pixbuf Icon { get { return Gdk.Pixbuf.LoadFromResource("Examples.Resources.Example-16.png"); } }
      public override string LicenseGroup { get { return "examples"; } }
   }

#endregion


}

