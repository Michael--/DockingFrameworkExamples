
// This file has been generated by the GUI designer. Do not modify.
namespace DockingExamples
{
	public partial class TestToolBarStatusBar
	{
		private global::Gtk.VBox vbox1;
		private global::Gtk.Label label1;
		private global::Gtk.Label label2;
		private global::Gtk.Label label3;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget DockingExamples.TestToolBarStatusBar
			global::Stetic.BinContainer.Attach (this);
			this.Name = "DockingExamples.TestToolBarStatusBar";
			// Container child DockingExamples.TestToolBarStatusBar.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox ();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = "Add Toolbar Buttons";
			this.vbox1.Add (this.label1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.label1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.LabelProp = "Add and remove Staus Bar messages";
			this.vbox1.Add (this.label2);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.label2]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.LabelProp = "message text";
			this.vbox1.Add (this.label3);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.label3]));
			w3.Position = 2;
			w3.Expand = false;
			w3.Fill = false;
			this.Add (this.vbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
