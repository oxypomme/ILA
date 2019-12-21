using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A custom struct type
    /// </summary>
    public class StructType : VarType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StructType()
        {
            Members = new SortedDictionary<string, VarType>();
        }

        #region Public Properties

        /// <summary>
        /// The members of the structure
        /// </summary>
        public SortedDictionary<string, VarType> Members { get; set; }

        /// <summary>
        /// Name of the type
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(Name);
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write(Name);
        }

        #endregion Public Properties
    }
}