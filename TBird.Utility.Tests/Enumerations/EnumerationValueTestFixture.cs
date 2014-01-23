namespace TBird.Utility.Tests.Enumerations
{
    using TBird.Utility.Enumerations;

    using NUnit.Framework;

    [TestFixture]
    public class EnumerationValueTestFixture
    {
        private class EnumerationValueSubClass : EnumerationValue
        {
            public int SomeNewValue { get; set; }
        }

        [Test]
        public void ToString_ReturnsStandardFormat()
        {
            EnumerationValue ev = new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value 1" };
            Assert.AreEqual("Value1(1) - Value 1", ev.ToString());
        }

        [Test]
        public void Equals_AllEqualCases()
        {
            EnumerationValue ev1 = new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value 1" };
            Assert.IsTrue(ev1.Equals(ev1));
            Assert.IsFalse(ev1.Equals(null));
            Assert.IsFalse(ev1.Equals((EnumerationValue)null));
            Assert.IsFalse(ev1.Equals("test"));
            Assert.IsFalse(ev1.Equals(new EnumerationValueSubClass()));

            EnumerationValue ev2 = new EnumerationValue() { Value = 2, Name = "Value1", DisplayName = "Value 1" };
            Assert.AreNotEqual(ev1, ev2);
            Assert.IsFalse(ev1.Equals(ev2));
            EnumerationValue ev3 = new EnumerationValue() { Value = 1, Name = "Value2", DisplayName = "Value 1" };
            Assert.IsFalse(ev1.Equals(ev3));
            EnumerationValue ev4 = new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value 2" };
            Assert.IsFalse(ev1.Equals(ev4));
        }

        [Test]
        public void GetHashCode_CompareEqualAndInEquality()
        {
            EnumerationValue ev1 = new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value 1" };
            EnumerationValue ev2 = new EnumerationValue() { Value = 2, Name = "Value2", DisplayName = "Value 2" };
            Assert.AreEqual(ev1.GetHashCode(), ev1.GetHashCode());
            Assert.AreNotEqual(ev1.GetHashCode(), ev2.GetHashCode());
        }
    }
}
