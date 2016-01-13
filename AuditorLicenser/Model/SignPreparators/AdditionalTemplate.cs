// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdditionalTemplate.cs" company="">
//
// </copyright>
// <summary>
//   The additional template.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MSSQLServerAuditor.Licenser.Model.SignPreparators
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// The additional template.
    /// </summary>
    [Serializable]
    public class AdditionalTemplate
    {
        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        [XmlAttribute]
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        [XmlAttribute]
        public string FileName { get; set; }
    }
}