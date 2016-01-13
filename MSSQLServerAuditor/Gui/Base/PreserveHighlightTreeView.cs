using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MSSQLServerAuditor.Model;

namespace MSSQLServerAuditor.Gui.Base
{
	/// <summary>
	/// Tree that can preserve highlight of selected node on focus loss
	/// </summary>
	public class PreserveHighlightTreeView : TreeView
	{
		// Space between Image and Label.
		private const int SPACE_IL             = 3;
		private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
		private const int TVS_EX_DOUBLEBUFFER  = 0x0004;

		private TreeNode _currentNode;

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		[Display]
		public Image ActiveScheduleOverlayImage { get; set; }

		/// <summary>
		/// Occurs in case of choice change.
		/// </summary>
		/// <param name="e">TreeViewEventArgs.</param>
		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			if (this._currentNode != null)
			{
				this._currentNode.BackColor = BackColor;

				//this._currentNode.ForeColor = ForeColor;
			}

			this._currentNode = e.Node;

			base.OnAfterSelect(e);
		}

		/// <summary>
		/// Occurs in case of node deploymentю
		/// </summary>
		/// <param name="e">TreeViewCancelEventArgs</param>
		protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
			e.Node.BackColor = Color.FromKnownColor(KnownColor.Highlight);

			// e.Node.ForeColor = Color.FromKnownColor(KnownColor.HighlightText);

			base.OnBeforeSelect(e);
		}

		/// <summary>
		/// Occurs in case of focus loss.
		/// </summary>
		/// <param name="e">EventArgs.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			if (this._currentNode != null)
			{
				this._currentNode.BackColor = Color.FromKnownColor(KnownColor.Highlight);

				//this._currentNode.ForeColor = Color.FromKnownColor(KnownColor.HighlightText);
			}

			base.OnLostFocus(e);
		}

		/// <summary>
		/// Handled created.
		/// </summary>
		/// <param name="e">EventArgs</param>
		protected override void OnHandleCreated(EventArgs e)
		{
			SendMessage(
				Handle,
				TVM_SETEXTENDEDSTYLE,
				(IntPtr)TVS_EX_DOUBLEBUFFER,
				(IntPtr)TVS_EX_DOUBLEBUFFER
			);

			base.OnHandleCreated(e);
		}

		/// <summary>
		/// Occurs in case of draw node.
		/// </summary>
		/// <param name="e">DrawTreeNodeEventArgs</param>
		protected override void OnDrawNode(DrawTreeNodeEventArgs e)
		{
			// get node font and node fore color
			Font  nodeFont      = GetTreeNodeFont(e.Node);
			Color nodeForeColor = GetTreeNodeForeColor(e.Node);

			// draw node text
			TextRenderer.DrawText(
				e.Graphics,
				e.Node.Text,
				nodeFont,
				e.Bounds,
				nodeForeColor,
				TextFormatFlags.Left | TextFormatFlags.Top
			);

			// border around text not use
			if (e.Node.IsSelected)
			{
				using (Pen pen = new Pen(Color.Black))
				{
					pen.DashStyle = DashStyle.Solid;
					pen.Width     = 1;

					Rectangle penBounds = e.Bounds;

					penBounds.Width  -= 2;
					penBounds.Height -= 1;
					penBounds.Offset(0, -1);

					e.Graphics.DrawRectangle(pen, penBounds);
				}
			}

			DrawOverlayIcon(e);

			base.OnDrawNode(e);
		}

		private void DrawOverlayIcon(DrawTreeNodeEventArgs e)
		{
			if (ActiveScheduleOverlayImage == null)
			{
				return;
			}

			ConcreteTemplateNodeDefinition definition = e.Node.Tag as ConcreteTemplateNodeDefinition;

			if (definition != null && definition.NodeActivated)
			{
				TemplateNodeInfo nodeDef = definition.TemplateNode;

				// Draw overlay icon if the node has active jobs
				if (nodeDef != null && nodeDef.HasActiveJobs)
				{
					Image overlayImage = ActiveScheduleOverlayImage;
					int   nodeHeight   = e.Node.Bounds.Height;

					// Overlay image size and coordinates
					int imgSize = nodeHeight / 2;
					int imgLeft = e.Node.Bounds.Left - SPACE_IL - imgSize;
					int imgTop  = e.Node.Bounds.Bottom - imgSize;

					// draw overlay icon
					e.Graphics.DrawImage(
						overlayImage,
						imgLeft,
						imgTop,
						imgSize,
						imgSize
					);
				}
			}
		}

		private Font GetTreeNodeFont(TreeNode node)
		{
			Font nodeFont = node.NodeFont ?? this.Font;

			return nodeFont;
		}

		private Color GetTreeNodeForeColor(TreeNode node)
		{
			Color nodeForeColor;

			try
			{
				ConcreteTemplateNodeDefinition tags = node.Tag as ConcreteTemplateNodeDefinition;

				if ((tags == null) || (tags.NodeActivated))
				{
					nodeForeColor = node.ForeColor;
				}
				else
				{
					nodeForeColor = Color.LightGray;
				}
			}
			catch
			{
				nodeForeColor = node.ForeColor;
			}

			return nodeForeColor;
		}
	}
}
