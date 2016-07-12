using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Docking.Components;
using Docking.Tools;
using Gtk;

namespace DockingExamples
{
   [System.ComponentModel.ToolboxItem(false)]
   public partial class Clothoides : Component, IPersistable, ILocalizableComponent
   {
      #region construction 
      public Clothoides()
      {
         this.Build();
         drawingarea1.Events |= Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask | Gdk.EventMask.ButtonMotionMask;

         drawingarea1.ButtonPressEvent += drawingarea_ButtonPressEvent;
         drawingarea1.ButtonReleaseEvent += drawingarea_ButtonReleaseEvent;
         drawingarea1.MotionNotifyEvent += drawingarea_MotionNotifyEvent;

         drawingarea1.ExposeEvent += new ExposeEventHandler(OnDrawingareaExposeEvent);
      }
    #endregion

    #region ILocalizableComponent

    string ILocalizableComponent.Name { get { return "Clothoides"; } }

    void ILocalizableComponent.LocalizationChanged(Docking.DockItem item)
    {
    }
    #endregion


    #region component interaction

    public override void Loaded()
      {
         base.Loaded();
         SetPropertyObject(m_Properties);
      }

      public override void PropertyChanged()
      {
         base.PropertyChanged();
         drawingarea1.QueueDraw();
      }

      #endregion

      #region persistency & properties
      class MyProperties
      {
         public bool DrawConnections { get; set; }
         public bool DrawInformation { get; set; }
         public bool DrawConstructionsPoints { get; set; }
         public bool DrawWholeInnerCircle { get; set; }
         public Color ColorStraight { get; set; }
         public Color ColorLine { get; set; }
         public Color ColorCircle { get; set; }
         public Color ColorClothoide { get; set; }
         public Color ColorOther { get; set; }
         public Color ColorNodes { get; set; }
         public Color ColorNodeSelected { get; set; }
      }

      void IPersistable.SaveTo(IPersistency persistency)
      {
         string instance = DockItem.Id.ToString();

         persistency.SaveSetting(instance, "ColorStraight", m_Properties.ColorStraight);
         persistency.SaveSetting(instance, "ColorLine", m_Properties.ColorLine);
         persistency.SaveSetting(instance, "ColorCircle", m_Properties.ColorCircle);
         persistency.SaveSetting(instance, "ColorClothoide", m_Properties.ColorClothoide);
         persistency.SaveSetting(instance, "ColorOther", m_Properties.ColorOther);
         persistency.SaveSetting(instance, "ColorNodes", m_Properties.ColorNodes);
         persistency.SaveSetting(instance, "ColorNodeSelected", m_Properties.ColorNodeSelected);
         persistency.SaveSetting(instance, "DrawConnections", m_Properties.DrawConnections);
         persistency.SaveSetting(instance, "DrawInformation", m_Properties.DrawInformation);
         persistency.SaveSetting(instance, "DrawWholeInnerCircle", m_Properties.DrawWholeInnerCircle);
         persistency.SaveSetting(instance, "DrawConstructionsPoints", m_Properties.DrawConstructionsPoints);

         persistency.SaveSetting(instance, "NodeCount", m_Nodes.Count);
         for (int i = 0; i < m_Nodes.Count; i++)
         {
            var n = m_Nodes.ElementAt(i);
            persistency.SaveSetting(instance, "NodeX" + i.ToString(), n.X);
            persistency.SaveSetting(instance, "NodeY" + i.ToString(), n.Y);
         }
      }


