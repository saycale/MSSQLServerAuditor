// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogger.cs" company="">
//
// </copyright>
// <summary>
//   The Logger interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MSSQLServerAuditor.Licenser
{
    /// <summary>
    /// The Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// The write to log.
        /// </summary>
        /// <param name="st">
        /// The st.
        /// </param>
        void WriteToLog(string st);

        /// <summary>
        /// The write to log.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="parameters">The parameters.</param>
        void WriteToLog(string template, params object[] parameters);
    }
}