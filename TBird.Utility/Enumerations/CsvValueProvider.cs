// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvValueProvider.cs" company="Advisory Board Company - Crimson">
//   Copyright © 2014 Advisory Board Company - Crimson
// </copyright>
// <summary>
//   Defines the CsvValueProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class CsvValueProvider : IEnumerationValueProvider
    {
        #region Implementation of IEnumerationValueProvider

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "This is checked by code contracts for the Interface")]
        public IEnumerable<EnumerationValue> GetValues(EnumerationTableBindingsAttribute bindings)
        {
            List<EnumerationValue> values = new List<EnumerationValue>();
            string fileName = Path.Combine("csv", string.Format(CultureInfo.InvariantCulture, "{0}.{1}.csv", bindings.SchemaName, bindings.TableName));
            using (StreamReader reader = File.OpenText(fileName))
            {
                int lineNumber = 1;
                string line;
                List<string> extraColumns = new List<string>();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');
                    if (lineNumber == 1)
                    {
                        if (bindings.KeyColumn != tokens[0] || bindings.NameColumn != tokens[1]
                            || bindings.DisplayNameColumn != tokens[2])
                        {
                            throw new InvalidOperationException(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0}: Column Names and Header Names do not match: File - {1}, {2}, {3} - Bindings - {4}, {5}, {6}",
                                    fileName,
                                    tokens[0],
                                    tokens[1],
                                    tokens[2],
                                    bindings.KeyColumn,
                                    bindings.NameColumn,
                                    bindings.DisplayNameColumn));
                        }

                        if (bindings.PropertyBindings.Count > 0)
                        {
                            if (bindings.PropertyBindings.Count != tokens.Count() - 3)
                            {
                                throw new InvalidOperationException(
                                    string.Format(
                                        "All extra properties must me present in File Header, File has {0} expected {1}",
                                        tokens.Count() - 3,
                                        bindings.PropertyBindings.Count));
                            }
                        }

                        // This is a bit strange because the dictionary has the property name as the key and the file
                        // header has column name.  So, first verify that the column name exists in the values, then
                        // go backwards to find the key that matches this column.
                        for (int i = 3; i < tokens.Count(); i++)
                        {
                            string extraHeaderText = bindings.PropertyBindings.Values.FirstOrDefault(x => x == tokens[i]);
                            if (extraHeaderText == null)
                            {
                                throw new InvalidOperationException(string.Format("Column {0} from file not found in property bindings", tokens[i]));
                            }

                            extraColumns.Add(bindings.PropertyBindings.Keys.First(x => bindings.PropertyBindings[x] == extraHeaderText));
                        }
                    }
                    else
                    {
                        if (tokens.Count() != extraColumns.Count + 3)
                        {
                            throw new InvalidOperationException(
                                string.Format(
                                    CultureInfo.InvariantCulture, "{0}({1}): CSV file header must contain {2} column names", fileName, lineNumber, extraColumns.Count + 3));
                        }

                        EnumerationValue enumValue = new EnumerationValue()
                        {
                            Value = int.Parse(tokens[0], CultureInfo.InvariantCulture),
                            Name = tokens[1],
                            DisplayName = tokens[2],
                        };
                        for (int i = 0; i < extraColumns.Count; i++)
                        {
                            enumValue.PropertyValues[extraColumns[i]] = tokens[2 + i];
                        }

                        values.Add(enumValue);
                    }

                    lineNumber++;
                }

                return values;
            }
        }

        #endregion
    }
}