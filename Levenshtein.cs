using System;
using Docking.Components;
using Gtk;
using System.Collections.Generic;
using Docking;

namespace Examples
{
    [System.ComponentModel.ToolboxItem(false)]
    public partial class Levenshtein : Component
    {
        public Levenshtein()
        {
            this.Build();
            this.Name = "Levenshtein"; 
            m_Buffer = textview.Buffer;
            m_Buffer.Text = "In information theory and computer science, the Levenshtein distance is a string metric for measuring the difference between two sequences. Informally, the Levenshtein distance between two words is the minimum number of single-character edits required to change one word into the other. The phrase edit distance is often used to refer specifically to Levenshtein distance. The Levenshtein distance between two strings is defined as the minimum number of edits needed to transform one string into the other, with the allowable edit operations being insertion, deletion, or substitution of a single character. It is named after Vladimir Levenshtein, who considered this distance in 1965. It is closely related to pairwise string alignments."
                + "\n\nFor example, the Levenshtein distance between \"kitten\" and \"sitting\" is 3, since the following three edits change one into the other, and there is no way to do it with fewer than three edits:"
                    + "\n\nkitten → sitten (substitution of \"s\" for \"k\")"
                    + "\nsitten → sittin (substitution of \"i\" for \"e\")"
                    + "\nsittin → sitting (insertion of \"g\" at the end).";

            TextTag tag = new TextTag("bold");
            tag.Weight = Pango.Weight.Bold;
            tag.Background = "yellow";
            m_Buffer.TagTable .Add(tag);

            entryLine.Changed += (sender, e) => 
            {
                SearchAndMark(entryLine.Text);
            };

            m_Buffer.Changed += (sender, e) => 
            {
                TokenizeText();
                SearchAndMark(entryLine.Text);
            };

            TokenizeText();
            labelBestMatch.LabelProp = "- ";
        }

        public override void Loaded(DockItem item)
        {
            base.Loaded(item);

            // attach python
            m_ScriptingInstance = new LevenshteinScripting(this);
        }      

        public override object GetScriptingInstance()
        {
            return m_ScriptingInstance;
        }

        void SearchAndMark(string s)
        {
            Byte bestkey;
            string[] result = Search(s, out bestkey);
            if (result != null)
            {
                ResetMark();
                Mark(result);
                labelBestMatch.LabelProp = bestkey.ToString() + " ";
            }
            else
            {
                ResetMark();
                labelBestMatch.LabelProp = "- ";
            }
        }


        TextBuffer m_Buffer;
        String [] m_Token;
        LevenshteinScripting m_ScriptingInstance;


        void TokenizeText()
        {
            m_Token = textview.Buffer.Text.Split(new char[] {' ', '.', ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }

        void ResetMark()
        {
            m_Buffer.RemoveAllTags(m_Buffer.GetIterAtOffset(0), m_Buffer.GetIterAtOffset(int.MaxValue));
        }

        void Mark(string[]strings)
        {
            if (strings != null)
            {
                foreach(string s in strings)
                    MarkAll(s);
            }
        }

        string[] Search(String txt, out Byte distance)
        {
            return Docking.Tools.Levenshtein.Search(m_Token, txt, out distance);
        }



        void MarkAll(string exp)
        {
            TextIter start, end;
            start = m_Buffer.GetIterAtOffset(0);
            TextIter limit = m_Buffer.GetIterAtOffset(int.MaxValue);
            while(start.ForwardSearch(exp, TextSearchFlags.TextOnly, out start, out end, limit))
            {
                m_Buffer.ApplyTag("bold", start, end);
                start.Offset++;
            }
        }

        // encapsulate python access
        public class LevenshteinScripting
        {
            public LevenshteinScripting(Levenshtein l)
            {
                lev = l;
            }

            Levenshtein lev;

            public string[] Search(string s)
            {
                Byte bestkey;
                return lev.Search(s, out bestkey);
            }

            public void Mark(string[] strings)
            {
                lev.Mark(strings);
            }

            public void Mark(string s)
            {
                lev.Mark(new string[] { s });
            }

            public void ResetMark()
            {
                lev.ResetMark();
            }
        }
    }

    #region Starter / Entry Point
    
    public class LevenshteinFactory : ComponentFactory
    {
        public override Type TypeOfInstance { get { return typeof(Levenshtein); } }
        public override String MenuPath { get { return @"View\Examples\Levenshtein"; } }
        public override String Comment { get { return "Levenshtein example"; } }
        public override Gdk.Pixbuf Icon { get { return Gdk.Pixbuf.LoadFromResource("Examples.Resources.Example-16.png"); } }
    }
    
    #endregion

}

