// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryPreparator.cs" company="">
//
// </copyright>
// <summary>
//   The query preparator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MSSQLServerAuditor.Licenser.Model.SignPreparators
{
    using System.Diagnostics.CodeAnalysis;

    using MSSQLServerAuditor.Model;
    using MSSQLServerAuditor.Utils;

    /// <summary>
    /// The query preparator.
    /// </summary>
    public class QueryPreparator
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly LicSettingsInfo settings;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryPreparator"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public QueryPreparator(LicSettingsInfo settings, ILogger logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        /// <summary>
        /// The prepare.
        /// </summary>
        /// <param name="queryItem">
        /// The query item.
        /// </param>
        public void PrepareIfNeeds(QueryItemInfo queryItem)
        {
            if (!this.PreparationNeeded(queryItem))
            {
                return;
            }

            this.logger.WriteToLog("Вставка дополнительного SQL");

            var additionalSql = this.settings.AdditionalSql.Replace("\r\n", string.Empty);
            if (!additionalSql.EndsWith(";"))
            {
                additionalSql += ";";
            }

            queryItem.Text = additionalSql + queryItem.Text;

            this.logger.WriteToLog("SQL вставлен");
        }

        /// <summary>
        ///     The preparation nedeed.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        private bool PreparationNeeded(QueryItemInfo queryItem)
        {
            return (queryItem.ParentQuery.Source == QuerySource.MSSQL || queryItem.ParentQuery.Source == QuerySource.TDSQL) // #187
                && !string.IsNullOrEmpty(this.settings.AdditionalSql);
        }
    }
}