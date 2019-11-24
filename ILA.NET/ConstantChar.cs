using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class ConstantChar : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        public ConstantChar()
        {
            Type = GenericType.Char;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type { get => Type; }
        public char Value { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write("'" + Value.ToString() + "'");
        }

        #endregion Public Properties
    }
}