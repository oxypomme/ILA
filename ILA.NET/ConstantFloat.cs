using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class ConstantFloat : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        public ConstantFloat()
        {
            Type = GenericType.Float;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type => Type;
        public float Value { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(Value.ToString(new System.Globalization.CultureInfo("en")));
        }

        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write(Value.ToString(new System.Globalization.CultureInfo("en")));
        }

        #endregion Public Properties
    }
}