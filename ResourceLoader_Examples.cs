using System;

namespace DockingExamples
{
   // a copy of class ResourceLoader_Docking - see there! It is necessary to have a copy in each assembly SEPARATELY!
   public class ResourceLoader_DockingExamples
   {
      private static string RESOURCE_PREFIX = "DockingExamples.Resources."; // this is added here explicitly to PREVENT that resources from other assemblies get loaded! (that would lead to random crashes...)

      // a pink 16x16 dummy placeholder PNG for resources that cannot be retrieved
      private static byte[] DUMMY_PLACEHOLDER_IMAGE = System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAAK3RFWHRDcmVhdGlvbiBUaW1lAFNhIDI3IEp1biAyMDE1IDE0OjQ5OjM2ICswMTAwswJnxwAAAAd0SU1FB98GGww0ExFqaikAAAAJcEhZcwAACxIAAAsSAdLdfvwAAAAEZ0FNQQAAsY8L/GEFAAAABlBMVEUAAAD/AP82/WKvAAAADklEQVR42mP4/5+BFAQA/U4f4d7IdZcAAAAASUVORK5CYII=");

      public static Gdk.Pixbuf LoadPixbuf(string resourcename)
      {
         if(!String.IsNullOrEmpty(resourcename))
         {
            string fullresourcename = RESOURCE_PREFIX+resourcename;
            try { return Gdk.Pixbuf.LoadFromResource(fullresourcename); }
            #if DEBUG
            catch(Exception e) { System.Console.Error.WriteLine(e.ToString()); }   
            #else
            catch(Exception) {}
            #endif
         }        

         // return dummy placeholder image here instead of null to avoid program crashes!
         // return a new one each time to make all instances independent!
         return new Gdk.Pixbuf(DUMMY_PLACEHOLDER_IMAGE);
         // NOTE: GtkSharp has a bug in this Gdk.Pixbuf(DUMMY_PLACEHOLDER_IMAGE) constructor:
         // after reading from that byte stream, it lacks the call to Gdk.PixbufLoader.Close(), thus, this produces lots of output:
         // "GdkPixbufLoader finalized without calling gdk_pixbuf_loader_close() - this is not allowed. You must explicitly end the data stream to the loader before dropping the last reference."
         // We already tried to workaround that by manually ensuring the close() call by this code
         //    Gdk.PixbufLoader loader = new Gdk.PixbufLoader();
         //    bool ok = loader.Write(DUMMY_PLACEHOLDER_IMAGE);
         //    Debug.Assert(ok);
         //    ok = loader.Close();
         //    Debug.Assert(ok);
         //    Gdk.Pixbuf pixbuf = loader.Pixbuf;        
         //    loader = null;
         //    return pixbuf;
         // , however, that did not help. We suspect that the loader.Close() function itself is broken inside and does not propagate its call to Gtk.
         // S.Lohse 2015-06-27
      }

      public static Gtk.Image LoadImage(string resourcename)
      {
         return new Gtk.Image(LoadPixbuf(resourcename));
      }
   }
}
