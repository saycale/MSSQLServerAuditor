using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui.Base
{
	public class TabPageBuffer
	{
		private readonly List<TabPage> _pages;

		public TabPageBuffer()
		{
			this._pages = new List<TabPage>();
		}

		public void Resize(int size)
		{
			int count = this._pages.Count;

			if (count > size)
			{
				// shrink the buffer
				for (int i = count - 1; i >= size; i--)
				{
					TabPage oldPage = this._pages[i];

					this._pages.Remove(oldPage);

					DisposeTabPage(oldPage);
				}
			}
			else if (count < size)
			{
				// extend the buffer
				int pagesToAdd = size - count;

				AddPages(pagesToAdd);
			}
		}

		public TabPage GetPage(int index)
		{
			int count = this._pages.Count;

			if (index >= count)
			{
				int pagesToAdd = index - count + 1;

				AddPages(pagesToAdd);
			}

			return this._pages[index];
		}

		private void AddPages(int pagesToAdd)
		{
			for (int i = 0; i < pagesToAdd; i++)
			{
				AddNewPage();
			}
		}

		private void AddNewPage()
		{
			TabPage newTab = new TabPage
			{
				Margin  = new Padding(0),
				Padding = new Padding(0)
			};

			this._pages.Add(newTab);
		}

		private void DisposeTabPage(TabPage tabPage)
		{
			tabPage.DisposeChildControls();
			tabPage.Dispose();
		}
	}
}
