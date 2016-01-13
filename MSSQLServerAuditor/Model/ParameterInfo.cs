using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MSSQLServerAuditor.Model
{
    [Serializable]
    public class ParameterInfo
    {
        [XmlAttribute]
        public string Key { get; set; }
        [XmlAttribute]
        public ParameterInfoType Type { get; set; }
        [XmlAttribute]
        public string Parameter { get; set; }
        [XmlAttribute]
        public string Default { get; set; }
        [XmlAttribute]
        public string Value { get; set; }

        public ParameterInfo() { }

        public ParameterInfo(string key, ParameterInfoType type, string parameter, string @default, string value)
        {
            Key = key;
            Parameter = parameter;
            Type = type;
            Default = @default;
            Value = value;
        }
    }
}
