namespace TBird.Utility.Tests.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using TBird.Utility.Enumerations;

    using NUnit.Framework;

    [TestFixture]
    public class EnumerationSerializationTestFixture
    {
        [Serializable]
        public class EnumSerializeClass : IXmlSerializable
        {
            public string SimpleString { get; set; }

            public int SimpleInt { get; set; }

            public TestEnums SimpleEnum { get; set; }

            public static List<string> AreEqual(EnumSerializeClass left, EnumSerializeClass right)
            {
                List<string> messages = new List<string>();
                if (left.SimpleString != right.SimpleString)
                {
                    messages.Add(string.Format("SimpleString not equal: {0} != {1}", left.SimpleString, right.SimpleString));
                }

                if (left.SimpleInt != right.SimpleInt)
                {
                    messages.Add(string.Format("SimpleInt not equal: {0} != {1}", left.SimpleInt, right.SimpleInt));
                }

                if (left.SimpleEnum != right.SimpleEnum)
                {
                    messages.Add(string.Format("SimpleEnum not equal: {0} != {1}", left.SimpleEnum, right.SimpleEnum));
                }

                return messages;
            }

            #region Implementation of IXmlSerializable

            /// <summary>
            /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
            /// </returns>
            public XmlSchema GetSchema()
            {
                return null;
            }

            /// <summary>
            /// Generates an object from its XML representation.
            /// </summary>
            /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized. </param>
            public void ReadXml(XmlReader reader)
            {
                reader.MoveToContent();
                this.SimpleString = reader.GetAttribute("SimpleString");
                this.SimpleInt = Convert.ToInt16(reader.GetAttribute("SimpleInt"));
                this.SimpleEnum = TestEnums.FromString(reader.GetAttribute("SimpleEnum"));
            }

            /// <summary>
            /// Converts an object into its XML representation.
            /// </summary>
            /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
            public void WriteXml(XmlWriter writer)
            {
                writer.WriteAttributeString("SimpleString", this.SimpleString);
                writer.WriteAttributeString("SimpleInt", this.SimpleInt.ToString());
                writer.WriteAttributeString("SimpleEnum", this.SimpleEnum.Name);
            }

            #endregion
        }

        [Test]
        public void XmlSerialization_SerializeDeSerialize_ReturnsSameObject()
        {
            EnumSerializeClass originalEnum = new EnumSerializeClass()
            {
                SimpleString = "Some random string",
                SimpleInt = 42,
                SimpleEnum = TestEnums.Value3,
            };
            EnumSerializeClass deserializedEnum;

            StringBuilder bldr = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
            };

            using (XmlWriter writer = XmlWriter.Create(new StringWriter(bldr), settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EnumSerializeClass));
                serializer.Serialize(writer, originalEnum);
            }

            string serializedObject = bldr.ToString();
            using (StringReader reader = new StringReader(serializedObject))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(EnumSerializeClass));
                deserializedEnum = serializer.Deserialize(reader) as EnumSerializeClass;
            }

            List<string> messages = EnumSerializeClass.AreEqual(originalEnum, deserializedEnum);
            StringBuilder errorMessage = new StringBuilder();
            foreach (string message in messages)
            {
                bldr.AppendLine(message);
            }

            Assert.AreEqual(0, messages.Count(), errorMessage.ToString());
        }

        [Test]
        public void BinarySerialization_SerializeDeSerialize_ReturnsSameObject()
        {
            EnumSerializeClass originalEnum = new EnumSerializeClass()
            {
                SimpleString = "Some random string",
                SimpleInt = 42,
                SimpleEnum = TestEnums.Value3,
            };
            EnumSerializeClass deserializedEnum;

            StringBuilder bldr = new StringBuilder();

            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, originalEnum);
                stream.Seek(0, SeekOrigin.Begin);
                deserializedEnum = formatter.Deserialize(stream) as EnumSerializeClass;
            }

            List<string> messages = EnumSerializeClass.AreEqual(originalEnum, deserializedEnum);
            StringBuilder errorMessage = new StringBuilder();
            foreach (string message in messages)
            {
                bldr.AppendLine(message);
            }

            Assert.AreEqual(0, messages.Count(), errorMessage.ToString());
        }

        [Serializable]
        public class TestSerializer
        {
            public int IntValue { get; set; }

            public string StringValue { get; set; }

            public EnumerationSerializer<TestEnums> SimpleEnum { get; set; }

            public static List<string> AreEqual(TestSerializer left, TestSerializer right)
            {
                List<string> messages = new List<string>();
                if (left.IntValue != right.IntValue)
                {
                    messages.Add(string.Format("IntValue not equal: {0} != {1}", left.IntValue, right.IntValue));
                }

                if (left.StringValue != right.StringValue)
                {
                    messages.Add(string.Format("StringValue not equal: {0} != {1}", left.StringValue, right.StringValue));
                }

                if (left.SimpleEnum.Enum != right.SimpleEnum.Enum)
                {
                    messages.Add(string.Format("StringValue not equal: {0} != {1}", left.SimpleEnum.Enum, right.SimpleEnum.Enum));
                }

                return messages;
            }
        }

        [Test]
        public void EnumerationSerializer_SerializeDeSerialize_ReturnsSameObject()
        {
            TestSerializer originalEnum = new TestSerializer()
            {
                IntValue = 3,
                StringValue = "Four",
                SimpleEnum = new EnumerationSerializer<TestEnums>() { Enum = TestEnums.Value4 },
            };

            TestSerializer deserializedEnum;

            StringBuilder bldr = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
            };

            using (XmlWriter writer = XmlWriter.Create(new StringWriter(bldr), settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestSerializer));
                serializer.Serialize(writer, originalEnum);
            }

            string serializedObject = bldr.ToString();
            using (StringReader reader = new StringReader(serializedObject))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestSerializer));
                deserializedEnum = serializer.Deserialize(reader) as TestSerializer;
            }

            List<string> messages = TestSerializer.AreEqual(originalEnum, deserializedEnum);
            StringBuilder errorMessage = new StringBuilder();
            foreach (string message in messages)
            {
                bldr.AppendLine(message);
            }

            Assert.AreEqual(0, messages.Count(), errorMessage.ToString());
        }
    }
}
