using System;
using System.Drawing;
using Docking.Components;
using Docking.Tools;
using Gtk;

namespace DockingExamples
{
   [System.ComponentModel.ToolboxItem(false)]
   public partial class CompassNeedle : Component, IPersistable, ILocalizableComponent
   {
      public CompassNeedle()
      {
         this.Build();
         drawingarea1.Events |= Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask | Gdk.EventMask.ButtonMotionMask;

         drawingarea1.ButtonPressEvent += drawingarea_ButtonPressEvent;
         drawingarea1.ButtonReleaseEvent += drawingarea_ButtonReleaseEvent;
         drawingarea1.MotionNotifyEvent += drawingarea_MotionNotifyEvent;

         drawingarea1.ExposeEvent += new ExposeEventHandler(OnDrawingareaExposeEvent);
      }

      #region ILocalizableComponent

      string ILocalizableComponent.Name { get { return "Compass Needle"; } }

      void ILocalizableComponent.LocalizationChanged(Docking.DockItem item)
      {
      }
      #endregion

      #region persistency & properties
      class MyProperties
      {
      }

      void IPersistable.SaveTo(IPersistency persistency)
      {
         // string instance = DockItem.Id.ToString();
      }


      void IPersistable.LoadFrom(IPersistency persistency)
      {
         // string instance = DockItem.Id.ToString();

         //m_Properties = new MyProperties();
      }

      #endregion

      #region member variables
      //MyProperties m_Properties;
      double m_AngleQuota = 0;
      #endregion

      #region mouse interaction

      void drawingarea_ButtonPressEvent(object o, ButtonPressEventArgs args)
      {
      }

      void drawingarea_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
      {
         if ((args.Event.State & Gdk.ModifierType.Button1Mask) != 0)
         {
            int width, height;
            GdkWindow.GetSize(out width, out height);
            var p1 = new PointF(width / 2, height / 2);
            var p2 = new PointF((float)args.Event.X, (float)args.Event.Y);
            m_AngleQuota = Coord.AngleDegree(p1, p2);
            drawingarea1.QueueDraw();
         }
      }

      void drawingarea_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
      {
      }

      #endregion

      #region drawings

      void OnDrawingareaExposeEvent(object o, ExposeEventArgs args)
      {
         Gdk.EventExpose expose = args.Args[0] as Gdk.EventExpose;
         Gdk.Window win = expose.Window;
         int width, height;
         win.GetSize(out width, out height);
         Gdk.Rectangle exposeRect = expose.Area;


         if (true)
         {
            var layout = new Pango.Layout(PangoContext)
            {
               FontDescription = Pango.FontDescription.FromString("Tahoma 12")
            };
            var gc = new Gdk.GC(win)
            {
               RgbFgColor = Color.Blue.ToGdk(),
            };
            var text = "Under construction...";
            layout.SetText(text);
            Drawing.DrawLayout(win, gc, layout, width / 2, height / 2, Drawing.Origin.Center, Drawing.Origin.Center);
         }

         using (var context = Gdk.CairoHelper.Create(win))
         {
            double scale = Math.Min(width, height) / 100.0 / 2;
            context.Translate(width / 2, height / 2);
            context.Scale(scale, scale);

            // draw clock tick marks
            {
               context.Save();

               // minute tick marks
               context.SetDash(new double[] { 1.5, 3 * Math.PI - 1.5 }, 1);
               Drawing.Ellipse(context, PointF.Empty, 90, Color.Black, false, 3);

               // hour tick marks
               context.SetDash(new double[] { 1.5, 15 * Math.PI - 1.5 }, 1);
               Drawing.Ellipse(context, PointF.Empty, 90, Color.Blue, false, 5);

               context.Restore();
            }

            DrawHand(context, 0, Color.DarkBlue);
            DrawQuota(context, m_AngleQuota, Color.DarkRed);


         }
      }

    void DrawHand(Cairo.Context context, double angle, Color color)
    {
      context.Save();
      context.Rotate(angle * Math.PI / 180.0);
      context.MoveTo(1, 10);
      context.LineTo(0, -85);
      context.LineTo(-1, 10);
      context.ClosePath();
      context.SetSourceColor(color.ToCairo());
      context.Stroke();
      context.Restore();
    }

    void DrawQuota(Cairo.Context context, double angle, Color color)
    {
      context.Save();
      context.Rotate(angle * Math.PI / 180.0);
      context.MoveTo(0, -95);
      context.LineTo(1, -110);
      context.LineTo(-1, -110);
      context.ClosePath();
      context.SetSourceColor(color.ToCairo());
      context.Stroke();
      context.Restore();
    }


    #endregion


  }

  #region Starter / Entry Point

  public class CompassNeedleFactory : ComponentFactory
  {
    public override Type TypeOfInstance { get { return typeof(CompassNeedle); } }
    public override String MenuPath { get { return @"View\Examples\Compass Needle"; } }
    public override String Comment { get { return "Compass needle turning filter"; } }
    public override Mode Options { get { return Mode.MultiInstance; } }
    public override Gdk.Pixbuf Icon { get { return ResourceLoader_DockingExamples.LoadPixbuf("Example-16.png"); } }
    public override string LicenseGroup { get { return "examples"; } }
  }

  #endregion

}

