using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MSSQLServerAuditor.Preprocessor;

namespace MSSQLServerAuditor.Table
{
    using MSSQLServerAuditor.Table.Settings;
    using MSSQLServerAuditor.Utils;

    /// <summary>
    /// User table.
    /// </summary>
    public partial class TableControl : UserControl
    {
        private const string MenuRestore = "menuRestore";
        private const string MenuHideAll = "menuHideAll";
        private const string MenuHide = "menuHide";
        private const string MenuSelect = "menuSelect";
        private const string MenuAll = "menuAll";


        private string _id;
        private string _propopcessorType;
        private TableStateSettings _stateSettings = new TableStateSettings();

        /// <summary>
        /// Initializing object TableControl.
        /// </summary>
        public TableControl()
        {
            InitializeComponent();
        }

        internal void SetId(string id, string preprocessorType)
        {
            _id = id;
            _propopcessorType = preprocessorType;

            _stateSettings = Program.Model.LayoutSettings.GetExtendedSettings<TableStateSettings>(
                _id, preprocessorType, Program.Model.Settings.ReportLanguage);

            if (_stateSettings == null)
                _stateSettings = new TableStateSettings();
        }

        /// <summary>
        /// Get configuration from string and draw graph
        /// </summary>
        /// <param name="text">XML configuration represented as string</param>
        /// <param name="ownerSize">Owner size.</param>
        /// <param name="context">Configuration context.</param>
        public void SetConfigurationFromText(string text, Size ownerSize, object context)
        {
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)))
            {
                stream.Seek(0, SeekOrigin.Begin);
                SetConfiguration(TableConfiguration.LoadFromXml(stream), ownerSize, context);
            }
        }

        /// <summary>
        /// Load configuration from XML file and draw graph
        /// </summary>
        /// <param name="fileName">XML configuration filename</param>
        /// <param name="ownerSize">Owner size.</param>
        /// <param name="context">Configuration context.</param>
        public void SetConfiguration(string fileName, Size ownerSize, object context)
        {
            SetConfiguration(TableConfiguration.LoadFromXml(fileName), ownerSize, context);
        }

        /// <summary>
        /// Load configuration from XML in stream and draw graph
        /// </summary>
        /// <param name="stream">Xml configuration stream</param>
        /// <param name="ownerSize">Owner size.</param>
        /// <param name="context">Configuration context.</param>
        public void SetConfiguration(Stream stream, Size ownerSize, object context)
        {
            SetConfiguration(TableConfiguration.LoadFromXml(stream), ownerSize, context);
        }

        private List<Dictionary<ColumnDefinition, object>> _plainData;

        /// <summary>
        /// Sets grid paras (column) Draw graph according passed configuration
        /// </summary>
        /// <param name="configuration">Graph configuration</param>
        /// <param name="ownerSize">Owner size.</param>
        /// <param name="context">Configuration context.</param>
        public void SetConfiguration(TableConfiguration configuration, Size ownerSize, object context)
        {
            var dataSource = new DataTable();

            dataGridView.AutoGenerateColumns = false;

            _plainData = configuration.TableSource.GetPlainData(context);
            dataGridView.ColumnCount = configuration.ColumnDefinitions.Count;

            if (configuration.UseAutoSize)
            {
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            }
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            for (int i = 0; i < configuration.ColumnDefinitions.Count; i++)
            {
                var column = dataGridView.Columns[i];
                var definition = configuration.ColumnDefinitions[i];

                column.Tag = definition;
                column.DefaultCellStyle.Format = definition.Format;
                column.HeaderText = definition.ColumnName;
                column.DataPropertyName = definition.ColumnName;

                column.DefaultCellStyle.Alignment = definition.Align.GetColumnAlign();
                column.DefaultCellStyle.Font = new Font(string.Empty, Convert.ToInt32(definition.FontSize ?? "8"), definition.GetFontStyle());

                column.HeaderCell.Style.Alignment = definition.HeaderAlign.GetColumnAlign();
                column.HeaderCell.Style.Font = new Font(
                    string.Empty,
                    Convert.ToInt32(definition.HeaderFontSize ?? "8"),
                    definition.GetHeaderFontStyle());

                var columnConfig = _stateSettings.GetColumnSettings(definition.Tag);
                if (!configuration.UseAutoSize)
                {
                    column.Width = columnConfig.Width;
                }

                column.Visible = columnConfig.Visible;

                dataSource.Columns.Add(definition.ColumnName, definition.Type.GetClrType());
            }

            foreach (var rows in _plainData)
            {
                dataSource.LoadDataRow(rows.Values.ToArray(), true);
            }

            dataGridView.DataSource = dataSource.DefaultView;
        }

        string GetLocalizedText(string name)
        {
            if (Program.Model != null)
                if (Program.Model.LocaleManager != null)
                    return Program.Model.LocaleManager.GetLocalizedText(GetType().Name, name);
            return name;
        }

        private void DataGridViewColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ShowContextMenu(e.X, e.Y, e.ColumnIndex, e.RowIndex);
            }
        }

        private void ShowContextMenu(int x, int y, int? columnIndex, int? rowIndex)
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            var hideColumns = (from DataGridViewColumn c in dataGridView.Columns
                               where !c.Visible
                               select c).ToList();
            if (hideColumns.Count > 0)
            {
                ToolStripMenuItem restoreItems =
                    new ToolStripMenuItem(GetLocalizedText(MenuRestore));
                contextMenu.Items.Add(restoreItems);
                foreach (var series in hideColumns)
                {
                    var item = restoreItems.DropDownItems.Add(series.HeaderText);
                    item.Click += (o, args) =>
                        {
                            SetColumnVisible(series, true);
                        };
                }
                if (hideColumns.Count > 1)
                {
                    restoreItems.DropDownItems.Add(new ToolStripSeparator());
                    restoreItems.DropDownItems.Add(GetLocalizedText(MenuAll)).Click +=
                        (o, args) =>
                            {
                                foreach (DataGridViewColumn s in hideColumns)
                                    SetColumnVisible(s, true);
                            };
                }
            }
            if (hideColumns.Count != dataGridView.Columns.Count)
            {
                ToolStripMenuItem hideAllItems =
                    new ToolStripMenuItem(GetLocalizedText(MenuHideAll));
                hideAllItems.Click +=
                    (sender2, args) => { foreach (DataGridViewColumn s in dataGridView.Columns)
                        SetColumnVisible(s, false); };
                contextMenu.Items.Add(hideAllItems);
            }

            if (columnIndex != null)
            {
                if (contextMenu.Items.Count > 0)
                    contextMenu.Items.Add(new ToolStripSeparator());
                contextMenu.Items.Add(GetLocalizedText(MenuHide)).Click +=
                    (o, args) => SetColumnVisible(dataGridView.Columns[columnIndex.Value], false);
            }

            Rectangle d = new Rectangle(0,0,0,0);
            if ((columnIndex != null) && (rowIndex != null))
            d = dataGridView.GetCellDisplayRectangle(columnIndex.Value, rowIndex.Value, false);
            Point location = new Point(d.Left + x, d.Top + y);
            contextMenu.Show(dataGridView, location);
        }

        private void DataGridViewColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            var colDef = (ColumnDefinition)e.Column.Tag;
            _stateSettings.GetColumnSettings(colDef.Tag).Width = e.Column.Width;
            Program.Model.LayoutSettings.SetExtendedSettings(
                _id, _propopcessorType, Program.Model.Settings.ReportLanguage,
                _stateSettings);
        }

        private void SetColumnVisible(DataGridViewColumn column, bool visible)
        {
            column.Visible = visible;
            _stateSettings.GetColumnSettings(((ColumnDefinition)column.Tag).Tag).Visible = visible;
            Program.Model.LayoutSettings.SetExtendedSettings(
                _id, _propopcessorType, Program.Model.Settings.ReportLanguage,
                _stateSettings);
        }

        private void dataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                ShowContextMenu(e.X, e.Y, null, null);
        }
    }
}
