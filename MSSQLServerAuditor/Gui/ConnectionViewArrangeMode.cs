using System.Runtime.Serialization;
namespace MSSQLServerAuditor.Gui
{
    /// <summary>
    /// Mode view connection.
    /// </summary>
    public enum ConnectionViewArrangeMode
    {
        /// <summary>
        /// Tabs
        /// </summary>
        [EnumMember] AsTabs,
        /// <summary>
        /// Root nodes
        /// </summary>
        [EnumMember] AsRootNodes
    }
}