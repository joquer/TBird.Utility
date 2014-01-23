// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeUtilsTest.cs" company="Mr Smarti Pantz LLC">
//   Copyright 2011 Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   This is a test class for TypeUtilsTest and is intended
//   to contain all TypeUtilsTest Unit Tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Linq;

    using NUnit.Framework;

    using TBird.Utility;

    /// <summary>
    /// This is a test class for TypeUtilsTest and is intended
    /// to contain all TypeUtilsTest Unit Tests
    /// </summary>
    [TestFixture]
    public class TypeHelperTest
    {
        [Test]
        public void GetType_FindType()
        {
            Type t = typeof(TypeHelperTest);
            Assert.AreEqual(t, TypeHelper.GetType(t.FullName));
            t = typeof(string);
            Assert.AreEqual(t, TypeHelper.GetType(t.FullName));
            Assert.IsNull(TypeHelper.GetType("BadTypeName"));
        }

        private class FlatTestObject
        {
            public int IntValue { get; set; }
            public bool BoolValue { get; set; }
            public double DoubleValue { get; set; }
            public string StringValue { get; set; }
        }

        private class FlatTestObject_Accessor
        {
            public int IntValue { get; set; }
            public bool BoolValue { get; set; }
            public double DoubleValue { get; set; }
            public string StringValue { get; set; }
        }

        private class SubClassObject
        {
            public int ValueA { get; set; }

            public decimal ValueB { get; set; }
        }

        private class NestedTestObject
        {
            public int TopValue { get; set; }

            public string TopString { get; set; }

            public SubClassObject SubObject { get; set; }
        }

        private class ObjectWithList
        {
            public List<FlatTestObject> ObjectList { get; private set; }

            public ObjectWithList()
            {
                this.ObjectList = new List<FlatTestObject>();
            }
        }

        private class ObjectWithCollection
        {
            public Collection<FlatTestObject> ObjectList { get; private set; }

            public ObjectWithCollection()
            {
                this.ObjectList = new Collection<FlatTestObject>();
            }
        }

        public class ObjectWithArray
        {
            public string[] StringArray { get; set; }
        }

        public class ObjectWithUri
        {
            public Uri TestUri { get; set; }
        }

        [Test]
        public void CompareObjects_CompareFlatObjectAllFieldsDifferent_ReturnsMessageList()
        {
            FlatTestObject left = new FlatTestObject() { BoolValue = true, IntValue = 5, DoubleValue = 12.2, StringValue = "String", };
            FlatTestObject right = new FlatTestObject() { BoolValue = false, IntValue = 4, DoubleValue = 102.2, StringValue = "Different String", };
            Collection<string> messages = TypeHelper.CompareObjects("FlatTestObject", left, right);
            Assert.AreEqual(4, messages.Count, TypeHelper.FlattenMessages("FlatTestObject", messages));
        }

        [Test]
        public void CompareObjects_CompareFlatObjectAllFieldsEqual_ReturnsEmptyList()
        {
            FlatTestObject left = new FlatTestObject() { BoolValue = true, IntValue = 5, DoubleValue = 12.2, StringValue = "String", };
            Collection<string> messages = TypeHelper.CompareObjects("FlatTestObject", left, left);
            Assert.AreEqual(0, messages.Count, TypeHelper.FlattenMessages("FlatTestObject", messages));
        }

        [Test]
        public void CompareObjects_CompareObjectAndObjectAccessor_ReturnsEmptyList()
        {
            FlatTestObject left = new FlatTestObject() { BoolValue = true, IntValue = 5, DoubleValue = 12.2, StringValue = "String", };
            FlatTestObject_Accessor right = new FlatTestObject_Accessor() { BoolValue = true, IntValue = 5, DoubleValue = 12.2, StringValue = "String", };
            Collection<string> messages = TypeHelper.CompareObjects("FlatTestObject", left, right);
            Assert.AreEqual(0, messages.Count, TypeHelper.FlattenMessages("FlatTestObject", messages));

            FlatTestObject_Accessor left2 = new FlatTestObject_Accessor() { BoolValue = true, IntValue = 5, DoubleValue = 12.2, StringValue = "String", };
            FlatTestObject right2 = new FlatTestObject() { BoolValue = true, IntValue = 5, DoubleValue = 12.2, StringValue = "String", };
            messages = TypeHelper.CompareObjects("FlatTestObject", left2, right2);
            Assert.AreEqual(0, messages.Count, TypeHelper.FlattenMessages("FlatTestObject", messages));
        }

        [Test]
        public void CompareObjects_CompareNestedObjectAllFieldsDifferent_ReturnsMessageList()
        {
            NestedTestObject left = new NestedTestObject()
            {
                TopValue = 1002,
                TopString = "Top String Value",
                SubObject = new SubClassObject() { ValueA = 97, ValueB = 2010.10m, }
            };
            NestedTestObject right = new NestedTestObject()
            {
                TopValue = 2010,
                TopString = "Different String",
                SubObject = new SubClassObject() { ValueA = 12, ValueB = 1002.0m, }
            };
            Collection<string> messages = TypeHelper.CompareObjects("NestedTestObject", left, right);
            Assert.AreEqual(4, messages.Count, TypeHelper.FlattenMessages("NestedTestObject", messages));
        }

        [Test]
        public void CompareObjects_CompareNestedObjectAllFieldsEqual_ReturnsEmptyList()
        {
            NestedTestObject left = new NestedTestObject()
            {
                TopValue = 1002,
                TopString = "Top String Value",
                SubObject = new SubClassObject() { ValueA = 97, ValueB = 2010.10m, }
            };
            Collection<string> messages = TypeHelper.CompareObjects("NestedTestObject", left, left);
            Assert.AreEqual(0, messages.Count, TypeHelper.FlattenMessages("NestedTestObject", messages));
        }

        [Test]
        public void CompareObjects_CompaerObjectsWithArray_ObjectsEqual()
        {
            ObjectWithArray owa = new ObjectWithArray() { StringArray = new[] { "one", "two", "three" }, };
            Collection<string> messages = TypeHelper.CompareObjects(owa.GetType().Name, owa, owa);
            Assert.AreEqual(0, messages.Count, TypeHelper.FlattenMessages(owa.GetType().Name, messages));
        }

        [Test]
        public void CompareObjects_CompareObjectWithList_ReturnsEmptyList()
        {
            ObjectWithList left = new ObjectWithList();
            for (int i = 0; i < 10; i++)
            {
                left.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = i % 2 == 0,
                        DoubleValue = i * 1224.0,
                        IntValue = i * 113,
                        StringValue = string.Format("String {0}", i * 73),
                    });
            }

            Collection<string> messages = TypeHelper.CompareObjects("ObjectWithList", left, left);
            Assert.AreEqual(0, messages.Count, TypeHelper.FlattenMessages("ObjectWithList", messages));
        }

        [Test]
        public void CompareObjects_CompareObjectWithList_ItemsAreDifferent()
        {
            const int NumberObjects = 2;
            ObjectWithList left = new ObjectWithList();
            for (int i = 1; i <= NumberObjects; i++)
            {
                left.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = false,
                        DoubleValue = i * 1224.0,
                        IntValue = i * 113,
                        StringValue = string.Format("String {0}", i * 73),
                    });
            }

            ObjectWithList right = new ObjectWithList();
            for (int i = 1; i <= NumberObjects; i++)
            {
                right.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = true,
                        DoubleValue = i * 124.0,
                        IntValue = i * 13,
                        StringValue = string.Format("String {0}", i * 42),
                    });
            }

            Collection<string> messages = TypeHelper.CompareObjects("ObjectWithList", left, right);
            Assert.AreEqual(8, messages.Count, TypeHelper.FlattenMessages("ObjectWithList", messages));
        }

        [Test]
        public void CompareObjects_CompareObjectWithList_ListsAreOfDifferentLengths()
        {
            ObjectWithList left = new ObjectWithList();
            for (int i = 0; i < 10; i++)
            {
                left.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = i % 2 == 0,
                        DoubleValue = i * 1224.0,
                        IntValue = i * 113,
                        StringValue = string.Format("String {0}", i * 73),
                    });
            }

            ObjectWithList right = new ObjectWithList();
            for (int i = 0; i < 5; i++)
            {
                right.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = i % 3 == 0,
                        DoubleValue = i * 124.0,
                        IntValue = i * 13,
                        StringValue = string.Format("String {0}", i * 42),
                    });
            }

            Collection<string> messages = TypeHelper.CompareObjects("ObjectWithList", left, right);
            Assert.AreEqual(1, messages.Count, TypeHelper.FlattenMessages("ObjectWithList", messages));
        }
        [Test]
        public void CompareObjects_CompareObjectWithCollection_ReturnsEmptyList()
        {
            ObjectWithCollection left = new ObjectWithCollection();
            for (int i = 0; i < 10; i++)
            {
                left.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = i % 2 == 0,
                        DoubleValue = i * 1224.0,
                        IntValue = i * 113,
                        StringValue = string.Format("String {0}", i * 73),
                    });
            }

            Collection<string> messages = TypeHelper.CompareObjects("ObjectWithList", left, left);
            Assert.AreEqual(0, messages.Count, TypeHelper.FlattenMessages("ObjectWithList", messages));
        }

        [Test]
        public void CompareObjects_CompareObjectWithCollection_ItemsAreDifferent()
        {
            const int NumberObjects = 2;
            ObjectWithCollection left = new ObjectWithCollection();
            for (int i = 1; i <= NumberObjects; i++)
            {
                left.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = false,
                        DoubleValue = i * 1224.0,
                        IntValue = i * 113,
                        StringValue = string.Format("String {0}", i * 73),
                    });
            }

            ObjectWithCollection right = new ObjectWithCollection();
            for (int i = 1; i <= NumberObjects; i++)
            {
                right.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = true,
                        DoubleValue = i * 124.0,
                        IntValue = i * 13,
                        StringValue = string.Format("String {0}", i * 42),
                    });
            }

            Collection<string> messages = TypeHelper.CompareObjects("ObjectWithList", left, right);
            Assert.AreEqual(8, messages.Count, TypeHelper.FlattenMessages("ObjectWithList", messages));
        }

        [Test]
        public void CompareObjects_CompareObjectWithCollection_CollectionsAreOfDifferentLengths()
        {
            ObjectWithCollection left = new ObjectWithCollection();
            for (int i = 0; i < 10; i++)
            {
                left.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = i % 2 == 0,
                        DoubleValue = i * 1224.0,
                        IntValue = i * 113,
                        StringValue = string.Format("String {0}", i * 73),
                    });
            }

            ObjectWithCollection right = new ObjectWithCollection();
            for (int i = 0; i < 5; i++)
            {
                right.ObjectList.Add(
                    new FlatTestObject()
                    {
                        BoolValue = i % 3 == 0,
                        DoubleValue = i * 124.0,
                        IntValue = i * 13,
                        StringValue = string.Format("String {0}", i * 42),
                    });
            }

            Collection<string> messages = TypeHelper.CompareObjects("ObjectWithList", left, right);
            Assert.AreEqual(1, messages.Count, TypeHelper.FlattenMessages("ObjectWithList", messages));
        }

        [Test]
        public void CompareObjects_CompaerObjectsWithUri_ObjectsEqual()
        {
            ObjectWithUri owu = new ObjectWithUri() { TestUri = new Uri("http://www.mrsmartipantz.com/") };
            Collection<string> messages = TypeHelper.CompareObjects(owu.GetType().Name, owu, owu);
            Assert.AreEqual(0, messages.Count, TypeHelper.FlattenMessages(owu.GetType().Name, messages));
        }
    }
}
