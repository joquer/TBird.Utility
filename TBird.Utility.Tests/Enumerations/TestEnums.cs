namespace TBird.Utility.Tests.Enumerations
{
    using System;
    using System.Collections.Generic;

    using TBird.Utility.Enumerations;

#if !NETFX_CORE
    [Serializable]
#endif
    public class TestEnums : Enumeration<TestEnums>
    {
        public static TestEnums Value1 = new TestEnums(1);

        public static TestEnums Value2 = new TestEnums(2, "This is Value 2");

        public static TestEnums Value3 = new TestEnums(3);

        public static TestEnums Value4 = new TestEnums(4);

        [EnumerationDefaultValue]
        public static TestEnums ValueDefault = new TestEnums(100, "Default Value");

        protected TestEnums(int value, string displayName)
            : base(value, displayName)
        {
        }

        protected TestEnums(int value)
            : base(value)
        {
        }
    }

#if !NETFX_CORE
    [Serializable]
#endif
    public class DefaultLoadTestEnums : Enumeration<DefaultLoadTestEnums>
    {
        [EnumerationDefaultValue]
        public static DefaultLoadTestEnums Value1 = new DefaultLoadTestEnums(1);

        public static DefaultLoadTestEnums Value2 = new DefaultLoadTestEnums(2, "This is Value 2");

        public static DefaultLoadTestEnums Value3 = new DefaultLoadTestEnums(3);

        public static DefaultLoadTestEnums Value4 = new DefaultLoadTestEnums(4);

        public static DefaultLoadTestEnums ValueDefault = new DefaultLoadTestEnums(100, "Default Value");

        protected DefaultLoadTestEnums(int value, string displayName)
            : base(value, displayName)
        {
        }

        protected DefaultLoadTestEnums(int value)
            : base(value)
        {
        }
    }

    public class SecondEnums : Enumeration<SecondEnums>
    {
        [EnumerationDefaultValue]
        public static SecondEnums SV1 = new SecondEnums(1);

        public static SecondEnums SV2 = new SecondEnums(2);

        protected SecondEnums(int value)
            : base(value)
        {
        }
    }

    public class DuplicateValueEnum : Enumeration<DuplicateValueEnum>
    {
        [EnumerationDefaultValue]
        public static DuplicateValueEnum FirstValue = new DuplicateValueEnum(1);

        public static DuplicateValueEnum SecondValue = new DuplicateValueEnum(2);

        public static DuplicateValueEnum ThirdValue = new DuplicateValueEnum(2);

        protected DuplicateValueEnum(int value)
            : base(value)
        {
        }
    }

    [EnumerationTableBindings("dbo", "TestVerifyEnums")]
    public class TestVerifyEnum : Enumeration<TestVerifyEnum>
    {
        [EnumerationDefaultValue]
        public static TestVerifyEnum Value1 = new TestVerifyEnum(1, "Value 1");

        public static TestVerifyEnum Value2 = new TestVerifyEnum(2, "Value 2");

        public static TestVerifyEnum Value3 = new TestVerifyEnum(3, "Value 3");

        public static TestVerifyEnum Value4 = new TestVerifyEnum(4, "Value 4");

        public static TestVerifyEnum Value5 = new TestVerifyEnum(5);

        protected TestVerifyEnum(int value)
            : base(value)
        {
        }

        protected TestVerifyEnum(int value, string displayName)
            : base(value, displayName)
        {
        }
    }

    public class DefaultValueAttributeEnum : Enumeration<DefaultValueAttributeEnum>
    {
        public static DefaultValueAttributeEnum FirstValue = new DefaultValueAttributeEnum(1);

        public static DefaultValueAttributeEnum SecondValue = new DefaultValueAttributeEnum(2);

        [EnumerationDefaultValue]
        public static DefaultValueAttributeEnum ThirdValue = new DefaultValueAttributeEnum(3);

        public static DefaultValueAttributeEnum FourthValue = new DefaultValueAttributeEnum(4);

        public DefaultValueAttributeEnum(int value)
            : base(value)
        {
        }
    }

    public class DefaultValueWithMultipleDefaultsEnum : Enumeration<DefaultValueWithMultipleDefaultsEnum>
    {
        public static DefaultValueWithMultipleDefaultsEnum FirstValue = new DefaultValueWithMultipleDefaultsEnum(1);

        [EnumerationDefaultValue]
        public static DefaultValueWithMultipleDefaultsEnum SecondValue = new DefaultValueWithMultipleDefaultsEnum(2);

        [EnumerationDefaultValue]
        public static DefaultValueWithMultipleDefaultsEnum ThirdValue = new DefaultValueWithMultipleDefaultsEnum(3);

        public static DefaultValueWithMultipleDefaultsEnum FourthValue = new DefaultValueWithMultipleDefaultsEnum(4);

        public DefaultValueWithMultipleDefaultsEnum(int value)
            : base(value)
        {
        }
    }

    public class EnumWithNoDefault : Enumeration<EnumWithNoDefault>
    {
        public static EnumWithNoDefault FirstValue = new EnumWithNoDefault(1);

        public static EnumWithNoDefault SecondValue = new EnumWithNoDefault(2);

        public static EnumWithNoDefault ThirdValue = new EnumWithNoDefault(3);

        public static EnumWithNoDefault FourthValue = new EnumWithNoDefault(4);

        public EnumWithNoDefault(int value)
            : base(value)
        {
        }
    }

    [EnumerationTableBindings("dbo", "UpdateDatabaseEnums")]
    public class UpdateDatabaseEnums : Enumeration<UpdateDatabaseEnums>
    {
        public static UpdateDatabaseEnums Value1 = new UpdateDatabaseEnums(1);

        public static UpdateDatabaseEnums Value2 = new UpdateDatabaseEnums(2);

        public static UpdateDatabaseEnums Value3 = new UpdateDatabaseEnums(3);

        public static UpdateDatabaseEnums Value4 = new UpdateDatabaseEnums(4, "Display Name with special char's \" & *");

        public UpdateDatabaseEnums(int value, string displayNamne)
            : base(value, displayNamne)
        {
        }

        public UpdateDatabaseEnums(int value)
            : base(value)
        {
        }
    }

    [EnumerationTableBindings("dbo", "BadLineFormatCsvEnums")]
    public class BadLineFormatCsvEnums : Enumeration<BadLineFormatCsvEnums>
    {
        public static BadLineFormatCsvEnums Value1 = new BadLineFormatCsvEnums(1);

        public static BadLineFormatCsvEnums Value2 = new BadLineFormatCsvEnums(2);

        public static BadLineFormatCsvEnums Value3 = new BadLineFormatCsvEnums(3);

        public BadLineFormatCsvEnums(int value)
            : base(value)
        {
        }
    }

    [EnumerationTableBindings("dbo", "BadHeaderCsvEnums")]
    public class BadHeaderCsvEnums : Enumeration<BadHeaderCsvEnums>
    {
        public static BadHeaderCsvEnums Value1 = new BadHeaderCsvEnums(1);

        public static BadHeaderCsvEnums Value2 = new BadHeaderCsvEnums(2);

        public static BadHeaderCsvEnums Value3 = new BadHeaderCsvEnums(3);

        public BadHeaderCsvEnums(int value)
            : base(value)
        {
        }
    }

    // Enumeration classes with extra data values that are to synchronized with the database.
    [EnumerationTableBindings("dbo", "ExtraPropertyEnums")]
    public class ExtraPropertyEnums : Enumeration<ExtraPropertyEnums>
    {
        public static ExtraPropertyEnums Value1 = new ExtraPropertyEnums(1)
        {
            IntValue = 1,
            StringValue = "1",
            SomeOtherValue = 1
        };

        public static ExtraPropertyEnums Value2 = new ExtraPropertyEnums(2)
        {
            IntValue = 2,
            StringValue = "2",
            SomeOtherValue = 2
        };

        public static ExtraPropertyEnums Value3 = new ExtraPropertyEnums(3)
        {
            IntValue = 3,
            StringValue = "3",
            SomeOtherValue = 3
        };

        [EnumerationPropertyBinding]
        public int IntValue { get; set; }

        [EnumerationPropertyBinding]
        public string StringValue { get; set; }

        [EnumerationPropertyBinding(ColumnName = "OtherValue")]
        public int SomeOtherValue { get; set; }

        public ExtraPropertyEnums(int value)
            : base(value)
        {
        }
    }

    // Enumeration classes with extra data values that are to synchronized with the database.
    [EnumerationTableBindings("dbo", "FirstBadExtraPropertyEnums")]
    public class FirstBadExtraPropertyEnums : Enumeration<FirstBadExtraPropertyEnums>
    {
        public static FirstBadExtraPropertyEnums Value1 = new FirstBadExtraPropertyEnums(1)
        {
            IntValue = 1,
            StringValue = "1",
            SomeOtherValue = 1
        };

        public static FirstBadExtraPropertyEnums Value2 = new FirstBadExtraPropertyEnums(2)
        {
            IntValue = 2,
            StringValue = "2",
            SomeOtherValue = 2
        };

        public static FirstBadExtraPropertyEnums Value3 = new FirstBadExtraPropertyEnums(3)
        {
            IntValue = 3,
            StringValue = "3",
            SomeOtherValue = 3
        };

        [EnumerationPropertyBinding]
        public int IntValue { get; set; }

        [EnumerationPropertyBinding]
        public string StringValue { get; set; }

        [EnumerationPropertyBinding(ColumnName = "OtherValue")]
        public int SomeOtherValue { get; set; }

        public FirstBadExtraPropertyEnums(int value)
            : base(value)
        {
        }
    }

    // Enumeration classes with extra data values that are to synchronized with the database.
    [EnumerationTableBindings("dbo", "SecondBadExtraPropertyEnums")]
    public class SecondBadExtraPropertyEnums : Enumeration<SecondBadExtraPropertyEnums>
    {
        public static SecondBadExtraPropertyEnums Value1 = new SecondBadExtraPropertyEnums(1)
        {
            IntValue = 1,
            StringValue = "1",
            SomeOtherValue = 1
        };

        public static SecondBadExtraPropertyEnums Value2 = new SecondBadExtraPropertyEnums(2)
        {
            IntValue = 2,
            StringValue = "2",
            SomeOtherValue = 2
        };

        public static SecondBadExtraPropertyEnums Value3 = new SecondBadExtraPropertyEnums(3)
        {
            IntValue = 3,
            StringValue = "3",
            SomeOtherValue = 3
        };

        [EnumerationPropertyBinding]
        public int IntValue { get; set; }

        [EnumerationPropertyBinding]
        public string StringValue { get; set; }

        [EnumerationPropertyBinding(ColumnName = "OtherValue")]
        public int SomeOtherValue { get; set; }

        public SecondBadExtraPropertyEnums(int value)
            : base(value)
        {
        }
    }

    // Enumeration classes with extra data values that are to synchronized with the database.
    [EnumerationTableBindings("dbo", "ThirdBadExtraPropertyEnums")]
    public class ThirdBadExtraPropertyEnums : Enumeration<ThirdBadExtraPropertyEnums>
    {
        public static ThirdBadExtraPropertyEnums Value1 = new ThirdBadExtraPropertyEnums(1)
        {
            IntValue = 1,
            StringValue = "1",
            SomeOtherValue = 1
        };

        public static ThirdBadExtraPropertyEnums Value2 = new ThirdBadExtraPropertyEnums(2)
        {
            IntValue = 2,
            StringValue = "2",
            SomeOtherValue = 2
        };

        public static ThirdBadExtraPropertyEnums Value3 = new ThirdBadExtraPropertyEnums(3)
        {
            IntValue = 3,
            StringValue = "3",
            SomeOtherValue = 3
        };

        [EnumerationPropertyBinding]
        public int IntValue { get; set; }

        [EnumerationPropertyBinding]
        public string StringValue { get; set; }

        [EnumerationPropertyBinding(ColumnName = "OtherValue")]
        public int SomeOtherValue { get; set; }

        public ThirdBadExtraPropertyEnums(int value)
            : base(value)
        {
        }
    }

    // These are bad enum definition classes and are used to test error handling of the
    // update and verification routines.
    [EnumerationTableBindings("dbo", "BadEnumClass")]
    public class BadEnumClass
    {
    }

    [EnumerationTableBindings("dbo", "BadEnumClass2")]
    public class BadEnumClassBadGetValues
    {
        public static IEnumerable<EnumerationValue> GetValues()
        {
            return null;
        }
    }
}
