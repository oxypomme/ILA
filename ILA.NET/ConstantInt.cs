using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class ConstantInt : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        public ConstantInt()
        {
            Type = GenericType.Int;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type => Type;
        public int Value { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}