namespace TBird.Utility.Tests.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TBird.Utility.Enumerations;
    using TBird.Utility.Tests;

    using NUnit.Framework;

    [TestFixture]
    public class EnumerationVerificationTest
    {
        private class DummyVerifyProvider : IEnumerationValueProvider
        {
            public IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings)
            {
                return Enumerable.Empty<EnumerationValue>();
            }
        }

        [Test]
        public void VerifyEnums_VerifyDefinitionsVersusCSVFiles_AllMatch()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new CsvValueProvider());
            List<string> differences = new List<string>();
            differences.AddRange(ev.VerifyEnumeration(typeof(TestVerifyEnum)));
            Assert.AreEqual(0, differences.Count(), TestUtils.FlattenMessages(differences));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_TestNoBindings_ThrowsExcpetion()
        {
            EnumerationVerifier verifier = new EnumerationVerifier(new DummyVerifyProvider());
            verifier.VerifyEnumeration(typeof(TestEnums));
            Assert.Fail("VerifyEnumeration should have thrown InvalidOperationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_BadEnumClass_ThrowsExcpetion()
        {
            EnumerationVerifier verifier = new EnumerationVerifier(new DummyVerifyProvider());
            verifier.VerifyEnumeration(typeof(BadEnumClass));
            Assert.Fail("VerifyEnumeration should have thrown InvalidOperationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_BadGetValuesInEnum_ThrowsExcpetion()
        {
            EnumerationVerifier verifier = new EnumerationVerifier(new DummyVerifyProvider());
            verifier.VerifyEnumeration(typeof(BadEnumClassBadGetValues));
            Assert.Fail("VerifyEnumeration should have thrown InvalidOperationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_ClassWithNoTableBindings_ThrowsExcpetion()
        {
            EnumerationVerifier verifier = new EnumerationVerifier(new DummyVerifyProvider());
            IEnumerable<string> differences = verifier.VerifyEnumeration(typeof(TestEnums));
            Assert.Fail("VerifyEnumeration should have thrown InvalidOperationException");
        }

        [Test]
        public void Constructor_SetsKeyColumnName()
        {
            EnumerationTableBindingsAttribute standardAtt = new EnumerationTableBindingsAttribute("dbo", "TestEnums");
            Assert.AreEqual("TestEnumKey", standardAtt.KeyColumn);
            EnumerationTableBindingsAttribute iesAtt = new EnumerationTableBindingsAttribute("dbo", "Categories");
            Assert.AreEqual("CategoryKey", iesAtt.KeyColumn);
            EnumerationTableBindingsAttribute singularAtt = new EnumerationTableBindingsAttribute("dbo", "Category");
            Assert.AreEqual("CategoryKey", singularAtt.KeyColumn);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VerifyEnums_BadHeaderCsvFile_ThrowsException()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new CsvValueProvider());
            List<string> differences = new List<string>();
            differences.AddRange(ev.VerifyEnumeration(typeof(BadHeaderCsvEnums)));
            Assert.Fail("VerifyEnumeration should have thrown InvalidOperationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VerifyEnums_BadLineFormatCsvFile_ThrowsException()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new CsvValueProvider());
            List<string> differences = new List<string>();
            differences.AddRange(ev.VerifyEnumeration(typeof(BadLineFormatCsvEnums)));
            Assert.Fail("VerifyEnumeration should have thrown InvalidOperationException");
        }

        // Positive test case, tests that it reads in csv values and everything matches.
        [Test]
        public void VerifyExtraProperties_VerifyExtraProperties_AllMatch()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new CsvValueProvider());
            List<string> differences = new List<string>();
            differences.AddRange(ev.VerifyEnumeration(typeof(ExtraPropertyEnums)));
            Assert.AreEqual(0, differences.Count(), TestUtils.FlattenMessages(differences));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VerifyExtraProperties_BadFileHeader_ThrowsException()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new CsvValueProvider());
            ev.VerifyEnumeration(typeof(FirstBadExtraPropertyEnums));
            Assert.Fail("Test should have thrown an InvalidOperationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VerifyExtraProperties_BadDataLine_ThrowsException()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new CsvValueProvider());
            ev.VerifyEnumeration(typeof(SecondBadExtraPropertyEnums));
            Assert.Fail("Test should have thrown an InvalidOperationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VerifyExtraProperties_BadHeaderLineNotEnoughColumns_ThrowsException()
        {
            EnumerationVerifier ev = new EnumerationVerifier(new CsvValueProvider());
            ev.VerifyEnumeration(typeof(ThirdBadExtraPropertyEnums));
            Assert.Fail("Test should have thrown an InvalidOperationException");
        }
    }
}
