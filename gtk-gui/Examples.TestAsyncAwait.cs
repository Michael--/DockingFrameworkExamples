
// This file has been generated by the GUI designer. Do not modify.
namespace Examples
{
	public partial class TestAsyncAwait
	{
		private global::Gtk.VBox vbox1;
		private global::Gtk.Button buttonStart;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TextView textview1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Examples.TestAsyncAwait
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Examples.TestAsyncAwait";
			// Container child Examples.TestAsyncAwait.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.buttonStart = new global::Gtk.Button ();
			this.buttonStart.CanFocus = true;
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.UseUnderline = true;
			this.buttonStart.Label = global::Mono.Unix.Catalog.GetString ("Start");
			this.vbox1.Add (this.buttonStart);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.buttonStart]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.textview1 = new global::Gtk.TextView ();
			this.textview1.CanFocus = true;
			this.textview1.Name = "textview1";
			this.textview1.Editable = false;
			this.GtkScrolledWindow.Add (this.textview1);
			this.vbox1.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.GtkScrolledWindow]));
			w3.Position = 1;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