      void IPersistable.LoadFrom(IPersistency persistency)
      {
         string instance = DockItem.Id.ToString();

         m_Properties = new MyProperties();
         m_Properties.ColorStraight = persistency.LoadSetting(instance, "ColorStraight", Color.Brown);
         m_Properties.ColorLine = persistency.LoadSetting(instance, "ColorLine", Color.Beige);
         m_Properties.ColorCircle = persistency.LoadSetting(instance, "ColorCircle", Color.Red);
         m_Properties.ColorClothoide = persistency.LoadSetting(instance, "ColorClothoide", Color.Green);
         m_Properties.ColorOther = persistency.LoadSetting(instance, "ColorOther", Color.Brown);
         m_Properties.ColorNodes = persistency.LoadSetting(instance, "ColorNodes", Color.Blue);
         m_Properties.ColorNodeSelected = persistency.LoadSetting(instance, "ColorNodeSelected", Color.YellowGreen);
         m_Properties.DrawConnections = persistency.LoadSetting(instance, "DrawConnections", m_Properties.DrawConnections);
         m_Properties.DrawInformation = persistency.LoadSetting(instance, "DrawInformation", m_Properties.DrawInformation);
         m_Properties.DrawWholeInnerCircle = persistency.LoadSetting(instance, "DrawWholeInnerCircle", m_Properties.DrawWholeInnerCircle);
         m_Properties.DrawConstructionsPoints = persistency.LoadSetting(instance, "DrawConstructionsPoints", m_Properties.DrawConstructionsPoints);

         int count = persistency.LoadSetting(instance, "NodeCount", 0);
         for (int i = 0; i < count; i++)
         {
            var X = persistency.LoadSetting(instance, "NodeX" + i.ToString(), 0.0);
            var Y = persistency.LoadSetting(instance, "NodeY" + i.ToString(), 0.0);
            m_Nodes.Add(new PointF((float)X, (float)Y));
         }
      }

      #endregion

      #region member variables
      MyProperties m_Properties;
      List<PointF> m_Nodes = new List<PointF>();
      const int NodeSize = 15;

      int m_SelectedNode = -1;
      int m_HoveredNode = -1;
      PointF m_Moved = PointF.Empty; // helper show us node has been moved by mouse
      #endregion


      #region mouse interaction

      void drawingarea_ButtonPressEvent(object o, ButtonPressEventArgs args)
      {
         if (args.Event.Button == LEFT_MOUSE_BUTTON)
         {
            m_SelectedNode = m_HoveredNode;
            if (m_SelectedNode < 0)
            {
               lock (m_Nodes)
               {
                  m_HoveredNode = m_SelectedNode = m_Nodes.Count;
                  ComponentManager.MessageWriteLine("Node:{0} add", m_SelectedNode);
                  m_Nodes.Add(new PointF((float)args.Event.X, (float)args.Event.Y));
               }
            }
            m_Moved = m_Nodes.ElementAt(m_SelectedNode);
            drawingarea1.QueueDraw();
         }
      }

      void drawingarea_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
      {
         var p = new PointF((float)args.Event.X, (float)args.Event.Y);
         if (m_SelectedNode >= 0)
         {
            lock (m_Nodes)
               m_Nodes[m_SelectedNode] = p;
            drawingarea1.QueueDraw();
         }
         else
         {
            var h = FindNode(p, 20);
            if (h != m_HoveredNode)
            {
               if (h >= 0)
                  ComponentManager.MessageWriteLine("Node:{0} select", h);
               else
                  ComponentManager.MessageWriteLine("Node:{0} release", m_HoveredNode);

               m_HoveredNode = h;
               drawingarea1.QueueDraw();
            }
         }
      }

      void drawingarea_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
      {
         if (m_SelectedNode >= 0)
         {
            var n = m_Nodes.ElementAt(m_SelectedNode);
            if (m_Moved != n)
               ComponentManager.MessageWriteLine("Node:{0} moved", m_SelectedNode);
            int width, height;
            GdkWindow.GetSize(out width, out height);
            if (n.X < 0 || n.Y < 0 || n.X > width || n.Y > height)
            {
               ComponentManager.MessageWriteLine("Node:{0} removed", m_SelectedNode);
               lock (m_Nodes)
                  m_Nodes.RemoveAt(m_SelectedNode);
               m_HoveredNode = -1;
               drawingarea1.QueueDraw();
            }
         }
         m_SelectedNode = -1;
      }

