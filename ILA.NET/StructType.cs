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
            Members = new Dictionary<string, VarType>();
        }

        #region Public Properties

        /// <summary>
        /// The members of the structure
        /// </summary>
        public Dictionary<string, VarType> Members { get; set; }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
            textWriter.Write(Name);
        }

        #endregion Public Properties
    }
}