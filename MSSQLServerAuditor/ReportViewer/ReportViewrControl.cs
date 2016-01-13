using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using MSSQLServerAuditor.Model;


namespace MSSQLServerAuditor.ReportViewer
{
    /// <summary>
    /// RDLC report.
    /// </summary>
    public partial class ReportViewrControl : UserControl
    {
        public string ReportXML
        {
            get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ReportViewrControl()
        {
            InitializeComponent();
            rvReport.ProcessingMode = ProcessingMode.Local;
        }

        /// <summary>
        /// Report viewer load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rvReport_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Load report in view control.
        /// </summary>
        /// <param name="report">Report stream.</param>
        public void LoadReportDefinition(MemoryStream report, string xml)
        {
            ReportXML = xml;
            rvReport.LocalReport.LoadReportDefinition(report);
        }

        /// <summary>
        /// Get report datasets names.
        /// </summary>
        /// <returns></returns>
        public List<string> GetReportDataSetsNames()
        {
            return rvReport.LocalReport.GetDataSourceNames().ToList();
        }

        /// <summary>
        /// Load data sources.
        /// </summary>
        /// <param name="dataSources"></param>
        public void LoadReportDataSources(List<ReportDataSource> dataSources)
        {
            foreach (var dataSource in dataSources)
            {
                rvReport.LocalReport.DataSources.Add(dataSource);
            }
        }

        /// <summary>
        /// Get parameters.
        /// </summary>
        /// <returns></returns>
        public ReportParameterInfoCollection GetReportParameters()
        {
            return rvReport.LocalReport.GetParameters();
        }

        /// <summary>
        /// Show report.
        /// </summary>
        public void ShowReport()
        {
            rvReport.LocalReport.Refresh();
            rvReport.RefreshReport();
        }
    }
}
