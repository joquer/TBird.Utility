namespace TBird.Utility.Tests.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TBird.Utility.Enumerations;

    using NUnit.Framework;

    [TestFixture]
    public class EnumerationTableBindingsTestFixture
    {
        public class GoodDatabaseProvider : IEnumerationValueProvider
        {
            #region Implementation of IEnumerationValueProvider

            public IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings)
            {
                yield return new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value 1" };
                yield return new EnumerationValue() { Value = 2, Name = "Value2", DisplayName = "Value 2" };
                yield return new EnumerationValue() { Value = 3, Name = "Value3", DisplayName = "Value 3" };
                yield return new EnumerationValue() { Value = 4, Name = "Value4", DisplayName = "Value 4" };
                yield return new EnumerationValue() { Value = 5, Name = "Value5", DisplayName = "Value5" };
            }

            #endregion
        }

        public class BadCountDatabaseProvider : IEnumerationValueProvider
        {
            #region Implementation of IEnumerationValueProvider

            public IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings)
            {
                yield return new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value 1" };
                yield return new EnumerationValue() { Value = 2, Name = "Value2", DisplayName = "Value 2" };
                yield return new EnumerationValue() { Value = 4, Name = "Value4", DisplayName = "Value 4" };
            }

            #endregion
        }

        public class BadValueDatabaseProvider : IEnumerationValueProvider
        {
            #region Implementation of IEnumerationValueProvider

            public IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings)
            {
                yield return new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value 1" };
                yield return new EnumerationValue() { Value = 2, Name = "Value2", DisplayName = "Value 2" };
                yield return new EnumerationValue() { Value = 3, Name = "Value5", DisplayName = "Value 5" };
                yield return new EnumerationValue() { Value = 4, Name = "Value4", DisplayName = "Value 4" };
                yield return new EnumerationValue() { Value = 5, Name = "Value5", DisplayName = "Value5" };
            }

            #endregion
        }

        [Test]
        public void VerifyEnum_TestGoodComparison_ReturnsEmptyList()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new GoodDatabaseProvider());
            Assert.AreEqual(0, ev.VerifyEnumeration(typeof(TestVerifyEnum)).Count());
        }

        [Test]
        public void VerifyEnum_TestBadCountComparison_ReturnsSingleDifference()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new BadCountDatabaseProvider());
            Assert.AreEqual(1, ev.VerifyEnumeration(typeof(TestVerifyEnum)).Count());
        }

        [Test]
        public void VerifyEnum_TestBadValueComparison_ReturnsSingleDifference()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new BadValueDatabaseProvider());
            Assert.AreEqual(1, ev.VerifyEnumeration(typeof(TestVerifyEnum)).Count());
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPropertyBinding_AddDuplicateBinding_ThrowsException()
        {
            EnumerationTableBindingsAttribute att = new EnumerationTableBindingsAttribute("dbo", "TestCase");
            att.AddPropertyBinding("PropertyName", "ColumnName");
            att.AddPropertyBinding("PropertyName", "ColumnName");
            Assert.Fail("AddPropertyBinding should have thrown an InvalidOperationException");
        }
    }
}
