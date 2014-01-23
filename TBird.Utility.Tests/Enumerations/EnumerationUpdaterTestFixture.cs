namespace TBird.Utility.Tests.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    using TBird.Utility.Enumerations;

    using NUnit.Framework;

    using TBird.Utility.Tests.Enumerations;

    [TestFixture]
    [Ignore]
    public class EnumerationUpdaterTestFixture
    {
        private readonly string connectionString;

        public EnumerationUpdaterTestFixture()
        {
            this.connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }

        /// <summary>
        /// This class is used to test error conditions before loading actually begins, so no
        /// actual update or insert routines are required.
        /// </summary>
        private class DummyUpdateProvider : IEnumerationUpdateProvider
        {
            public IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings)
            {
                return Enumerable.Empty<EnumerationValue>();
            }

            public void UpdateValue(EnumerationTableBindingsAttribute bindings, EnumerationValue value)
            {
            }

            public void InsertValue(EnumerationTableBindingsAttribute bindings, EnumerationValue value)
            {
            }

            public void Clear(EnumerationTableBindingsAttribute bindings)
            {
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_TestNoBindings_ThrowsExcpetion()
        {
            EnumerationUpdater updater = new EnumerationUpdater(new DummyUpdateProvider());
            updater.Update(typeof(TestEnums));
            Assert.Fail("The Update method should have thrown an InvalidOperationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_BadEnumClass_ThrowsExcpetion()
        {
            EnumerationUpdater updater = new EnumerationUpdater(new DummyUpdateProvider());
            updater.Update(typeof(BadEnumClass));
            Assert.Fail("The Update method should have thrown an InvalidOperationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_BadEnumClassNoGetValues_ThrowsExcpetion()
        {
            EnumerationUpdater updater = new EnumerationUpdater(new DummyUpdateProvider());
            updater.Update(typeof(BadEnumClassBadGetValues));
            Assert.Fail("The Update method should have thrown an InvalidOperationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_ClassWithNoTableBindings_ThrowsExcpetion()
        {
            EnumerationUpdater updater = new EnumerationUpdater(new DummyUpdateProvider());
            updater.Update(typeof(TestEnums));
            Assert.Fail("VerifyEnumeration should have thrown InvalidOperationException");
        }

        /// <summary>
        /// This unit test was used to develop the routines used by the other unit tests.  It also
        /// verifies that a complete round trip from Enum to database and back works correctly.
        /// </summary>
        [Test]
        public void Update_AllValuesMatch_NoDbChanges()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                EnumerationDatabaseHelper.ClearEnumTable(conn, typeof(UpdateDatabaseEnums));
                EnumerationDatabaseHelper.InsertEnumValues(conn, typeof(UpdateDatabaseEnums), UpdateDatabaseEnums.GetValues());

                EnumerationUpdater updater = new EnumerationUpdater(new AdoNetUpdateProvider(conn, false));
                UpdateStats stats = updater.Update(typeof(UpdateDatabaseEnums));
                EnumerationVerifier ev = new EnumerationVerifier(new AdoNetValueProvider(conn, false));
                IEnumerable<string> messages = ev.VerifyEnumeration(typeof(UpdateDatabaseEnums));
                Assert.AreEqual(0, messages.Count());
                Assert.AreEqual(0, stats.AddCount);
                Assert.AreEqual(0, stats.UpdateCount);
            }
        }

        [Test]
        public void Update_UpdateExistingEnum_AllItemsMatch()
        {
            EnumerationValue[] values = new EnumerationValue[]
                {
                    new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value1" },
                    new EnumerationValue() { Value = 2, Name = "Value 2", DisplayName = "This is Value 2" },
                    new EnumerationValue() { Value = 3, Name = "Value 3", DisplayName = "This is Value 3" },
                    new EnumerationValue() { Value = 4, Name = "Value 4", DisplayName = "This is Value 4" },
                };

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                EnumerationDatabaseHelper.ClearEnumTable(conn, typeof(UpdateDatabaseEnums));
                EnumerationDatabaseHelper.InsertEnumValues(conn, typeof(UpdateDatabaseEnums), values);
                EnumerationUpdater updater = new EnumerationUpdater(new AdoNetUpdateProvider(conn, false));
                UpdateStats stats = updater.Update(typeof(UpdateDatabaseEnums));
                EnumerationVerifier ev = new EnumerationVerifier(new AdoNetValueProvider(conn, false));
                IEnumerable<string> messages = ev.VerifyEnumeration(typeof(UpdateDatabaseEnums));
                Assert.AreEqual(0, messages.Count());
                Assert.AreEqual(0, stats.AddCount);
                Assert.AreEqual(3, stats.UpdateCount);
            }
        }

        [Test]
        public void Update_AddAllToEmptyTable_AllItemsMatch()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                EnumerationDatabaseHelper.ClearEnumTable(conn, typeof(UpdateDatabaseEnums));
                EnumerationUpdater updater = new EnumerationUpdater(new AdoNetUpdateProvider(conn, false));
                UpdateStats stats = updater.Update(typeof(UpdateDatabaseEnums));
                EnumerationVerifier ev = new EnumerationVerifier(new AdoNetValueProvider(conn, false));
                IEnumerable<string> messages = ev.VerifyEnumeration(typeof(UpdateDatabaseEnums));
                Assert.AreEqual(0, messages.Count());
                Assert.AreEqual(4, stats.AddCount);
                Assert.AreEqual(0, stats.UpdateCount);
            }
        }

        [Test]
        public void Update_AddNewEnum_AllItemsMatch()
        {
            EnumerationValue[] values = new EnumerationValue[]
                {
                    new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value1" },
                    new EnumerationValue() { Value = 2, Name = "Value2", DisplayName = "Value2" },
                };

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                EnumerationDatabaseHelper.ClearEnumTable(conn, typeof(UpdateDatabaseEnums));
                EnumerationDatabaseHelper.InsertEnumValues(conn, typeof(UpdateDatabaseEnums), values);
                EnumerationUpdater updater = new EnumerationUpdater(new AdoNetUpdateProvider(conn, false));
                UpdateStats stats = updater.Update(typeof(UpdateDatabaseEnums));
                EnumerationVerifier ev = new EnumerationVerifier(new AdoNetValueProvider(conn, false));
                IEnumerable<string> messages = ev.VerifyEnumeration(typeof(UpdateDatabaseEnums));
                Assert.AreEqual(0, messages.Count());
                Assert.AreEqual(2, stats.AddCount);
                Assert.AreEqual(0, stats.UpdateCount);
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_DatabaseHasExtraValue_ThrowsInvalidOperation()
        {
            EnumerationValue[] values = new EnumerationValue[]
                {
                    new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value1" },
                    new EnumerationValue() { Value = 2, Name = "Value2", DisplayName = "Value2" },
                    new EnumerationValue() { Value = 3, Name = "Value3", DisplayName = "Value3" },
                    new EnumerationValue() { Value = 4, Name = "Value4", DisplayName = "Value4" },
                    new EnumerationValue() { Value = 5, Name = "Value5", DisplayName = "Value5" },
                };

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                EnumerationDatabaseHelper.ClearEnumTable(conn, typeof(UpdateDatabaseEnums));
                EnumerationDatabaseHelper.InsertEnumValues(conn, typeof(UpdateDatabaseEnums), values);
                EnumerationUpdater updater = new EnumerationUpdater(new AdoNetUpdateProvider(conn, false));
                updater.Update(typeof(UpdateDatabaseEnums));
                Assert.Fail("Update sould have thrown an InvalidOperationException");
            }
        }

        [Test]
        public void Clear_LoadThenClearEnums_TableShouldbEmpty()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                Type testType = typeof(UpdateDatabaseEnums);
                EnumerationDatabaseHelper.ClearEnumTable(conn, testType);
                IEnumerationUpdateProvider provider = new AdoNetUpdateProvider(conn, false);
                EnumerationUpdater updater = new EnumerationUpdater(provider);
                UpdateStats stats = updater.Update(testType);
                IEnumerable<EnumerationValue> values = provider.GetValues(EnumerationTableBindingsAttribute.GetEnumBindings(testType));
                Assert.AreEqual(UpdateDatabaseEnums.Enumerations.Count(), values.Count());
                updater.Clear(testType);
                values = provider.GetValues(EnumerationTableBindingsAttribute.GetEnumBindings(testType));
                Assert.AreEqual(0, values.Count());
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Clear_TestNoBindings_ThrowsExcpetion()
        {
            EnumerationUpdater updater = new EnumerationUpdater(new DummyUpdateProvider());
            updater.Clear(typeof(TestEnums));
            Assert.Fail("The Update method should have thrown an InvalidOperationException");
        }
    }
}
