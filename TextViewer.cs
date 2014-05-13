using System;
using System.IO;
using System.Collections.Generic;
using Docking.Components;
using Docking;
using Gtk;
using Docking.Tools;

namespace Examples
{
    [System.ComponentModel.ToolboxItem(false)]
    public partial class TextViewer : Component, IFileOpen
    {       
        #region IFileOpen

        static FileFilterExt sFileFilter_TXT = new FileFilterExt("*.txt", "Text File");

        List<FileFilterExt> IFileOpen.SupportedFileTypes()
        {
           return new List<FileFilterExt>() { sFileFilter_TXT };
        }

        String IFileOpen.TryOpenFile(String filename)
        {
            if(!sFileFilter_TXT.Matches(filename) || !File.Exists(filename))
                return null;
            return sFileFilter_TXT.Name;
        }
        
        bool IFileOpen.OpenFile(String filename)
        {
            if(!sFileFilter_TXT.Matches(filename) || !File.Exists(filename))
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
        public override Type TypeOfInstance { get { return typeof(TextViewer); } }
        public override String MenuPath { get { return @"View\Examples\TextViewer"; } }
        public override String Comment { get { return "Load *.txt files"; } }
        public override Gdk.Pixbuf Icon { get { return Gdk.Pixbuf.LoadFromResource("Examples.Resources.Example-16.png"); } }
    }

}

