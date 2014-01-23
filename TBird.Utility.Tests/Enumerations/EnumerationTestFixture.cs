namespace TBird.Utility.Tests.Enumerations

{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using TBird.Utility.Enumerations;

    using NUnit.Framework;

    [TestFixture]
    public class EnumerationTestFixture
    {
        [Test]
        public void EnumCtor_SetEnumValues_AllEnumFieldsPopulated()
        {
            // Test the case where only a value is supplied so that name and displayname are taken from Field name.
            TestEnums t = TestEnums.Value1;
            Assert.AreEqual(1, t.Value, "Enum value does not match");
            Assert.AreEqual("Value1", t.Name, "Enum Name does not match");
            Assert.AreEqual("Value1", t.DisplayName, "Enum display string does not match");

            // Test case where a different displaystring is specified.
            t = TestEnums.Value2;
            Assert.AreEqual(2, t.Value, "Enum value does not match");
            Assert.AreEqual("Value2", t.Name, "Enum Name does not match");
            Assert.AreEqual("This is Value 2", t.DisplayName, "Enum display string does not match");
        }

        [Test]
        public void IsValid_TestAllCases()
        {
            Assert.IsTrue(TestEnums.IsValidValue(1));
            Assert.IsFalse(TestEnums.IsValidValue(1234));
            Assert.IsTrue(TestEnums.IsValidName("Value1"));
            Assert.IsFalse(TestEnums.IsValidName("Value1234"));
            Assert.IsTrue(TestEnums.IsValidDisplayName("Value1"));
            Assert.IsFalse(TestEnums.IsValidDisplayName("Value1234"));
            Assert.IsTrue(TestEnums.IsValidDisplayName("This is Value 2"));
        }

        private readonly List<ExtraPropertyTestValue> extraValues = new List<ExtraPropertyTestValue>()
            {
                new ExtraPropertyTestValue()
                    {
                        EnumValue = 1,
                        Values =
                            new List<PropertyValue>
                                {
                                    new PropertyValue() { Name = "IntValue", Value = 1 },
                                    new PropertyValue() { Name = "StringValue", Value = "1" },
                                    new PropertyValue() { Name = "SomeOtherValue", Value = 1 },
                                },
                    },
                new ExtraPropertyTestValue()
                    {
                        EnumValue = 2,
                        Values =
                            new List<PropertyValue>
                                {
                                    new PropertyValue() { Name = "IntValue", Value = 2 },
                                    new PropertyValue() { Name = "StringValue", Value = "2" },
                                    new PropertyValue() { Name = "SomeOtherValue", Value = 2 },
                                },
                    },
                new ExtraPropertyTestValue()
                    {
                        EnumValue = 3,
                        Values =
                            new List<PropertyValue>
                                {
                                    new PropertyValue() { Name = "IntValue", Value = 3 },
                                    new PropertyValue() { Name = "StringValue", Value = "3" },
                                    new PropertyValue() { Name = "SomeOtherValue", Value = 3 },
                                },
                    },
            };

        private object GetPropertyValue(int enumValue, string propertyName)
        {
            ExtraPropertyTestValue testValue = this.extraValues.FirstOrDefault(x => x.EnumValue == enumValue);
            if (testValue == null)
            {
                return null;
            }

            return testValue.Values.Where(x => x.Name == propertyName).Select(x => x.Value).First();
        }

        [Test]
        public void EnumCtor_LoadsExtraProperty_ReturnsAllValues()
        {
            foreach (EnumerationValue value in ExtraPropertyEnums.GetValues())
            {
                foreach (string key in value.PropertyValues.Keys)
                {
                    Assert.AreEqual(this.GetPropertyValue(value.Value, key), value.PropertyValues[key]);
                }
            }
        }

        [Test]
        public void Equals_TestEquality()
        {
            TestEnums t = TestEnums.Value1;
            Assert.IsTrue(TestEnums.Value1 == t);
            Assert.IsFalse(TestEnums.Value1 == TestEnums.Value4);
            Assert.IsTrue(TestEnums.Value3.Equals(3));
            Assert.IsFalse(TestEnums.Value3.Equals(1001));
        }

        [Test]
        public void EnumLoad_LoadFromIntAndString_AllEnumsFound()
        {
            TestEnums intEnum = TestEnums.FromInt(2);
            Assert.IsNotNull(intEnum);
            Assert.AreEqual(TestEnums.Value2, intEnum);

            TestEnums stringEnum = TestEnums.FromString("Value3");
            Assert.IsNotNull(stringEnum);
            Assert.AreEqual(TestEnums.Value3, stringEnum);

            TestEnums displayNameEnum = TestEnums.FromDisplayName("This is Value 2");
            Assert.IsNotNull(displayNameEnum);
            Assert.AreEqual(TestEnums.Value2, displayNameEnum);
        }

        [Test]
        public void DefaultValue_GetDefaultValueIsFirstCall_ReturnsNonNull()
        {
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual("Value1", DefaultLoadTestEnums.DefaultValue.DisplayName);
            }
        }

        [Test]
        public void EnumLoad_LoadFromIntAndString_ReturnsDefault()
        {
            TestEnums intEnum = TestEnums.FromIntWithDefault(10001);
            Assert.IsNotNull(intEnum);
            Assert.AreEqual(TestEnums.DefaultValue, intEnum);

            TestEnums stringEnum = TestEnums.FromStringWithDefault("BadValue");
            Assert.IsNotNull(stringEnum);
            Assert.AreEqual(TestEnums.DefaultValue, stringEnum);

            TestEnums displayNameEnum = TestEnums.FromDisplayNameWithDefault("Another Bad Value");
            Assert.IsNotNull(displayNameEnum);
            Assert.AreEqual(TestEnums.DefaultValue, displayNameEnum);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FromString_LoadInvalidString_ThrowArgumentException()
        {
            TestEnums badEnum = TestEnums.FromString("BadEnumName");
            Assert.Fail("FromString() shouild have thrown an ArugmentException.");
        }

        [Test]
        public void GetAll_GetAllFromDifferentEnums_ReturnsAllValues()
        {
            List<int> testEnumValues = new List<int>() { 1, 2, 3, 4, 100 };
            CollectionAssert.AreEqual(testEnumValues, TestEnums.Enumerations.OrderBy(e => e.Value).Select(e => e.Value).ToList());

            List<int> secondEnumVlaues = new List<int>() { 1, 2 };
            CollectionAssert.AreEqual(secondEnumVlaues, SecondEnums.Enumerations.OrderBy(e => e.Value).Select(e => e.Value).ToList());
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetAll_EnumHasDuplicateValue_ThrowsInvalidOperationException()
        {
            Assert.AreEqual(DuplicateValueEnum.Enumerations.Count(), 3);
        }

        [Test]
        public void IntCast_CastValuetoIntAndCompare_AreEqual()
        {
            int val = (int)TestEnums.Value2;
            Assert.AreEqual(2, val);
        }

        [Test]
        public void DefaultValue_TestDefaultValueWithAttribute_ReturnsTaggedDefault()
        {
            DefaultValueAttributeEnum defaultValueAttribute = DefaultValueAttributeEnum.DefaultValue;
            Assert.AreEqual(DefaultValueAttributeEnum.ThirdValue, defaultValueAttribute);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DefaultValue_TestEnumWithMultipleDefaultAttributes_ThrowsInvalidOperation()
        {
            DefaultValueWithMultipleDefaultsEnum defaultValueAttribute = DefaultValueWithMultipleDefaultsEnum.DefaultValue;
            Assert.AreEqual(DefaultValueWithMultipleDefaultsEnum.SecondValue, defaultValueAttribute);
        }

        [Test]
        public void DefaultValue_TestEnumWithNoDefault_ReturnsNull()
        {
            EnumWithNoDefault defaultValueAttribute = EnumWithNoDefault.FirstValue;
            defaultValueAttribute = EnumWithNoDefault.DefaultValue;
            Assert.IsNull(defaultValueAttribute);
        }

        [Test]
        public void VerifyEnum_TestGoodComparison_ReturnsEmptyList()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new EnumerationTableBindingsTestFixture.GoodDatabaseProvider());
            Assert.AreEqual(0, ev.VerifyEnumeration(typeof(TestVerifyEnum)).Count());
        }

        [Test]
        public void VerifyEnum_TestBadCountComparison_ReturnsSingleDifference()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new EnumerationTableBindingsTestFixture.BadCountDatabaseProvider());
            Assert.AreEqual(1, ev.VerifyEnumeration(typeof(TestVerifyEnum)).Count());
        }

        [Test]
        public void VerifyEnum_TestBadValueComparison_ReturnsSingleDifference()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new EnumerationTableBindingsTestFixture.BadValueDatabaseProvider());
            Assert.AreEqual(1, ev.VerifyEnumeration(typeof(TestVerifyEnum)).Count());
        }

        [Test]
        public void Equals_CompareTwoEnums_EqualityAndNon()
        {
            Assert.IsTrue(TestEnums.Value1.Equals(TestEnums.Value1));
            Assert.IsFalse(TestEnums.Value1.Equals(TestEnums.Value2));
        }

        [Test]
        public void Equals_CompaerToNull_ReturnsFalse()
        {
            Assert.IsFalse(TestEnums.Value1.Equals(null));
        }

        [Test]
        public void CompareTo_CompareEqualityCases_ReturnsCompareValue()
        {
            Assert.IsTrue(TestEnums.Value3.CompareTo(TestEnums.Value4) < 0);
            Assert.IsTrue(TestEnums.Value3.CompareTo(TestEnums.Value2) > 0);
            Assert.IsTrue(TestEnums.Value3.CompareTo(TestEnums.Value3) == 0);
        }

        [Test]
        public void GetHashCode_CompareEqualAndInEquality()
        {
            Assert.AreEqual(TestEnums.Value1.GetHashCode(), TestEnums.Value1.GetHashCode());
            Assert.AreNotEqual(TestEnums.Value1.GetHashCode(), TestEnums.Value2.GetHashCode());
        }

        [Test]
        public void LessThanOperator_TestAllCases()
        {
            TestEnums val1 = TestEnums.Value1;
            TestEnums val2 = TestEnums.Value2;
            Assert.IsTrue(val1 < val2);
            Assert.IsFalse(val2 < val1);
            Assert.IsFalse(val1 < null);

            // This is a valid test, but causes a warning, not sure how to get rid of the warning.
            // TestEnums val3 = TestEnums.Value3;
            // Assert.IsFalse(val3 < val3);
        }

        [Test]
        public void GreaterThanOperator_TestAllCases()
        {
            TestEnums val1 = TestEnums.Value1;
            TestEnums val2 = TestEnums.Value2;
            Assert.IsTrue(val2 > val1);
            Assert.IsFalse(val1 > val2);
            Assert.IsFalse(val1 > null);

            // This is a valid test, but causes a warning, not sure how to get rid of the warning.
            // TestEnums val3 = TestEnums.Value3;
            // Assert.IsFalse(val3 > val3);
        }
    }
}