      int FindNode(PointF p, double maxDistance)
      {
         lock (m_Nodes)
         {
            int result = -1;
            double best = double.MaxValue;
            for (int i = 0; i < m_Nodes.Count; i++)
            {
               var s = m_Nodes.ElementAt(i);
               // hit test
               double v = Math.Sqrt(Math.Pow(s.X - p.X, 2) + Math.Pow(s.Y - p.Y, 2));
               if (v < best)
               {
                  result = i;
                  best = v;
               }
            }
            if (best < maxDistance)
               return result;
            return -1;
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


         if (m_Nodes.Count == 0)
         {
            var layout = new Pango.Layout(PangoContext)
            {
               FontDescription = Pango.FontDescription.FromString("Tahoma 12")
            };
            var gc = new Gdk.GC(win)
            {
               RgbFgColor = Color.Blue.ToGdk(),
            };
            var text = "Add some nodes by clicking into window.\n" +
               "Move nodes with the mouse.\n" +
               "Remove nodes by moving out of inner area";
            layout.SetText(text);
            Drawing.DrawLayout(win, gc, layout, width / 2, height / 2, Drawing.Origin.Center, Drawing.Origin.Center);

         }

         lock (m_Nodes)
         {
            using (var context = Gdk.CairoHelper.Create(win))
            {
               // collect all straight line segments
               var straights = new List<PointF>();
               straights.Add(m_Nodes.FirstOrDefault());

               // Draw connection
               if (m_Properties.DrawConnections)
                  DrawConnections(context);

               // Draw text at connections
               if (m_Properties.DrawInformation)
                  DrawInfos(context);

               // Calculate & draw clothoides
               CalculateAndDrawClothoides(context, straights);

               // Draw nodes
               DrawNodes(win, context);
            }
         }
      }

      /// <summary>
      /// Calculates the and draw clothoides.
      /// The selected algorithm is very raw and not optimized.
      /// Some optional debug output are commented out.
      /// </summary>
      void CalculateAndDrawClothoides(Cairo.Context context, List<PointF> straights)
      {
         for (int i = 1; i < m_Nodes.Count - 1; i++)
         {
            PointF left = m_Nodes[i - 1];
            PointF center = m_Nodes[i];
            PointF right = m_Nodes[i + 1];
            double a1 = Coord.AngleRad(center, left);
            double a2 = Coord.AngleRad(center, right);
            double adiff = Coord.AngleDifferenceRad(a1, a2);
            double len1 = Coord.Length(left, center) / 4;
            double len2 = Coord.Length(center, right) / 4;
            double len = Math.Min(len1, len2);
            PointF k1 = center.OffsetPolar(len, a1);
            PointF k2 = center.OffsetPolar (len, a2);

            // clothoide start from straight
            if (m_Properties.DrawConstructionsPoints)
            {
               Drawing.Ellipse(context, k1, 3, m_Properties.ColorOther, true, 1);
               Drawing.Ellipse(context, k2, 3, m_Properties.ColorOther, true, 1);
            }

            // calculate circle touch booth points k1, k2 / center between lines
            double YM = len * Math.Tan(adiff / 2);
            int signR = Math.Sign(YM);
            YM = Math.Abs(YM);
            double baseAngle = a1 - Math.PI / 2 * signR;
            PointF M = k1.OffsetPolar(YM, baseAngle);
            // Drawing.Ellipse (context, M, YM);

            // Circle center
            if (m_Properties.DrawConstructionsPoints)
               Drawing.Ellipse(context, M, 3, m_Properties.ColorOther, true, 1);

            // define and calculate clothoide
            double tauMax = (Math.PI - Math.Abs(adiff)) / 2;
            double divider = 0.1;
            double dR, R, tau;
            do
            {
               dR = Math.Min(5, YM * divider);
               R = YM - dR;
               tau = Math.Sqrt((24 * R + 6 * dR) * dR) / (2 * R);
               divider *= 0.95;
            }
            while (Math.Abs(tau) > tauMax);

            // whole center circle
            if (m_Properties.DrawWholeInnerCircle)
               Drawing.Ellipse(context, M, R, m_Properties.ColorOther, false, 1);

            PointF UE1 = M.OffsetPolar(R, a1 + (Math.PI / 2 + tau) * signR);
            PointF UE2 = M.OffsetPolar(R, a2 - (Math.PI / 2 + tau) * signR);

            // clothoides end to circle
            if (m_Properties.DrawConstructionsPoints)
            {
               Drawing.Ellipse(context, UE1, 3, m_Properties.ColorOther, true, 1);
               Drawing.Ellipse(context, UE2, 3, m_Properties.ColorOther, true, 1);
            }

            // Draw Arc, inner part of turn
            double angle1 = -Math.PI / 2 + a1 + (Math.PI / 2 + tau) * signR;
            double angle2 = -Math.PI / 2 + a2 - (Math.PI / 2 + tau) * signR;
            Drawing.Arc(context, M, R, angle1, angle2, signR >= 0, m_Properties.ColorCircle, false, 1);

            double L = tau * 2 * R;
            double A2 = L * R;

            // draw clothoides
            {
               // calculate more than necessary offsets on the clothoide 
               // this simple algorithm is not effecient, but only a solution
               var offset = new List<SizeF>();
               double dt = L / 50;
               double X = 0;
               double Y = 0;
               for (double dl = 0; dl < L; dl += dt)
               {
                  double d = dl * dl / (2 * A2);
                  X += Math.Cos(d) * dt;
                  Y += Math.Sin(d) * dt;

                  var newOffset = new SizeF((float)X, (float)Y);

                  // get 1st and last offset
                  if (offset.Count == 0 || dl + dt >= L)
                  {
                     offset.Add(newOffset);
                  }
                  // and some more with a little distance to the previous offset
                  else
                  {
                     var diff = SizeF.Subtract(offset.Last(), newOffset);
                     if (Math.Abs(diff.Height) > 5 || Math.Abs(diff.Width) > 5)
                        offset.Add(newOffset);
                  }
               }

               // draw clothoide from end to start of definition
               double scale = 1;
               int last = offset.Count - 1;
               float ox = offset[last].Width;
               float oy = offset[last].Height;
               double ax1 = a1 + Math.PI / 2;
               double ax2 = -a2 + Math.PI / 2;
               if (signR < 0)
                  ax1 += Math.PI;
               if (signR > 0)
                  ax2 -= Math.PI;

               var clo1 = new List<PointF>();
               var clo2 = new List<PointF>();
               foreach (var v in Enumerable.Reverse(offset))
               {
                  X = (v.Width - ox) * scale;
                  Y = (v.Height - oy) * scale;

                  double xx = X * signR;
                  {
                     double xr = xx * Math.Cos(ax1) - Y * Math.Sin(ax1);
                     double yr = Y * Math.Cos(ax1) + xx * Math.Sin(ax1);
                     PointF pt = UE1.Offset(xr, yr);
                     clo1.Add(pt);
                  }
                  {
                     double xr = xx * Math.Cos(ax2) - Y * Math.Sin(ax2);
                     double yr = Y * Math.Cos(ax2) + xx * Math.Sin(ax2);
                     PointF pt = UE2.Offset(xr, -yr);
                     clo2.Add(pt);
                  }
               }
               Drawing.Polyline(context, clo1, m_Properties.ColorClothoide, 1);
               Drawing.Polyline(context, clo2, m_Properties.ColorClothoide, 1);
               straights.Add(clo1.LastOrDefault());
               straights.Add(clo2.LastOrDefault());
            }
         }
         straights.Add(m_Nodes.LastOrDefault());
         for (int j = 0; j < straights.Count(); j += 2)
         {
            var line = straights.GetRange(j, 2);
            Drawing.Polyline(context, line, m_Properties.ColorStraight, 1);
         }
      }

      void DrawConnections(Cairo.Context context)
      {
         var points = new List<PointF>();
         foreach (var p in m_Nodes)
            points.Add(p);

         Drawing.Polyline(context, points, m_Properties.ColorLine, 1);
      }

      void DrawInfos(Cairo.Context context)
      {
         context.Save();
         context.SelectFontFace("Tahoma 10", Cairo.FontSlant.Normal, Cairo.FontWeight.Bold);
         // cairo.SetFontSize(10);

         for (int i = 0; i < m_Nodes.Count - 1; i++)
         {
            var a = m_Nodes[i];
            var b = m_Nodes[i + 1];
            var c = Coord.Center(a, b);
            double angle = Coord.AngleRad(a, b);
            double len = Coord.Length(a, b);
            string text = string.Format("{0:F1} {1:F1}°", len, Coord.Rad2Degree(angle));
            Drawing.DrawText(context, (int)c.X, (int)c.Y, angle, text);
         }
         context.Restore();
      }

      void DrawNodes(Gdk.Window win, Cairo.Context context)
      {
         var layout = new Pango.Layout(PangoContext)
         {
            FontDescription = Pango.FontDescription.FromString("Tahoma 10")
         };
         var gc = new Gdk.GC(win)
         {
            RgbFgColor = Color.Yellow.ToGdk(),
         };
         for (int i = 0; i < m_Nodes.Count; i++)
         {
            var a = m_Nodes.ElementAt(i);
            DrawNode(context, a, m_Properties.ColorNodes);
            layout.SetText(i.ToString());
            Drawing.DrawLayout(win, gc, layout, a.X, a.Y, Drawing.Origin.Center, Drawing.Origin.Center);
         }

         if (m_HoveredNode >= 0)
         {
            var a = m_Nodes.ElementAt(m_HoveredNode);
            DrawNode(context, a, m_Properties.ColorNodeSelected);
            layout.SetText(m_HoveredNode.ToString());
            Drawing.DrawLayout(win, gc, layout, a.X, a.Y, Drawing.Origin.Center, Drawing.Origin.Center);
         }
      }

      void DrawNode(Cairo.Context context, PointF p, Color color)
      {
         var center = new PointF(p.X, p.Y);
         center.Offset(-NodeSize / 2, -NodeSize / 2);
         Drawing.Ellipse(context, center, NodeSize / 2, color, true, 1);
      }

      #endregion
   }


