using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MSSQLServerAuditor.BusinessLogic.UserSettings;
using MSSQLServerAuditor.Model.Internationalization;
using MSSQLServerAuditor.Model.Settings;
using MSSQLServerAuditor.Model;
using MSSQLServerAuditor.SQLite.Tables.UserSettings;
using MSSQLServerAuditor.Utils;

namespace MSSQLServerAuditor.Gui
{
	public partial class frmUserSettingsNode : LocalizableForm
	{
		#region Private objects and Initializing
		private readonly TreeNode                       _node;
		private readonly ConcreteTemplateNodeDefinition _nodeDefinition;
		private readonly InstanceTemplate               _template;
		private readonly UserSettingsManager            _userSettingsManager;
		private readonly MsSqlAuditorModel              _model;

		private frmUserSettingsNode()
		{
			this._model               = null;
			this._node                = null;
			this._nodeDefinition      = null;
			this._template            = null;
			this._userSettingsManager = null;

			InitializeComponent();
		}

		public frmUserSettingsNode(MsSqlAuditorModel model, TreeNode node) : this()
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			this._node           = node;
			this._nodeDefinition = node.Tag as ConcreteTemplateNodeDefinition;

			if (this._nodeDefinition == null)
			{
				throw new ArgumentException("Template is not specified for the node.");
			}

			this._model               = model;
			this._template            = GetUserSettings();
			this._userSettingsManager = new UserSettingsManager(
				this._model.Settings.InterfaceLanguage
			);
		}
		#endregion

		#region Private methods
		private InstanceTemplate GetUserSettings()
		{
			InstanceTemplate match = this._model.TemplateSettings.UserSettings.FirstOrDefault(i =>
				i.TemplateName == this._nodeDefinition.Connection.TemplateFileName &&
				i.Connection.ParentKey == this._nodeDefinition.TemplateNode.IdsHierarchy
			);

			return match;
		}

		private string GetNodeName()
		{
			if (this._template != null)
			{
				ActivitySetting activitySetting = this._template.Connection.Activity;

				if (activitySetting.NewNameNode != null)
				{
					return activitySetting.NewNameNode;
				}

				TemplateNodeLocaleInfo lang = this.Language;

				if (lang != null)
				{
					return lang.Text;
				}
			}

			return this._nodeDefinition.TemplateNode.Title;
		}

		private TemplateNodeLocaleInfo Language
		{
			get
			{
				return this._nodeDefinition.TemplateNode.Locales.FirstOrDefault(
					it => it.Language == this._model.Settings.InterfaceLanguage
				);
			}
		}

		private bool IsIcon(string imageFile)
		{
			using (Bitmap img = new Bitmap(imageFile))
			{
				return img.Height == img.Width && img.Height == 16;
			}
		}

		private void UpdateChildNodes(TreeNode parentNode, bool disable)
		{
			foreach (TreeNode childNode in parentNode.Nodes)
			{
				ConcreteTemplateNodeDefinition childNodeDef = childNode.Tag as ConcreteTemplateNodeDefinition;

				if (childNodeDef != null)
				{
					TemplateNodeInfo templateInfo = childNodeDef.TemplateNode;

					childNodeDef.NodeActivated = !disable;
					templateInfo.IsDisabled    = disable;

					childNode.ForeColor = GetColor(templateInfo);

					UpdateChildNodes(childNode, disable);
				}
			}
		}

		private Color GetColor(TemplateNodeInfo templateInfo)
		{
			return string.IsNullOrEmpty(templateInfo.FontColor)
				? Color.Black
				: Colors.FromString(templateInfo.FontColor);
		}

		private void BindIconControls()
		{
			string imagesFolder = Path.Combine(
				Application.StartupPath,
				"Images"
			);

			string[] pngFiles = Directory.GetFiles(
				imagesFolder,
				"*.png",
				SearchOption.AllDirectories
			);

			IEnumerable<string> iconFiles = pngFiles.Where(IsIcon);

			foreach (string iconFile in iconFiles)
			{
				string file = iconFile;

				BindingWrapper<string> iconItemWrapper = new BindingWrapper<string>(
					iconFile, s => Path.GetFileName(file)
				);

				cmbNodeIcon.Items.Add(iconItemWrapper);

				string iconName = Path.GetFileNameWithoutExtension(file);

				if (this._node.ImageKey == iconName)
				{
					cmbNodeIcon.SelectedItem  = iconItemWrapper;
					picIconNode.ImageLocation = iconFile;
				}
			}
		}

		#endregion

		#region Events methods

