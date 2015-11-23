using System;
using System.IO;
using System.Collections.Generic;
using Docking.Components;
using Docking;
using Gtk;
using Docking.Tools;

namespace DockingExamples
{
    [System.ComponentModel.ToolboxItem(false)]
    public partial class TextViewer : Component, IFileOpen
    {       
        #region IFileOpen

        List<FileFilterExt> IFileOpen.SupportedFileTypes()
        {
           return new List<FileFilterExt>() { ExampleTextViewerFactory.sFileFilter_TXT };
        }

        bool IFileOpen.CanOpenFile(String filename)
        {
           return (ExampleTextViewerFactory.sFileFilter_TXT.Matches(filename) || !File.Exists(filename));
        }
        
        bool IFileOpen.OpenFile(String filename)
        {
           if (!ExampleTextViewerFactory.sFileFilter_TXT.Matches(filename) || !File.Exists(filename))
                return false;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(filename))
            {
                if(reader==null)
                   return false;

               string txt = reader.ReadToEnd();
               textview.Buffer.Clear();
               textview.Buffer.InsertAtCursor(txt);

               return true;
            }
        }

        #endregion

        #region MAIN
        public TextViewer()
        {
            this.Build();
        }
        #endregion
    }

    public class ExampleTextViewerFactory : ComponentFactory
    {
        static public FileFilterExt sFileFilter_TXT = new FileFilterExt("*.txt", "Text File");

        public override Type TypeOfInstance { get { return typeof(TextViewer); } }
        public override String MenuPath { get { return @"View\Examples\TextViewer"; } }
        public override String Comment { get { return "Load *.txt files"; } }

        public override List<FileFilterExt> SupportedFileTypes { get { return new List<FileFilterExt>() { sFileFilter_TXT }; } }

        public override Gdk.Pixbuf Icon { get { return ResourceLoader_DockingExamples.LoadPixbuf("Example-16.png"); } }
        public override string LicenseGroup { get { return "examples"; } }
    }

}

