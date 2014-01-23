namespace TBird.Utility.Tests.Enumerations
{
    using System.Collections.Generic;

    public class ExtraPropertyTestValue
    {
        public int EnumValue { get; set; }

        public List<PropertyValue> Values { get; set; }
    }

    public class PropertyValue
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
}