   #region Drawing helper class
   public static class Drawing
   {
      public static void Polyline(Cairo.Context context, IEnumerable<PointF> points, Color color, double width = 1.0)
      {
         context.Save();

         if (points.Count() < 2)
            return;

         context.LineWidth = width;
         context.LineCap = Cairo.LineCap.Round;
         context.LineJoin = Cairo.LineJoin.Round;
         context.SetSourceColor(color.ToCairo());

         context.MoveTo(points.ElementAt(0).X, points.ElementAt(0).Y);
         for (int i = 1; i < points.Count(); i++)
         {
            context.LineTo(points.ElementAt(i).X, points.ElementAt(i).Y);
         }
         context.Stroke();
         context.Restore();
      }

      public static void Arc(Cairo.Context context, PointF p, double r, double a1, double a2, bool clockwise, Color color, bool filled, double width = 1.0)
      {
         context.Save();
         context.Translate(p.X, p.Y);

         if (!filled)
         {
            context.LineWidth = width;
            context.LineCap = Cairo.LineCap.Butt;
            context.LineJoin = Cairo.LineJoin.Bevel;
         }
         context.SetSourceColor(color.ToCairo());

         if (clockwise)
            context.Arc(0, 0, r, a1, a2);
         else context.ArcNegative(0, 0, r, a1, a2);

         if (filled)
            context.Fill();
         else
            context.Stroke();

         context.Restore();
      }

