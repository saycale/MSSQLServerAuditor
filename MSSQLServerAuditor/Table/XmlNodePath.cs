using System.Linq;
using System.Xml;

namespace MSSQLServerAuditor.Table
{
    /// <summary>
    /// Path node in XML.
    /// </summary>
    public static class XmlNodePath
    {
        /// <summary>
        /// Find element.
        /// </summary>
        /// <param name="document">Document to search.</param>
        /// <param name="path">Path to document.</param>
        /// <returns>Found XML element.</returns>
        public static XmlElement FindElement(XmlDocument document, string path)
        {
            var pathNodes = path.Split('\\', '/');

            var currentElement = document.DocumentElement;
            if (currentElement != null && currentElement.Name != pathNodes[0])
                currentElement = null;

            for (int index = 1; index < pathNodes.Length; index++)
            {
                string pathNode = pathNodes[index];
                if (currentElement != null)
                {
                    var elements = currentElement.GetElementsByTagName(pathNode);
                    if (elements.Count > 0)
                        currentElement = (XmlElement)elements[0];
                    else
                        return null;
                }
            }

            return currentElement;
        }

        /// <summary>
        /// Find or create XML element.
        /// </summary>
        /// <param name="documentRoot">Document to search.</param>
        /// <param name="path">Path to document.</param>
        /// <returns>Found or created XML element.</returns>
        public static XmlElement FindOrCreateElement(XmlElement documentRoot, string path)
        {
            var root = documentRoot;

            if (!string.IsNullOrEmpty(path))
            {
                var hierarchyList = path.Split('\\');
                for (int index = 0; index < hierarchyList.Length; index++)
                {
                    string s = hierarchyList[index];
                    if (root.ChildNodes.Cast<XmlElement>().Any(c => c.Name == s))
                    {
                        root = root[s];
                    }
                    else
                    {
                        var newElement = documentRoot.OwnerDocument.CreateElement(s);
                        root.AppendChild(newElement);
                        root = newElement;
                    }
                }
            }
            return root;
        }

        /// <summary>
        /// Find or create XML node.
        /// </summary>
        /// <param name="documentRoot">Document to search.</param>
        /// <param name="path">Path to document.</param>
        /// <returns>Found or created XML element.</returns>
        public static XmlNode FindOrCreateElement(XmlNode documentRoot, string path)
        {
            var root = documentRoot;

            if (!string.IsNullOrEmpty(path))
            {
                var hierarchyList = path.Split('\\');
                for (int index = 0; index < hierarchyList.Length; index++)
                {
                    string s = hierarchyList[index];
                    if (root.ChildNodes.Cast<XmlElement>().Any(c => c.Name == s))
                    {
                        root = root[s];
                    }
                    else
                    {
                        var newElement = documentRoot.OwnerDocument.CreateElement(s);
                        root.AppendChild(newElement);
                        root = newElement;
                    }
                }
            }
            return root;
        }
    }
}