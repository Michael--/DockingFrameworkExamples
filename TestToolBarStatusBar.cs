using System;
using Docking.Components;
using Docking;
using Gtk;
using System.Collections.Generic;

namespace DockingExamples
{
    [System.ComponentModel.ToolboxItem(false)]
    public partial class TestToolBarStatusBar : Component
    {
        public TestToolBarStatusBar ()
        {
            this.Build ();
            this.Name = "Tool-/Status-Bar";
            InitToolbarButtons();
        }

        void InitToolbarButtons()
        {
            mPush = new ToolButton("Push");
            mPush.Label = "Push";
            mPush.StockId = Stock.Add;
            mPush.TooltipText = "Push a new message to status bar";
            mPush.Clicked += (sender, e) =>
            {
                String text = String.Format("Hello {0} at {1}", ++mTextCounter, DateTime.Now.ToLongTimeString());
                uint id = ComponentManager.PushStatusbar(text);
                mStack.Push(id);
                UpdateMessageText();
            };

            mPop = new ToolButton("Pop");
            mPop.Label = "Pop";
            mPop.StockId = Stock.Remove;
            mPop.TooltipText = "Pop newest message from status bar";
            mPop.Clicked += (sender, e) =>
            {
                if (mStack.Count > 0)
                    ComponentManager.PopStatusbar(mStack.Pop ());
                UpdateMessageText();
            };
        }

        void UpdateMessageText()
        {
            label3.Text = String.Format ("Messages pushed to status bar: {0}", mStack.Count);
        }

        #region Component - Interaction

        public override void ComponentAdded(object item)
        {
            base.ComponentAdded(item);
        }

        public override void ComponentRemoved(object item)
        {
           base.ComponentRemoved(item);
        }

        public override void VisibilityChanged(object item, bool visible)
        {
            if (visible && !mAdded)
            {
                ComponentManager.AddToolItem (mPush);
                ComponentManager.AddToolItem (mPop);
                mAdded = true;
            }
            else if (mAdded)
            {
                ComponentManager.RemoveToolItem (mPush);
                ComponentManager.RemoveToolItem (mPop);
                while (mStack.Count > 0)
                    ComponentManager.PopStatusbar(mStack.Pop ());
                mAdded = false;
            }
            UpdateMessageText();
        }


        #endregion

        #region variables, properties
        ToolButton mPush;
        ToolButton mPop;
        bool mAdded = false;
        int mTextCounter = 0;
        Stack<uint> mStack = new Stack<uint>();
        #endregion
    }

    #region Starter / Entry Point

    public class TestToolBarStatusBarFactory : ComponentFactory
    {
        public override Type TypeOfInstance { get { return typeof(TestToolBarStatusBar); } }
        public override String MenuPath { get { return @"View\Examples\Test Toolbar and Statusbar"; } }
        public override String Comment { get { return "Example using toolbar and status bar"; } }
        public override Gdk.Pixbuf Icon { get { return ResourceLoader_DockingExamples.LoadPixbuf("TestToolBarStatusBar-16.png"); } }
        public override string LicenseGroup { get { return "examples"; } }
    }

    #endregion

}

