using System;
using Docking.Components;
using Docking;
using Gtk;
using System.Collections.Generic;

namespace Examples.TestToolAndStatusBar
{
    [System.ComponentModel.ToolboxItem(false)]
    public partial class TestToolBarStatusBar : Component, IComponent
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


        #region IComponent
        public ComponentManager ComponentManager { get; set; }

        void IComponent.Loaded(DockItem item)
        {}

        void IComponent.Save()
        {}

        bool IComponent.Closed() { return true; }

        #endregion

        #region Component - Interaction

        public override void ComponentAdded(object item)
        {
            if (item is IProperty)
                mPropertyInterface = item as IProperty;
        }

        public override void ComponentRemoved(object item)
        {
            if (item == mPropertyInterface)
                mPropertyInterface = null;
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

        public override void FocusChanged(object item)
        {
            if (this == item && mPropertyInterface != null)
                mPropertyInterface.SetObject(this);
        }

        #endregion

        #region variables, properties
        IProperty mPropertyInterface;
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
        public override Mode Options { get { return Mode.CloseOnHide; } }
        public override Gdk.Pixbuf Icon { get { return Gdk.Pixbuf.LoadFromResource("Examples.Resources.TestToolBarStatusBar-16.png"); } }
    }

    #endregion

}

