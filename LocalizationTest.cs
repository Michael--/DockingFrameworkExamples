using System;
using Docking.Components;
using System.Reflection;
using System.Resources;
using System.Collections;
using System.ComponentModel.Design;
using Docking;
using System.Collections.Generic;

namespace Examples
{

   [System.ComponentModel.ToolboxItem(false)]
   public partial class LocalizationTest : Component, ILocalizableComponent
   {
      public LocalizationTest()
      {
         this.Build();
      }

      // set the displayed name of the widget
      string ILocalizableComponent.Name { get { return "Localization"; } }

      // currently nothing do to, but special cases can be considered
      void ILocalizableComponent.LocalizationChanged(DockItem item) {}


      #region Component - Interaction

      LocalizationProperties mProperties = new LocalizationProperties();

      public override void ComponentAdded(object item)
      {
         base.ComponentAdded(item);
      }

      public override void ComponentRemoved(object item)
      {
         base.ComponentRemoved(item);
      }

      #endregion
   }

   public class LocalizationProperties
   {
      [System.ComponentModel.Category("LocalizationProperties.View")]
      [System.ComponentModel.DisplayName("LocalizationProperties.TestDouble")]
      [System.ComponentModel.Description("LocalizationProperties.Test property localization")]
      public double TestDouble { get; set; }
   }
   
   #region Starter / Entry Point

   public class LocalizationTestFactory : ComponentFactory
   {
      public override Type TypeOfInstance { get { return typeof(LocalizationTest); } }
      public override String MenuPath { get { return @"View\Examples\Localization Test"; } }
      public override String Comment { get { return "Localization example"; } }
      public override Gdk.Pixbuf Icon { get { return Gdk.Pixbuf.LoadFromResource("Examples.Resources.Example-16.png"); } }
   }

   #endregion

}

