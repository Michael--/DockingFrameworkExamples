using System;
using Docking;
using Docking.Components;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace DockingExamples
{
   [System.ComponentModel.ToolboxItem(false)]
   public partial class TestAsyncAwait : Component
   {
      public TestAsyncAwait()
      {
         this.Build();
         m_Scroll2EndMark = textview1.Buffer.CreateMark("Scroll2End", textview1.Buffer.EndIter, true);

         buttonStart.Clicked += async (sender, args) =>
         {
            WriteLine("Start async calculation");

            // this call is asyncronous and don't stop the UI, but wait for a result like a sync call
            // this method can call again while other async operations already running
            string result;

            // call async method
            if ((count % 2) == 0)
               result = await calcDataAsync("M1");

            // call sync method with a task async
            else
               result = await Task.Run(() => calcData("M2"));

            WriteLine(result);
         };
      }

      Gtk.TextMark m_Scroll2EndMark;
      int count = 0;
      Random rnd = new Random();

      public Task<string> calcDataAsync(string p)
      {
         return Task.Run(() => calcData(p));
      }

      public string calcData(string p)
      {
         Stopwatch total = new Stopwatch();
         total.Start();
         // wait 1..11 seconds, simulate calcultation time
         System.Threading.Thread.Sleep(1000 + rnd.Next() % 10000);
         total.Stop();
         return string.Format("Result {0}:{1} computed in {2:f1}s at {3}", p, ++count, total.Elapsed.TotalSeconds, DateTime.Now);
      }

      void WriteLine(string format, params object[] args)
      {
         lock (textview1)
         {
            Gtk.TextIter iter = textview1.Buffer.EndIter;
            String str = String.Format(format, args);
            textview1.Buffer.Insert(ref iter, str + "\r\n");
            textview1.Buffer.MoveMark(m_Scroll2EndMark, textview1.Buffer.EndIter);
            textview1.ScrollToMark(m_Scroll2EndMark, 0.0, false, 0, 0);
         }
      }

   }
   #region Starter / Entry Point

   public class TestAsyncAwaitFactory : ComponentFactory
   {
      public override Type TypeOfInstance { get { return typeof(TestAsyncAwait); } }
      public override String MenuPath { get { return @"View\Examples\Test async/await"; } }
      public override String Comment { get { return "Test the C# 5.0 feature async/await"; } }
      public override Mode Options { get { return Mode.MultiInstance; } }
      public override Gdk.Pixbuf Icon { get { return ResourceLoader_DockingExamples.LoadPixbuf("Example-16.png"); } }
      public override string LicenseGroup { get { return "examples"; } }
   }

   #endregion
}

