using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
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

         drawingarea1.ExposeEvent += OnDrawingareaExposeEvent;
      }

      #region ILocalizableComponent

      string ILocalizableComponent.Name { get { return "Compass Needle"; } }

      void ILocalizableComponent.LocalizationChanged(Docking.DockItem item)
      {
      }
      #endregion

      #region component interaction
      public override void Loaded()
      {
         base.Loaded();
         SetPropertyObject(m_Properties);
         Timer();
      }

      public override void PropertyChanged()
      {
         base.PropertyChanged();
         Update();
      }

      void Update()
      {
         m_HeadingSmoother.setIterationRate(m_Properties.IterationRate);
         m_HeadingSmoother.setDamping(m_Properties.Damping);
         m_HeadingSmoother.setAcceleration(m_Properties.Acceleration);
         if (m_Properties.FrameRate > 0)
            m_FrameDelay = (int)(1000.0 / m_Properties.FrameRate);
      }

      #endregion

      #region persistency & properties
      class MyProperties
      {
         public MyProperties()
         {
            FrameRate = 30;
            IterationRate = 25;
            Acceleration = 0.04;
            Damping = 0.7;
         }

         [System.ComponentModel.Category("CompassNeedle.Behaviour")]
         [System.ComponentModel.DisplayName("CompassNeedle.FrameRate")]
         [System.ComponentModel.Description("CompassNeedle.FrameRate-Description")]
         public uint FrameRate { get; set; }

         [System.ComponentModel.Category("CompassNeedle.Behaviour")]
         [System.ComponentModel.DisplayName("CompassNeedle.IterationRate")]
         [System.ComponentModel.Description("CompassNeedle.IterationRate-Description")]
         public uint IterationRate { get; set; }

         [System.ComponentModel.Category("CompassNeedle.Behaviour")]
         [System.ComponentModel.DisplayName("CompassNeedle.Acceleration")]
         [System.ComponentModel.Description("CompassNeedle.Acceleration-Description")]
         public double Acceleration { get; set; }

         [System.ComponentModel.Category("CompassNeedle.Behaviour")]
         [System.ComponentModel.DisplayName("CompassNeedle.Damping")]
         [System.ComponentModel.Description("CompassNeedle.Damping-Description")]
         public double Damping { get; set; }
      }

      void IPersistable.SaveTo(IPersistency persistency)
      {
         // string instance = DockItem.Id.ToString();
      }


      void IPersistable.LoadFrom(IPersistency persistency)
      {
         // string instance = DockItem.Id.ToString();

         m_Properties = new MyProperties();
         m_HeadingSmoother = new HeadingSmoother();
         Update();
      }

      #endregion

      #region member variables
      MyProperties m_Properties;
      int m_FrameDelay = 25;
      double m_AngleQuota = 0;
      double m_AngleCurrent = 0;
      HeadingSmoother m_HeadingSmoother;
      #endregion

      #region mouse interaction

      void drawingarea_ButtonPressEvent(object o, ButtonPressEventArgs args)
      {
         if (args.Event.Button == LEFT_MOUSE_BUTTON)
            UpdateQuota(new PointF((float)args.Event.X, (float)args.Event.Y));
      }

      void drawingarea_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
      {
         if ((args.Event.State & Gdk.ModifierType.Button1Mask) != 0)
            UpdateQuota(new PointF((float)args.Event.X, (float)args.Event.Y));
      }

      void drawingarea_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
      {
      }

      void UpdateQuota(PointF point)
      {
         int width, height;
         GdkWindow.GetSize(out width, out height);
         var center = new PointF(width / 2, height / 2);
         var angle = Coord.AngleDegree(center, point);
         m_HeadingSmoother.setQuota(angle);
      }

      #endregion

      #region Timer

      async void Timer()
      {
         while (true)
         {
            await Task.Delay(m_FrameDelay);
            m_AngleQuota = m_HeadingSmoother.getQuota();
            m_AngleCurrent = m_HeadingSmoother.getValue();

            if (this.DockItem.ContentVisible)
              drawingarea1.QueueDraw();
         }
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
            var text = "CompassNeedle.ShortHelp".Localized();
            layout.SetText(text);
            Drawing.DrawLayout(win, gc, layout, width / 2, height / 3 * 2, Drawing.Origin.Center, Drawing.Origin.Center);
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

            DrawHand(context, m_AngleCurrent, Color.DarkBlue);
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

  #region Filter Algorithm

  public class ValueSmoother
  {
    public ValueSmoother()
    {
      m_TimeStep = 40;
      m_Time = 0;
      m_LastValue = 0;
      m_Value = 0;
      m_Quota = 0;
      m_Rate = 0;
      m_Acceleration = 0.04;
      m_Damping = 0.7;
      stopwatch.Start();
    }

    public void setQuota(double quota)
    {
      m_Quota = quota;
    }

    public double getQuota()
    {
      return m_Quota;
    }

    public void setIterationRate(UInt32 rate)
    {
      m_TimeStep = 1000 / rate;
    }

    public void setAcceleration(double acc)
    {
      m_Acceleration = acc;
    }

    public void setDamping(double da)
    {
      m_Damping = da;
    }

    // get calculated heading at current timestamp
    virtual public double getValue()
    {
      UInt32 now = getCurrentTimestamp();
      UInt32 tdiff = now - m_Time;

      // check last bit, an overflow is possible because of our time raster
      // in that case no iteration step is necessary, but only interpolation
      bool overflow = (tdiff & 0x80000000) != 0;

      if (!overflow || m_Time == 0)
      {
        // 1st call or last call is far away, assume quota has been reached, avoid huge iteration steps
        if (tdiff > 3000 || m_Time == 0)
        {
          m_Time = now;
          m_Rate = 0;
          m_LastValue = m_Quota;
          m_Value = m_Quota;
          return m_Value;
        }

        // any time gap from last call, iterate smoothing algorithm 
        if (tdiff > 0)
        {
          UInt32 iterations = tdiff / m_TimeStep + 1;
          for (UInt32 i = 0; i < iterations; i++)
          {
            m_Time += m_TimeStep;
            double d = getDifference(m_Quota, m_Value);
            m_Rate += d * m_Acceleration; // acceleration
            m_Rate *= m_Damping;          // damping
            m_LastValue = m_Value;
            setValue(m_Value + m_Rate);
          }
        }
      }

      // now we have 2 Heading available
      // the last heading in the past and the newest heading in the future
      // with a time gap between both of m_TimeStep (25ms)
      // because the request time is anytime between, we need to calc an intermediate heading
      // note: m_Time is >= now, expected value between 0..m_Rate
      UInt32 dt = m_Time - now;

      // linear interpolation 
      double relation = (double)dt / (double)m_TimeStep;
      double dh = getDifference(m_LastValue, m_Value);
      double value = m_Value + relation * dh;
      return value;
    }

    virtual protected double getDifference(double ri1, double ri2)
    {
      var diff = ri1 - ri2;
      return diff;
    }

    Stopwatch stopwatch = new Stopwatch();

    private UInt32 getCurrentTimestamp()
    {
      return (UInt32)stopwatch.ElapsedMilliseconds;
    }

    virtual protected void setValue(double value)
    {
      m_Value = value;
    }

    UInt32 m_TimeStep;        // calculation time raster in milliseconds, e.g. 25ms is a proper value
    UInt32 m_Time;            // current calculated time, rastered in m_TimeStep
    double m_LastValue;     // the last calculated heading (time is now or past)
    protected double m_Value;         // the newest calculted heading (time is now or future
    double m_Quota;           // the heading we disire to reach with the filter algorithm
    double m_Rate;            // current heading change rate 
    double m_Acceleration;    // acceleration value
    double m_Damping;         // damping value
  }

  public class HeadingSmoother : ValueSmoother
  {
    protected override double getDifference(double ri1, double ri2)
    {
      var diff = ri1 - ri2;
      if (diff > 180)
        diff -= 360;
      else if (diff < -180)
        diff += 360;
      return diff;
    }

    protected override void setValue(double value)
    {
      base.setValue(value);
      if (m_Value < 0)
        m_Value += 360;
      else if (m_Value >= 360)
        m_Value -= 360;
    }

    public override double getValue()
    {
      var v = base.getValue();
      if (v < 0)
        v += 360;
      else if (v >= 360)
        v -= 360;
      return v;
    }
  }


  #endregion

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

