
// This file has been generated by the GUI designer. Do not modify.
namespace DockingExamples
{
	public partial class HelloWorld
	{
		private global::Gtk.VBox vbox3;
		
		private global::Gtk.Button button1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget DockingExamples.HelloWorld
			global::Stetic.BinContainer.Attach (this);
			this.Name = "DockingExamples.HelloWorld";
			// Container child DockingExamples.HelloWorld.Gtk.Container+ContainerChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.button1 = new global::Gtk.Button ();
			this.button1.CanFocus = true;
			this.button1.Name = "button1";
			this.button1.UseUnderline = true;
			this.button1.Label = "Click me!";
			this.vbox3.Add (this.button1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.button1]));
			w1.Position = 1;
			w1.Expand = false;
			w1.Fill = false;
			this.Add (this.vbox3);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.button1.Clicked += new global::System.EventHandler (this.OnButton1Clicked);
		}
	}
}