      public static void Ellipse(Cairo.Context context, PointF p, double r, Color color, bool filled, double width = 1.0)
      {
         context.Save();
         context.Translate(p.X, p.Y);

         if (!filled)
         {
            context.LineWidth = width;
            context.LineCap = Cairo.LineCap.Butt;
            context.LineJoin = Cairo.LineJoin.Bevel;
         }
         context.SetSourceColor(color.ToCairo());

         context.Arc(0, 0, r, 0, Math.PI * 2);

         if (filled)
            context.Fill();
         else
            context.Stroke();

         context.Restore();
      }



      public enum Origin
      {
         Near,
         Center,
         Far
      }

      public static void DrawText(Cairo.Context context, float x, float y, double angle, string text)
      {
         var ext = context.TextExtents(text);

         context.Save();
         context.Translate(x, y);
         context.Rotate(angle - Math.PI / 2);
         context.Translate(-ext.Width / 2, 0);
         context.ShowText(text);
         context.Stroke();

         context.Restore();
      }

      public static void DrawLayout(Gdk.Window win, Gdk.GC gc, Pango.Layout layout, float x, float y, Origin vertical = Origin.Far, Origin horizontal = Origin.Center)
      {
         if (horizontal != Origin.Near || vertical != Origin.Near)
         {
            int width, height;
            layout.GetPixelSize(out width, out height);

            switch (horizontal)
            {
               case Origin.Near:   /*nothing to do*/
                  break;
               case Origin.Center:
                  x -= width / 2;
                  break;
               case Origin.Far:
                  x -= width;
                  break;
            }

            switch (vertical)
            {
               case Origin.Near:   /*nothing to do*/
                  break;
               case Origin.Center:
                  y -= height / 2;
                  break;
               case Origin.Far:
                  y -= height;
                  break;
            }
         }
         win.DrawLayout(gc, (int)x, (int)y, layout);
      }

   }

