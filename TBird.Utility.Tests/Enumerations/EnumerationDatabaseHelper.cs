namespace TBird.Utility.Tests.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;

    using TBird.Utility.Enumerations;

    public class EnumerationDatabaseHelper
    {
        public static void ClearEnumTable(DbConnection conn, Type enumType)
        {
            EnumerationTableBindingsAttribute bindings = EnumerationTableBindingsAttribute.GetEnumBindings(enumType);
            DbCommand command = conn.CreateCommand();
            command.CommandText = string.Format("DELETE FROM {0}", bindings.TableName);
            command.ExecuteNonQuery();
        }

        public static void InsertEnumValues(DbConnection conn, Type enumType, IEnumerable<EnumerationValue> values)
        {
            EnumerationTableBindingsAttribute bindings = EnumerationTableBindingsAttribute.GetEnumBindings(enumType);

            foreach (EnumerationValue value in values)
            {
                using (DbCommand command = conn.CreateCommand())
                {
                    StringBuilder sqlBuilder = new StringBuilder();
                    sqlBuilder.AppendFormat(
                        "INSERT INTO {0} ({1}, {2}, {3}",
                        bindings.TableName,
                        bindings.KeyColumn,
                        bindings.NameColumn,
                        bindings.DisplayNameColumn);

                    foreach (string propertyName in value.PropertyValues.Keys)
                    {
                        sqlBuilder.AppendFormat(", {0}", bindings.PropertyBindings[propertyName]);
                    }

                    sqlBuilder.Append(") VALUES (@Value, @Name, @DisplayName");
                    foreach (string propertyName in value.PropertyValues.Keys)
                    {
                        sqlBuilder.AppendFormat(", @{0}", propertyName);
                    }

                    sqlBuilder.AppendFormat(")");

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
        }
    }
}
