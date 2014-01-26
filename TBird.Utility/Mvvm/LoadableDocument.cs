// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadableDocument.cs" company="Mr Smarti Pantz">
//   Copyright 2011 Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Mvvm
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Xml.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LoadableDocument
    {
        /// <summary>
        /// Writes the instance of the loadable object to the stream with the given namespace.
        /// If the file exists, it is deleted, then the new file is written.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="data">The data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Code Contracts validate the arguments")]
        public static void Write(Stream stream, string namespaceName, ILoadableObject data)
        {
            Contract.Requires(stream != null);
            Contract.Requires(data != null);
            XNamespace ns = namespaceName;

            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), data.Save(ns));
            doc.Save(stream);
        }

        /// <summary>
        /// Deserializes an ILoadableObject from a stream.
        /// </summary>
        /// <param name="stream">The stream for the XML document.</param>
        /// <param name="data">The instance whose data will be read from the XML stream.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "Arguments are validated by the Code Contracts.")]
        public static void Read(Stream stream, ILoadableObject data)
        {
            Contract.Requires(stream != null);
            Contract.Requires(data != null);
            XDocument doc = XDocument.Load(stream);
            data.Load(doc.Root);
        }
    }
}
