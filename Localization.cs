using System;
using Docking.Components;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.ComponentModel.Design;

namespace Examples
{

   [System.ComponentModel.ToolboxItem(true)]
   public partial class Localization : Gtk.Bin
   {
      public Localization()
      {
         this.Build();
         this.Name = "Localization".L();
         this.label1.LabelProp = "Any Content".L(GetType().Namespace);
          this.button1.Label = "Reset".L(GetType().Namespace);
         "blabla".L();
      }
   }
   
   #region Starter / Entry Point

   public class LocalizationFactory : ComponentFactory
   {
      public override Type TypeOfInstance { get { return typeof(Localization); } }
      public override String MenuPath { get { return @"View\Examples\Localization"; } }
      public override String Comment { get { return "Localization example"; } }
      public override Mode Options { get { return Mode.CloseOnHide; } }
      public override Gdk.Pixbuf Icon { get { return Gdk.Pixbuf.LoadFromResource ("Examples.HelloWorld-16.png"); } }
   }

   #endregion

}

