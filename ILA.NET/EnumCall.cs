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

        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(Enum.Values[Index]);
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}