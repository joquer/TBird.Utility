// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILoadableObject.cs" company="Mr Smarti Pantz">
//   Copyright 2011 Mr Smarti Pantz LLC
// </copyright>
// <summary>
//   An object that is serializable to/from XML.  This works with TypeUtils.CompareObject to allow comparison
//   of objects for unit testing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TBird.Utility.Mvvm
{
    using System.Xml.Linq;

    /// <summary>
    /// An object that is serializable to/from XML.  This works with TypeUtils.CompareObject to allow comparison
    /// of objects for unit testing.
    /// </summary>
    public interface ILoadableObject
    {
        /// <summary>
        /// Serialize the object into an XML Document.
        /// </summary>
        /// <param name="ns">The namespace of the document.</param>
        /// <returns>An XElement that holds an XML document representing the object.</returns>
        XElement Save(XNamespace ns);

        /// <summary>
        /// Deserialize the object from an XML document.
        /// </summary>
        /// <param name="el">An XElement containing an XML document representing this object.</param>
        void Load(XElement el);
    }
}
