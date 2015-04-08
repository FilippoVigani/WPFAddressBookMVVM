using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using AddressBookTestClass.Properties;
using WpfCommonLibrary;

namespace AddressBookTestClass
{
    [Serializable]
    public class Contact : BaseViewModel, ISerializable, IXmlSerializable
    {
        private string name = "";
        private string number = "";
        private string email = "";

        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        [XmlAttribute]
        public string Number
        {
            get { return number; }
            set { SetProperty(ref number, value); }
        }

        [XmlAttribute]
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(Settings.Default.Name, Name);
            info.AddValue(Settings.Default.Number, Number);
            info.AddValue(Settings.Default.Email, Email);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Name = reader.GetAttribute(Settings.Default.Name);
            Number = reader.GetAttribute(Settings.Default.Number);
            Email = reader.GetAttribute(Settings.Default.Email);            
            var isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                reader.ReadEndElement();
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(Settings.Default.Name, Name);
            writer.WriteAttributeString(Settings.Default.Number, Number);
            writer.WriteAttributeString(Settings.Default.Email, Email);
        }
    }
}