		private void btnSelectFontColor_Click(object sender, EventArgs e)
		{
			colorDialog.Color    = panelColor.BackColor;
			colorDialog.FullOpen = true;

			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				txtNodeColorName.Text = colorDialog.Color.Name;
				panelColor.BackColor  = colorDialog.Color;
			}
		}

		private void frmUserSettingsNode_Load(object sender, EventArgs e)
		{
			TemplateNodeInfo nodeInfo = this._nodeDefinition.TemplateNode;
			string           nodeName = nodeInfo.Title;

			if (string.IsNullOrEmpty(nodeName))
			{
				nodeName = GetNodeName();
			}

			txtNodeName.Text = nodeName;

			TreeNode parentNode = this._node.Parent;
			if (parentNode != null)
			{
				ConcreteTemplateNodeDefinition parentDef = parentNode.Tag as ConcreteTemplateNodeDefinition;
				if (parentDef != null)
				{
					if (!parentDef.NodeActivated)
					{
						chkDeactivateNode.Enabled = false;
					}
				}
			}

			Text = GetLocalizedText("captionText") + " " + nodeName;

			chkDeactivateNode.Checked = !this._nodeDefinition.NodeActivated;

			BindIconControls();

			Color nodeColor       = GetColor(nodeInfo);
			panelColor.BackColor  = nodeColor;
			txtNodeColorName.Text = nodeColor.Name;
		}

		private void cmbNodeIcon_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindingWrapper<string> selectedIcon = cmbNodeIcon.SelectedItem as BindingWrapper<string>;

			if (selectedIcon != null)
			{
				picIconNode.ImageLocation = selectedIcon.Item;
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			BindingWrapper<string> iconItem = cmbNodeIcon.SelectedItem as BindingWrapper<string>;

			if (iconItem == null)
			{
				return;
			}

			string           iconFileName = Path.GetFileNameWithoutExtension(iconItem.Item);
			TemplateNodeInfo nodeInfo     = this._nodeDefinition.TemplateNode;
			string           nodeName     = nodeInfo.Title;

			if (string.IsNullOrEmpty(nodeName))
			{
				nodeName = GetNodeName();
			}

			string additionalText = this._node.Text.Length > nodeName.Length
				? this._node.Text.Substring(nodeName.Length)
				: "";

			this._node.Text             = txtNodeName.Text + additionalText;
			this._node.ImageKey         = iconFileName;
			this._node.SelectedImageKey = iconFileName;

			if (this._nodeDefinition.NodeAvailable)
			{
				this._node.ForeColor = panelColor.BackColor;
			}

			this._nodeDefinition.NodeActivated = !chkDeactivateNode.Checked;

			if (this._nodeDefinition.TemplateNode.Childs.Count > 0)
			{
				bool disableChild = chkDeactivateNode.Checked;
				UpdateChildNodes(this._node, disableChild);
			}

			string nodeUiName = this.txtNodeName.Text;

			TemplateNodeLocaleInfo locale = nodeInfo.Locales.FirstOrDefault(
				l => l.Language == this._model.Settings.InterfaceLanguage
			);

			if (locale != null)
			{
				if (locale.Text == nodeUiName)
				{
					nodeUiName = string.Empty;
				}
			}

			nodeInfo.FontColor  = this.panelColor.BackColor.Name;
			nodeInfo.UIcon      = iconFileName;
			nodeInfo.Title      = nodeUiName;
			nodeInfo.IsDisabled = this.chkDeactivateNode.Checked;

			this._userSettingsManager.SaveUserSettings(
				new UserSettingsRow {
					TemplateNodeId = nodeInfo.TemplateNodeId.GetValueOrDefault(),
					NodeEnabled    = !this.chkDeactivateNode.Checked,
					NodeFontColor  = this.panelColor.BackColor.Name,
					NodeUIIcon     = iconFileName,
					NodeUIName     = nodeUiName
				}
			);

			this._model.GetVaultProcessor(this._nodeDefinition.Connection)
				.CurrentStorage.Save(nodeInfo);
		}

		private void btnDefaultName_Click(object sender, EventArgs e)
		{
			TemplateNodeInfo templateNodeInfo = this._nodeDefinition.TemplateNode;

			this._userSettingsManager.SaveUserSettings(
				new UserSettingsRow {
					TemplateNodeId = templateNodeInfo.TemplateNodeId.GetValueOrDefault(),
					NodeUIName     = string.Empty,
				}
			);

			TemplateNodeLocaleInfo locale = templateNodeInfo.Locales.FirstOrDefault(
				l => l.Language == this._model.Settings.InterfaceLanguage
			);

			if (locale != null)
			{
				this.txtNodeName.Text = locale.Text;
			}
		}
		#endregion
	}
}
