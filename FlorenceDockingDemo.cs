using System;
using Docking.Components;
using System.Diagnostics;

namespace Examples
{
   [System.ComponentModel.ToolboxItem(false)]
   public partial class FlorenceDockingDemo : Component
   {
      public FlorenceDockingDemo(Type demo)
      {
         this.Build();
         this.Name = demo.Name;
         this.plotwidget1.InteractivePlotSurface2D = new Florence.InteractivePlotSurface2D();
         DemoLib.PlotSurface2DDemo.IDemo sample = (DemoLib.PlotSurface2DDemo.IDemo)Activator.CreateInstance(demo);
         sample.CreatePlot(this.plotwidget1.InteractivePlotSurface2D);      }
   }
   #region Starter / Entry Point

   public abstract class FlorenceFactoryBase : ComponentFactory
   {
       public abstract Type theInstance { get; }
       public override Type TypeOfInstance { get { return theInstance; } }
       public override String MenuPath { get { return @"View\Examples\Florence\" + theInstance.Name; } }
       public override String Comment { get { return theInstance.Name + " example"; } }
       public override Gdk.Pixbuf Icon { get { return Gdk.Pixbuf.LoadFromResource("Examples.Resources.Example-16.png"); } }
       public override string LicenseGroup { get { return "examples"; } }
   }

   public class FlorenceFactory1 : FlorenceFactoryBase
   {
       public class PlotWave : FlorenceDockingDemo { public PlotWave() : base(typeof(DemoLib.PlotSurface2DDemo.PlotWave)) { } }
       public override Type theInstance { get { return typeof(PlotWave); } }
   }

   public class FlorenceFactory2 : FlorenceFactoryBase
   {
       public class PlotDataSet : FlorenceDockingDemo { public PlotDataSet() : base(typeof(DemoLib.PlotSurface2DDemo.PlotDataSet)) { } }
       public override Type theInstance { get { return typeof(PlotDataSet); } }
   }

   public class FlorenceFactory3 : FlorenceFactoryBase
   {
       public class PlotMockup : FlorenceDockingDemo { public PlotMockup() : base(typeof(DemoLib.PlotSurface2DDemo.PlotMockup)) { } }
       public override Type theInstance { get { return typeof(PlotMockup); } }
   }

   public class FlorenceFactory4 : FlorenceFactoryBase
   {
       public class PlotImage : FlorenceDockingDemo { public PlotImage() : base(typeof(DemoLib.PlotSurface2DDemo.PlotImage)) { } }
       public override Type theInstance { get { return typeof(PlotImage); } }
   }

   public class FlorenceFactory5 : FlorenceFactoryBase
   {
       public class PlotQE : FlorenceDockingDemo { public PlotQE() : base(typeof(DemoLib.PlotSurface2DDemo.PlotQE)) { } }
       public override Type theInstance { get { return typeof(PlotQE); } }
   }

   public class FlorenceFactory6 : FlorenceFactoryBase
   {
       public class PlotMarkers : FlorenceDockingDemo { public PlotMarkers() : base(typeof(DemoLib.PlotSurface2DDemo.PlotMarkers)) { } }
       public override Type theInstance { get { return typeof(PlotMarkers); } }
   }

   public class FlorenceFactory7 : FlorenceFactoryBase
   {
       public class PlotLogAxis : FlorenceDockingDemo { public PlotLogAxis() : base(typeof(DemoLib.PlotSurface2DDemo.PlotLogAxis)) { } }
       public override Type theInstance { get { return typeof(PlotLogAxis); } }
   }

   public class FlorenceFactory8 : FlorenceFactoryBase
   {
       public class PlotLogLog : FlorenceDockingDemo { public PlotLogLog() : base(typeof(DemoLib.PlotSurface2DDemo.PlotLogLog)) { } }
       public override Type theInstance { get { return typeof(PlotLogLog); } }
   }

   public class FlorenceFactory9 : FlorenceFactoryBase
   {
       public class PlotParticles : FlorenceDockingDemo { public PlotParticles() : base(typeof(DemoLib.PlotSurface2DDemo.PlotParticles)) { } }
       public override Type theInstance { get { return typeof(PlotParticles); } }
   }

   public class FlorenceFactory10 : FlorenceFactoryBase
   {
       public class PlotWavelet : FlorenceDockingDemo { public PlotWavelet() : base(typeof(DemoLib.PlotSurface2DDemo.PlotWavelet)) { } }
       public override Type theInstance { get { return typeof(PlotWavelet); } }
   }

   public class FlorenceFactory11 : FlorenceFactoryBase
   {
       public class PlotSincFunction : FlorenceDockingDemo { public PlotSincFunction() : base(typeof(DemoLib.PlotSurface2DDemo.PlotSincFunction)) { } }
       public override Type theInstance { get { return typeof(PlotSincFunction); } }
   }

   public class FlorenceFactory12 : FlorenceFactoryBase
   {
       public class PlotGaussian : FlorenceDockingDemo { public PlotGaussian() : base(typeof(DemoLib.PlotSurface2DDemo.PlotGaussian)) { } }
       public override Type theInstance { get { return typeof(PlotGaussian); } }
   }

   public class FlorenceFactory13 : FlorenceFactoryBase
   {
       public class PlotLabelAxis : FlorenceDockingDemo { public PlotLabelAxis() : base(typeof(DemoLib.PlotSurface2DDemo.PlotLabelAxis)) { } }
       public override Type theInstance { get { return typeof(PlotLabelAxis); } }
   }

   public class FlorenceFactory14 : FlorenceFactoryBase
   {
       public class PlotCircular : FlorenceDockingDemo { public PlotCircular() : base(typeof(DemoLib.PlotSurface2DDemo.PlotCircular)) { } }
       public override Type theInstance { get { return typeof(PlotCircular); } }
   }

   public class FlorenceFactory15 : FlorenceFactoryBase
   {
       public class PlotCandle : FlorenceDockingDemo { public PlotCandle() : base(typeof(DemoLib.PlotSurface2DDemo.PlotCandle)) { } }
       public override Type theInstance { get { return typeof(PlotCandle); } }
   }

   public class FlorenceFactory16 : FlorenceFactoryBase
   {
       public class PlotABC : FlorenceDockingDemo { public PlotABC() : base(typeof(DemoLib.PlotSurface2DDemo.PlotABC)) { } }
       public override Type theInstance { get { return typeof(PlotABC); } }
   }

   #endregion

}

