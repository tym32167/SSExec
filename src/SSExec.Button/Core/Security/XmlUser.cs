using System;
using System.Xml.Serialization;
using SSExec.Button.Core.Data.Contract;

namespace SSExec.Button.Core.Security
{
    [Serializable]
    public class XmlUser : IEntity<string>
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}