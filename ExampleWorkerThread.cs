using System;
using Docking.Components;
using Docking;
using Docking.Tools;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DockingExamples
{
   [System.ComponentModel.ToolboxItem(false)]
   public partial class ExampleWorkerThread : Docking.Components.Component
   {
      public ExampleWorkerThread()
      {
         this.Build();
         this.Name = "Threading";
         myThreadHeader = "Test" + instances++.ToString();
         progressbar1.Adjustment = new Gtk.Adjustment(0, 0, 100, 1, 1, 10);
         progressbar1.Adjustment.Lower = 0;
         progressbar1.Adjustment.Upper = 100;
      }

      bool m_Destroyed = false; // todo: should be replaced by an widget property, but which ?
      protected override void OnDestroyed()
      {
         m_Destroyed = true;
         base.OnDestroyed();
         RequestStop();
      }

      private List<CancellationTokenSource> cancelTokenList = new List<CancellationTokenSource>();

      private WorkerThread mWorkerThread = null;
      private WorkerThread mEndlessThread = null;
      private Object mWorkerThreadSemaphore = new object();
      private Object mEndlessThreadSemaphore = new object();

      String myThreadHeader;
      static int instances = 0;
      int myThreadId = 0;
      int myTaskId = 0;
      int countTasks = 0;
      static Random rnd = new Random();

      void StartNewThread()
      {
         if (ComponentManager.PowerDown || m_Destroyed)
            return;

         buttonStartThread.Sensitive = false;
         lock (mWorkerThreadSemaphore)
         {
            myThreadId++;
            String name = String.Format("{0}:{1}", myThreadHeader, myThreadId);
            String description = "Example how to use WorkerThread";
            Message(String.Format("Thread {0} started", name));
            mWorkerThread = new WorkerThread(name, description);
            mWorkerThread.WorkerSupportsCancellation = true;
            mWorkerThread.WorkerReportsProgress = true;
            mWorkerThread.DoWork += new DoWorkEventHandler(mWorkerThread_DoWork);
            mWorkerThread.ProgressChanged += new ProgressChangedEventHandler(mWorkerThread_ProgressChanged);
            mWorkerThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mWorkerThread_RunWorkerCompleted);
            mWorkerThread.RunWorkerAsync(ThreadPriority.BelowNormal);
         }
      }

      void StartEndlessThread()
      {
         if (ComponentManager.PowerDown || m_Destroyed)
            return;
         buttonEndlessStart.Sensitive = false;
         buttonEndlessStop.Sensitive = true;

         lock (mEndlessThreadSemaphore)
         {
            myThreadId++;
            String name = String.Format("Endless {0}:{1}", myThreadHeader, myThreadId);
            String description = "Endless WorkerThread";
            Message(String.Format("Thread {0} started", name));
            mEndlessThread = new WorkerThread(name, description);
            mEndlessThread.WorkerSupportsCancellation = true;
            mEndlessThread.WorkerReportsProgress = false;
            mEndlessThread.DoWork += new DoWorkEventHandler(mThreadEndless_DoWork);
            mEndlessThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mThreadEndless_RunWorkerCompleted);
            mEndlessThread.RunWorkerAsync(ThreadPriority.BelowNormal);
         }
      }


      public void RequestStop()
      {
         lock (mWorkerThreadSemaphore)
         {
            if (mWorkerThread != null)
               mWorkerThread.CancelAsync();
            foreach (CancellationTokenSource t in cancelTokenList)
               t.Cancel();
         }
         lock (mEndlessThreadSemaphore)
         {
            if (mEndlessThread != null)
               mEndlessThread.CancelAsync();
         }
      }

      // complete message
      private void mWorkerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         lock (mWorkerThreadSemaphore)
         {
            Message(String.Format("Thread {0}:{1} {2}",
                                     myThreadHeader, myThreadId,
                                     e.Cancelled ? "Canceled" : "completed"));
            mWorkerThread = null;
         }
         buttonStartThread.Sensitive = true;
      }

      // progress message
      private void mWorkerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         Gtk.Application.Invoke(delegate
         {
            if (!ComponentManager.PowerDown && !m_Destroyed)
               progressbar1.Adjustment.Value = e.ProgressPercentage;
         });
      }

      private void Message(String message)
      {
         if (ComponentManager.PowerDown)
            return;

         Gtk.Application.Invoke(delegate
         {
            ComponentManager.MessageWriteLine(message);
         });
      }

      private void mWorkerThread_DoWork(object sender, DoWorkEventArgs e)
      {
         WorkerThread worker = sender as WorkerThread;

         int duration = rnd.Next() % 10000 + 10000;
         int steps = 100;
         int onesleep = duration / steps;
         int proceeded = 0;
         while (proceeded < duration)
         {
            proceeded += onesleep;
            Thread.Sleep(onesleep);

            worker.ReportProgress(proceeded * 100 / duration);

            if (worker.CancellationPending)
            {
               e.Cancel = true;
               return;
            }
         }
      }

      // complete message
      private void mThreadEndless_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         lock (mEndlessThreadSemaphore)
         {
            Message(String.Format("Thread {0}:{1} {2}",
                                  myThreadHeader, myThreadId,
                                  e.Cancelled ? "Canceled" : "completed"));
            mEndlessThread = null;
         }
         buttonEndlessStart.Sensitive = true;
         buttonEndlessStop.Sensitive = false;
      }

      private void mThreadEndless_DoWork(object sender, DoWorkEventArgs e)
      {
         WorkerThread worker = sender as WorkerThread;

         while (true)
         {
            Thread.Sleep(50);

            if (worker.CancellationPending)
            {
               e.Cancel = true;
               return;
            }
         }
      }

      public override void Save()
      {
         base.Save();

         RequestStop(); // TODO this is the wrong place! RequestStop() should be called when the component is about to be CLOSED, not when SAVED!
      }

      protected void OnButton1Clicked(object sender, EventArgs e)
      {
         // start a new task with Task.Factory
         // this is a very common method to work on something in the background
         // use TaskInformation to observe this new task

         CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
         lock (cancelTokenList)
            cancelTokenList.Add(cancelTokenSource);

         Task.Factory.StartNew(() =>
         {
            myTaskId++;
            String name = String.Format("{0}:{1}", myThreadHeader, myTaskId);
            String description = "Example how to use a Task";
            Message(String.Format("Task {0} started", name));

            Gtk.Application.Invoke(delegate
            {
               labelTaskCount.Text = String.Format("Running count: {0}", ++countTasks);
            });
            TaskInformation ti = TaskInformation.Create(cancelTokenSource, name, description);

            int duration = rnd.Next() % 5000 + 5000;
            int steps = 100;
            int onesleep = duration / steps;
            int proceeded = 0;
            while (proceeded < duration)
            {
               proceeded += onesleep;
               Thread.Sleep(onesleep);
               ti.Progress = proceeded * 100 / duration;
               if (cancelTokenSource.IsCancellationRequested)
                  break;
            }
            Message(String.Format("Task {0} {1}", name,
                    cancelTokenSource.IsCancellationRequested ? "cancelled" : "finished"));
            ti.Destroy();
            Gtk.Application.Invoke(delegate
            {
               labelTaskCount.Text = String.Format("Running count: {0}", --countTasks);
            });
            lock (cancelTokenList)
               cancelTokenList.Remove(cancelTokenSource);
         }, cancelTokenSource.Token);
      }


      protected void OnButtonStartThreadClicked(object sender, EventArgs e)
      {
         if (mWorkerThread == null)
            StartNewThread();
      }

      protected void OnButtonEndlessStartClicked(object sender, EventArgs e)
      {
         if (mEndlessThread == null)
            StartEndlessThread();
      }

      protected void OnButtonEndlessStopClicked(object sender, EventArgs e)
      {
         if (mEndlessThread != null)
            mEndlessThread.CancelAsync();
      }
   }

   #region Starter / Entry Point

   public class ExampleWorkerThreadFactory : ComponentFactory
   {
      public override Type TypeOfInstance { get { return typeof(ExampleWorkerThread); } }
      public override String MenuPath { get { return @"View\Examples\WorkerThread"; } }
      public override String Comment { get { return "Example for using worker threads"; } }
      public override Mode Options { get { return Mode.MultiInstance; } }
      public override Gdk.Pixbuf Icon { get { return ResourceLoader_DockingExamples.LoadPixbuf("WorkerThread-16.png"); } }
      public override string LicenseGroup { get { return "examples"; } }
   }

   #endregion
}

