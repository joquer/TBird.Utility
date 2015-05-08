// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdoNetUpdateProvider.cs" company="Advisory Board Company - Crimson">
//   Copyright © 2014 Advisory Board Company - Crimson
// </copyright>
// <summary>
//   Defines the AdoNetUpdateProvider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Text;

    public class AdoNetUpdateProvider : IEnumerationUpdateProvider
    {
        private readonly DbConnection connection;

        private readonly bool hasSchemaSupport;

        public AdoNetUpdateProvider(DbConnection connection, bool supportsSchemas = true)
        {
            Contract.Requires(connection != null);
            Contract.Ensures(connection != null);
            Contract.Ensures(this.connection == connection);
            this.connection = connection;
            this.hasSchemaSupport = supportsSchemas;
        }

        #region Implementation of IEnumerationUpdateProvider

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "This is checked by code contracts for the Interface")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "This is checked by code contracts for the Interface")]
        public void UpdateValue(EnumerationTableBindingsAttribute bindings, EnumerationValue value)
        {
            using (DbCommand command = this.connection.CreateCommand())
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(
                    this.hasSchemaSupport
                        ? string.Format(CultureInfo.InvariantCulture, "UPDATE [{0}].[{1}]", bindings.SchemaName, bindings.TableName)
                        : string.Format(CultureInfo.InvariantCulture, "UPDATE {0}", bindings.TableName));
                sqlBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    " SET {0} = @Name, {1} = @DisplayName",
                    bindings.NameColumn,
                    bindings.DisplayNameColumn);

                foreach (string propertyName in value.PropertyValues.Keys)
                {
                    sqlBuilder.AppendFormat(", {0} = @{1}", bindings.PropertyBindings[propertyName], propertyName);
                }

                sqlBuilder.AppendFormat(" WHERE {0} = {1}", bindings.KeyColumn, value.Value);
                command.CommandText = sqlBuilder.ToString();
                command.Parameters.Add(new SqlParameter("@Name", value.Name));
                command.Parameters.Add(new SqlParameter("@DisplayName", value.DisplayName));
                foreach (string propertyName in value.PropertyValues.Keys)
                {
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", propertyName), value.PropertyValues[propertyName]));
                }

                command.ExecuteNonQuery();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "This is checked by code contracts for the Interface")]
        public void InsertValue(EnumerationTableBindingsAttribute bindings, EnumerationValue value)
        {
            using (DbCommand command = this.connection.CreateCommand())
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.Append(
                    this.hasSchemaSupport
                        ? string.Format(
                            CultureInfo.InvariantCulture,
                            "INSERT INTO [{0}].[{1}] ([{2}], [{3}], [{4}]",
                            bindings.SchemaName,
                            bindings.TableName,
                            bindings.KeyColumn,
                            bindings.NameColumn,
                            bindings.DisplayNameColumn)
                        : string.Format(
                            CultureInfo.InvariantCulture,
                            "INSERT INTO {0} ({1}, {2}, {3}",
                            bindings.TableName,
                            bindings.KeyColumn,
                            bindings.NameColumn,
                            bindings.DisplayNameColumn));

                foreach (string propertyName in value.PropertyValues.Keys)
                {
                    sqlBuilder.AppendFormat(", {0}", bindings.PropertyBindings[propertyName]);
                }

                sqlBuilder.Append(") VALUES (@Value, @Name, @DisplayName");
                foreach (string propertyName in value.PropertyValues.Keys)
                {
                    sqlBuilder.AppendFormat(", @{0}", propertyName);
                }

                sqlBuilder.Append(")");

                command.CommandText = sqlBuilder.ToString();
                command.Parameters.Add(new SqlParameter("@Value", value.Value));
                command.Parameters.Add(new SqlParameter("@Name", value.Name));
                command.Parameters.Add(new SqlParameter("@DisplayName", value.DisplayName));
                foreach (string propertyName in value.PropertyValues.Keys)
                {
                    command.Parameters.Add(new SqlParameter(string.Format("@{0}", propertyName), value.PropertyValues[propertyName]));
                }

                command.ExecuteNonQuery();
            }
        }

        public void Clear(EnumerationTableBindingsAttribute bindings)
        {
            Contract.Requires(bindings != null);
            using (DbCommand command = this.connection.CreateCommand())
            {
                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.AppendFormat(
                    this.hasSchemaSupport
                        ? string.Format(CultureInfo.InvariantCulture, "DELETE [{0}].[{1}]", bindings.SchemaName, bindings.TableName)
                        : string.Format(CultureInfo.InvariantCulture, "DELETE {0}", bindings.TableName));
                command.CommandText = sqlBuilder.ToString();
                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
