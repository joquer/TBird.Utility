namespace TBird.Utility.Tests.Enumerations
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    using TBird.Utility.Enumerations;

    using NUnit.Framework;

    using TBird.Utility.Tests.Enumerations;

    [TestFixture]
    [Ignore]
    public class EnumerationPropertyUpdaterTestFixture
    {
        private readonly string connectionString;

        public EnumerationPropertyUpdaterTestFixture()
        {
            this.connectionString = ConfigurationManager.AppSettings["ConnectionString"];
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

                EnumerationDatabaseHelper.ClearEnumTable(conn, typeof(ExtraPropertyEnums));
                EnumerationDatabaseHelper.InsertEnumValues(conn, typeof(ExtraPropertyEnums), ExtraPropertyEnums.GetValues());

                EnumerationUpdater updater = new EnumerationUpdater(new AdoNetUpdateProvider(conn, false));
                UpdateStats stats = updater.Update(typeof(ExtraPropertyEnums));
                EnumerationVerifier ev = new EnumerationVerifier(new AdoNetValueProvider(conn, false));
                IEnumerable<string> messages = ev.VerifyEnumeration(typeof(ExtraPropertyEnums));
                Assert.AreEqual(0, messages.Count());
                Assert.AreEqual(0, stats.AddCount);
                Assert.AreEqual(0, stats.UpdateCount);
            }
        }

        [Test]
        public void Update_UpdateAgainstBlankTable_AddAllValues()
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                EnumerationDatabaseHelper.ClearEnumTable(conn, typeof(ExtraPropertyEnums));

                EnumerationUpdater updater = new EnumerationUpdater(new AdoNetUpdateProvider(conn, false));
                UpdateStats stats = updater.Update(typeof(ExtraPropertyEnums));
                EnumerationVerifier ev = new EnumerationVerifier(new AdoNetValueProvider(conn, false));
                IEnumerable<string> messages = ev.VerifyEnumeration(typeof(ExtraPropertyEnums));
                Assert.AreEqual(0, messages.Count());
                Assert.AreEqual(3, stats.AddCount);
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
            values[0].PropertyValues["IntValue"] = 1;
            values[0].PropertyValues["StringValue"] = "1";
            values[0].PropertyValues["SomeOtherValue"] = 1;
            values[1].PropertyValues["IntValue"] = 2;
            values[1].PropertyValues["StringValue"] = "2";
            values[1].PropertyValues["SomeOtherValue"] = 2;

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                EnumerationDatabaseHelper.ClearEnumTable(conn, typeof(ExtraPropertyEnums));
                EnumerationDatabaseHelper.InsertEnumValues(conn, typeof(ExtraPropertyEnums), values);
                EnumerationUpdater updater = new EnumerationUpdater(new AdoNetUpdateProvider(conn, false));
                UpdateStats stats = updater.Update(typeof(ExtraPropertyEnums));
                EnumerationVerifier ev = new EnumerationVerifier(new AdoNetValueProvider(conn, false));
                IEnumerable<string> messages = ev.VerifyEnumeration(typeof(ExtraPropertyEnums));
                Assert.AreEqual(0, messages.Count());
                Assert.AreEqual(1, stats.AddCount);
                Assert.AreEqual(0, stats.UpdateCount);
            }
        }

        [Test]
        public void Update_UpdateExistingEnum_AllItemsMatch()
        {
            EnumerationValue[] values = new EnumerationValue[]
                {
                    new EnumerationValue() { Value = 1, Name = "Value1", DisplayName = "Value1" },
                    new EnumerationValue() { Value = 2, Name = "Value2", DisplayName = "Value2" },
                    new EnumerationValue() { Value = 3, Name = "Value3", DisplayName = "This is Value 3" },
                };
            values[0].PropertyValues["IntValue"] = 1;
            values[0].PropertyValues["StringValue"] = "1";
            values[0].PropertyValues["SomeOtherValue"] = 1;
            values[1].PropertyValues["IntValue"] = 2;
            values[1].PropertyValues["StringValue"] = "2";
            values[1].PropertyValues["SomeOtherValue"] = 2;
            values[2].PropertyValues["IntValue"] = 5;
            values[2].PropertyValues["StringValue"] = "6";
            values[2].PropertyValues["SomeOtherValue"] = 7;

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                EnumerationDatabaseHelper.ClearEnumTable(conn, typeof(ExtraPropertyEnums));
                EnumerationDatabaseHelper.InsertEnumValues(conn, typeof(ExtraPropertyEnums), values);
                EnumerationUpdater updater = new EnumerationUpdater(new AdoNetUpdateProvider(conn, false));
                UpdateStats stats = updater.Update(typeof(ExtraPropertyEnums));
                EnumerationVerifier ev = new EnumerationVerifier(new AdoNetValueProvider(conn, false));
                IEnumerable<string> messages = ev.VerifyEnumeration(typeof(ExtraPropertyEnums));
                Assert.AreEqual(0, messages.Count());
                Assert.AreEqual(0, stats.AddCount);
                Assert.AreEqual(1, stats.UpdateCount);
            }
        }
    }
}
