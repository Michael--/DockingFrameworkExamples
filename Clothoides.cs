using System;
using Docking.Components;

namespace DockingExamples
{
   [System.ComponentModel.ToolboxItem (false)]
   public partial class Clothoides : Component
   {
      public Clothoides ()
      {
         this.Build ();
      }
   }

   #region Starter / Entry Point

   public class ClothoidesFactory : ComponentFactory
   {
      public override Type TypeOfInstance { get { return typeof (Clothoides); } }
      public override String MenuPath { get { return @"View\Examples\Clothoides"; } }
      public override String Comment { get { return "Examine the nature of clothoides"; } }
      public override Mode Options { get { return Mode.MultiInstance; } }
      public override Gdk.Pixbuf Icon { get { return ResourceLoader_DockingExamples.LoadPixbuf ("Example-16.png"); } }
      public override string LicenseGroup { get { return "examples"; } }
   }

   #endregion

}

