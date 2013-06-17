using System;
using Docking.Components;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.ComponentModel.Design;
using Docking;

namespace Examples
{

   [System.ComponentModel.ToolboxItem(false)]
   public partial class LocalizationTest : Gtk.Bin, ILocalizable
   {
      public LocalizationTest()
      {
         this.Build();
      }

      // set the displayed name of the widget
      string ILocalizable.Name { get { return "Localization"; } }

      // currently nothing do to, but special cases can be considered
      void ILocalizable.LocalizationChanged(DockItem item) {}
   }
   
   #region Starter / Entry Point

   public class LocalizationTestFactory : ComponentFactory
   {
      public override Type TypeOfInstance { get { return typeof(LocalizationTest); } }
      public override String MenuPath { get { return @"View\Examples\Localization Test"; } }
      public override String Comment { get { return "Localization example"; } }
      public override Mode Options { get { return Mode.CloseOnHide; } }
      public override Gdk.Pixbuf Icon { get { return Gdk.Pixbuf.LoadFromResource ("Examples.HelloWorld-16.png"); } }
   }

   #endregion

}

