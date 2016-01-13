using System;
using System.Windows.Forms;

namespace MSSQLServerAuditor.Gui.LayoutSettings
{
    internal interface ILayoutSettingsProvider<TLayoutSettings> : IDisposable where TLayoutSettings : class, IFormLayoutSettings, new()
    {
        void AttachToChangeLayout();
        void DettachToChangeLayout();
        void SetPrefix(DataGridView dataGrid, string prefix);

        /// <summary>
        /// Load GUI settings from XML
        /// </summary>
        /// <param name="defValue"></param>
        /// <returns>true - settings loaded, false - no</returns>
        bool LoadSettings(TLayoutSettings defValue = null);

        /// <summary>
        /// Apply settings from object _settings to GUI
        /// </summary>
        void ApplySettings();

        /// <summary>
        /// Update _settings object by real GUI settings
        /// </summary>
        void UpdateSettings();

        /// <summary>
        /// Save GUI settings from XML
        /// </summary>
        void SaveSettings();

        TLayoutSettings Settings { get; }
    }
}