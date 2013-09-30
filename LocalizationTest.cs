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
   public partial class LocalizationTest : Component, ILocalizableComponent, IComponentInteract
   {
      public LocalizationTest()
      {
         this.Build();
      }

      // set the displayed name of the widget
      string ILocalizableComponent.Name { get { return "Localization"; } }

      // currently nothing do to, but special cases can be considered
      void ILocalizableComponent.LocalizationChanged(DockItem item) {}


      #region IComponentInteract
      List<IProperty> mProperty = new List<IProperty>();
      LocalizationProperties mProperties = new LocalizationProperties();

      void IComponentInteract.Added(object item)
      {
         if (item is IProperty)
         {
            IProperty property = item as IProperty;

            mProperty.Add(property);
            property.PropertyChanged += (e) =>
            {
               // if (e.Object == myObject) update...
            };
         }
      }

      void IComponentInteract.Removed(object item)
      {
         if (item is IProperty)
         {
            IProperty property = item as IProperty;
            mProperty.Remove(property);
         }
      }

      void IComponentInteract.Visible(object item, bool visible)
      {
      }

      void IComponentInteract.Current(object item)
      {
         if (item == this)
            foreach (IProperty p in mProperty)
               p.SetObject(mProperties);
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
      public override Mode Options { get { return Mode.CloseOnHide; } }
      public override Gdk.Pixbuf Icon { get { return Gdk.Pixbuf.LoadFromResource("Examples.Resources.HelloWorld-16.png"); } }
   }

   #endregion

}

