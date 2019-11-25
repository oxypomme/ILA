using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A hard coded char
    /// </summary>
    public class ConstantChar : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ConstantChar()
        {
            Type = GenericType.Char;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type { get => Type; }

        /// <summary>
        /// Value of the char
        /// </summary>
        public char Value { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write('\'');
            textWriter.Write(Value);
            textWriter.Write('\'');
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}