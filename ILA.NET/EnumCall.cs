using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Instruction that calls an enum value
    /// </summary>
    public class EnumCall : IValue
    {
        #region Public Properties

        /// <summary>
        /// Type of the enum to call
        /// </summary>
        public EnumType Enum { get; set; }

        /// <summary>
        /// the index corresponding to the value
        /// </summary>
        public int Index { get; set; }

        VarType IValue.Type => Enum;

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(Enum.Values[Index]);
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write(Index);
        }

        #endregion Public Properties
    }
}