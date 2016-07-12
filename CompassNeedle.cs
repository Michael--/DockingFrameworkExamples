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
    #endregion

    #region mouse interaction

    void drawingarea_ButtonPressEvent(object o, ButtonPressEventArgs args)
    {
      if (args.Event.Button == LEFT_MOUSE_BUTTON)
      {
        drawingarea1.QueueDraw();
      }
    }

    void drawingarea_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
    {
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

