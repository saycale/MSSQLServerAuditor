using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Utils
{
    /// <summary>
    /// Represents a Window System Menu
    /// </summary>
    public class SystemMenu
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);

        private readonly List<SystemMenuItem> _items;
        private readonly Form _form;
        private readonly IntPtr _systemMenuHandle;

        /// <summary>
        /// Window's system menu Windows API handle
        /// </summary>
        public IntPtr Handle
        {
            get { return _systemMenuHandle; }
        }

        /// <summary>
        /// Creates a new instance of SystemMenu class
        /// </summary>
        public SystemMenu(Form form)
        {
            _items = new List<SystemMenuItem>();
            _form = form;
            _systemMenuHandle = GetSystemMenu(_form.Handle, false);
        }

        /// <summary>
        /// Creates a new instance of SystemMenuItem and appends it to menu
        /// </summary>
        /// <param name="text">Text to be displayed as menu item caption</param>
        /// <param name="clicked">Menu item Clicked event handler</param>
        /// <returns>Just created menu item</returns>
        public SystemMenuItem AppendNewItem(string text, EventHandler clicked = null)
        {
            var item = new SystemMenuItem(text, this);
            if (clicked != null)
            {
                item.Click += clicked;
            }

            AppendMenu(_systemMenuHandle, item.Flags, item.MenuId, text);
            _items.Add(item);
            return item;
        }

        /// <summary>
        /// Creates menu sepatator item as instance of SystemMenuItem class and appeds it to menu
        /// </summary>
        /// <returns>Just created menu item</returns>
        public SystemMenuItem AppendSeparator()
        {
            return AppendNewItem("-");
        }
    }

    /// <summary>
    /// Represents a windws system menu item
    /// </summary>
    public class SystemMenuItem: MenuItem
    {
        /// <summary>
        /// ReSharper disable InconsistentNaming
        /// </summary>
        public const int SEPERATOR_FLAG = 0x800;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int ModifyMenu(IntPtr hMenu, int pos, int flags, int newPos, string text);

        private readonly int _flags;
        private readonly SystemMenu _menu;

        internal int Flags
        {
            get { return _flags; }
        }

        internal int MenuId
        {
            get { return MenuID; }
        }

        internal SystemMenuItem(string text, SystemMenu menu)
        {
            base.Text = text;
            _menu = menu;
            if (text == "-")
                _flags = SEPERATOR_FLAG;
        }

        /// <summary>
        /// Menu item caption
        /// </summary>
        new public string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                ModifyMenu(_menu.Handle, MenuId, _flags, MenuId, base.Text);
            }
        }
    }
}