
// This file has been generated by the GUI designer. Do not modify.
namespace Examples
{
 public partial class Levenshtein
 {
  private global::Gtk.VBox vbox3;
  private global::Gtk.HBox hbox2;
  private global::Gtk.Label label;
  private global::Gtk.Entry entryLine;
  private global::Gtk.Label labelBestMatch;
  private global::Gtk.ScrolledWindow GtkScrolledWindow;
  private global::Gtk.TextView textview;

  protected virtual void Build ()
  {
   global::Stetic.Gui.Initialize (this);
   // Widget Examples.Levenshtein
   global::Stetic.BinContainer.Attach (this);
   this.Name = "Examples.Levenshtein";
   // Container child Examples.Levenshtein.Gtk.Container+ContainerChild
   this.vbox3 = new global::Gtk.VBox ();
   this.vbox3.Name = "vbox3";
   this.vbox3.Spacing = 6;
   // Container child vbox3.Gtk.Box+BoxChild
   this.hbox2 = new global::Gtk.HBox ();
   this.hbox2.Name = "hbox2";
   this.hbox2.Spacing = 6;
   // Container child hbox2.Gtk.Box+BoxChild
   this.label = new global::Gtk.Label ();
   this.label.Name = "label";
   this.label.LabelProp = "Search for:";
   this.hbox2.Add (this.label);
   global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.label]));
   w1.Position = 0;
   w1.Expand = false;
   w1.Fill = false;
   // Container child hbox2.Gtk.Box+BoxChild
   this.entryLine = new global::Gtk.Entry ();
   this.entryLine.CanFocus = true;
   this.entryLine.Name = "entryLine";
   this.entryLine.IsEditable = true;
   this.entryLine.InvisibleChar = '●';
   this.hbox2.Add (this.entryLine);
   global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.entryLine]));
   w2.Position = 1;
   // Container child hbox2.Gtk.Box+BoxChild
   this.labelBestMatch = new global::Gtk.Label ();
   this.labelBestMatch.Name = "labelBestMatch";
   this.labelBestMatch.LabelProp = "bestMatch";
   this.hbox2.Add (this.labelBestMatch);
   global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.labelBestMatch]));
   w3.Position = 2;
   w3.Expand = false;
   w3.Fill = false;
   this.vbox3.Add (this.hbox2);
   global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox2]));
   w4.Position = 0;
   w4.Expand = false;
   w4.Fill = false;
   // Container child vbox3.Gtk.Box+BoxChild
   this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
   this.GtkScrolledWindow.Name = "GtkScrolledWindow";
   this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
   // Container child GtkScrolledWindow.Gtk.Container+ContainerChild
   this.textview = new global::Gtk.TextView ();
   this.textview.CanFocus = true;
   this.textview.Name = "textview";
   this.textview.WrapMode = ((global::Gtk.WrapMode)(2));
   this.GtkScrolledWindow.Add (this.textview);
   this.vbox3.Add (this.GtkScrolledWindow);
   global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.GtkScrolledWindow]));
   w6.Position = 1;
   this.Add (this.vbox3);
   if ((this.Child != null)) {
    this.Child.ShowAll ();
   }
   this.Hide ();
  }
 }
}
