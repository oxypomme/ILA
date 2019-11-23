using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class ConstantString : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        public ConstantString()
        {
            Type = GenericType.String;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type => Type;

        public string Value { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            _ = Value.ToString();
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}