using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// An enumeration type
    /// </summary>
    public class EnumType : VarType
    {
        #region Public Properties

        /// <summary>
        /// The available values of the enum
        /// </summary>
        public List<string> Values { get; set; }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}