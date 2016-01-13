// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XSLPreparator.cs" company="">
//
// </copyright>
// <summary>
//   The xsl preparator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MSSQLServerAuditor.Licenser.Model.SignPreparators
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml.Linq;

    using MSSQLServerAuditor.Utils;

    /// <summary>
    /// The XSL preparator.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class XSLPreparator
    {
        /// <summary>
        /// The XSL file path.
        /// </summary>
        private readonly KeyValuePair<string, string> xslFilePath;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="XSLPreparator" /> class.
        /// </summary>
        /// <param name="xslFilePath">The XSL file path.</param>
        /// <param name="logger">The logger.</param>
        public XSLPreparator(KeyValuePair<string, string> xslFilePath, ILogger logger)
        {
            this.xslFilePath = xslFilePath;
            this.logger = logger;
        }

        /// <summary>
        /// The prepare.
        /// </summary>
        /// <param name="additionalTemplates">
        /// The additional template file path.
        /// </param>
        public void Prepare(List<AdditionalTemplate> additionalTemplates)
        {
            if (!PreparationNeeded(this.xslFilePath.Value, additionalTemplates))
            {
                return;
            }

            this.logger.WriteToLog("Вставка дополнительного шаблона");
            XDocument doc = XDocument.Load(this.xslFilePath.Key);
            if (doc.Root == null)
            {
                logger.WriteToLog("Не найден шаблон");
                throw new ArgumentException("XSL file template must be exists");
            }

            foreach (XElement element in doc.Root.Elements())
            {
                foreach (var preprocessorAttr in element.Attributes("preprocessor"))
                {
                    if (preprocessorAttr != null && preprocessorAttr.Value == "HtmlPreprocessorDialog")
                    {
                        XElement html = this.FindElement(element, "html");
                        if (html != null)
                        {
                            XElement body = this.FindElement(html, "body");
                            if (body != null)
                            {
                                XElement additionalHtml = this.GetHtmlContent(
                                    this.xslFilePath.Value,
                                    additionalTemplates);
                                body.AddFirst(additionalHtml);
                                doc.Save(this.xslFilePath.Key);

                                this.logger.WriteToLog("В шаблон добавлен дополнительный HTML");
                            }
                            else
                            {
                                this.logger.WriteToLog("Шаблон не содержит тэга body");
                            }
                        }
                        else
                        {
                            this.logger.WriteToLog("Шаблон не содержит тэга html");
                        }
                    }
                    else
                    {
                        this.logger.WriteToLog("Шаблон не содержит препроцессора HtmlPreprocessorDialog");
                    }
                }
            }
        }

        /// <summary>
        /// The preparation needed.
        /// </summary>
        /// <param name="locale">The additional template file path.</param>
        /// <param name="additionalTemplates">The template file path.</param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        private static bool PreparationNeeded(string locale, IEnumerable<AdditionalTemplate> additionalTemplates)
        {
            var template = additionalTemplates.FirstOrDefault(t => t.Locale == locale);
            return template != null && !string.IsNullOrEmpty(template.Locale);
        }

        /// <summary>
        /// The get html content.
        /// </summary>
        /// <param name="locale">The locale.</param>
        /// <param name="additionalTemplates">The additional templates.</param>
        /// <returns>
        /// The <see cref="XElement" />.
        /// </returns>
        private XElement GetHtmlContent(string locale, IEnumerable<AdditionalTemplate> additionalTemplates)
        {
            var template = additionalTemplates.First(t => t.Locale == locale);

            XDocument doc = XDocument.Load(template.FileName);
            return doc.Root;
        }

        /// <summary>
        /// The find element.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="elementName">
        /// The element name.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/>.
        /// </returns>
        private XElement FindElement(XElement parent, string elementName)
        {
            if (parent.Name == elementName)
            {
                return parent;
            }

            return parent
                .Elements()
                .Select(element => this.FindElement(element, elementName))
                .FirstOrDefault(foundElement => foundElement != null);
        }
    }
}