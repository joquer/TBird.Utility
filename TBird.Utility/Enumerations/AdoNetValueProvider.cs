// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdoNetValueProvider.cs" company="Advisory Board Company - Crimson">
//   Copyright © 2014 Advisory Board Company - Crimson
// </copyright>
// <summary>
//   Defines the AdoNetValueProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Enumerations
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text;

    public class AdoNetValueProvider : IEnumerationValueProvider
    {
        private readonly DbConnection connection;

        private readonly bool hasSchemaSupport;

        public AdoNetValueProvider(DbConnection connection, bool supportsSchemas = true)
        {
            Contract.Requires(connection != null);
            Contract.Ensures(this.connection != null);
            Contract.Ensures(connection == this.connection);
            this.connection = connection;
            this.hasSchemaSupport = supportsSchemas;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "This is checked by code contracts for the Interface")]
        public IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings)
        {
            List<EnumerationValue> values = new List<EnumerationValue>();

            using (DbCommand command = this.connection.CreateCommand())
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(
                    this.hasSchemaSupport
                        ? string.Format(
                            CultureInfo.InvariantCulture,
                            "SELECT [{0}], [{1}], [{2}]",
                            bindings.KeyColumn,
                            bindings.NameColumn,
                            bindings.DisplayNameColumn)
                        : string.Format(
                            CultureInfo.InvariantCulture,
                            "SELECT {0}, {1}, {2}",
                            bindings.KeyColumn,
                            bindings.NameColumn,
                            bindings.DisplayNameColumn));

                foreach (string columnName in bindings.PropertyBindings.Values)
                {
                    sqlBuilder.AppendFormat(", {0}", columnName);
                }

                sqlBuilder.Append(
                    this.hasSchemaSupport
                        ? string.Format(CultureInfo.InvariantCulture, " FROM [{0}].[{1}]", bindings.SchemaName, bindings.TableName)
                        : string.Format(CultureInfo.InvariantCulture, " FROM {0}", bindings.TableName));

                command.CommandText = sqlBuilder.ToString();
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EnumerationValue value = new EnumerationValue()
                        {
                            Value = (int)reader[bindings.KeyColumn],
                            Name = (string)reader[bindings.NameColumn],
                            DisplayName = (string)reader[bindings.DisplayNameColumn],
                        };

                        foreach (string key in bindings.PropertyBindings.Keys)
                        {
                            value.PropertyValues[key] = reader[bindings.PropertyBindings[key]];
                        }

                        values.Add(value);
                    }
                }
            }

            return values;
        }
    }
}