   #endregion

   #region coordinate helper

   public static class Coord
   {
      public static PointF OffsetPolar(this PointF a, double len, double angle)
      {
         a.X += (float)(Math.Sin(angle) * len);
         a.Y -= (float)(Math.Cos(angle) * len);
         return a;
      }

      public static PointF Offset(this PointF a, double dx, double dy)
      {
         a.X += (float)dx;
         a.Y += (float)dy;
         return a;
      }

      public static PointF Center(PointF a, PointF b)
      {
         return new PointF((a.X + b.X) / 2, (a.Y + b.Y) / 2);
      }

      public static double Length(PointF a, PointF b)
      {
         double dy = b.Y - a.Y;
         double dx = b.X - a.X;
         return Math.Sqrt(dx * dx + dy * dy);
      }

      public static double AngleDegree(PointF a, PointF b)
      {
         double dy = b.Y - a.Y;
         double dx = b.X - a.X;
         double v = 90 + Math.Atan2(dy, dx) * 180 / Math.PI;
         if (v < 0)
            v += 360;
         else if (v > 360)
            v -= 360;
         return v;
      }

      public static double AngleRad(PointF a, PointF b)
      {
         return Degree2Rad(AngleDegree(a, b));
      }

      public static double Degree2Rad(double a)
      {
         return a * Math.PI / 180;
      }

      public static double Rad2Degree(double a)
      {
         return a * 180 / Math.PI;
      }

      public static double AngleDifferenceRad(double a, double b)
      {
         return Degree2Rad(AngleDifferenceDegree(Rad2Degree(a), Rad2Degree(b)));
      }

      public static double AbsAngleDifferenceRad(double a, double b)
      {
         return Degree2Rad(AbsAngleDifferenceDegree(Rad2Degree(a), Rad2Degree(b)));
      }

      static public double AbsAngleDifferenceDegree(double a, double b)
      {
         a = Math.Abs(b - a);
         if (a > 180)
            a = 360 - a;
         return a;
      }

      static public double AngleDifferenceDegree(double a, double b)
      {
         double d = a - b;
         if (d > 180)
            d -= 360;
         else if (d < -180)
            d += 360;
         return d;
      }

   }
   #endregion

   #region Starter / Entry Point

   public class ClothoidesFactory : ComponentFactory
   {
      public override Type TypeOfInstance { get { return typeof(Clothoides); } }
      public override String MenuPath { get { return @"View\Examples\Clothoides"; } }
      public override String Comment { get { return "Examine the nature of clothoides"; } }
      public override Mode Options { get { return Mode.MultiInstance; } }
      public override Gdk.Pixbuf Icon { get { return ResourceLoader_DockingExamples.LoadPixbuf("Example-16.png"); } }
      public override string LicenseGroup { get { return "examples"; } }
   }

   #endregion

}

