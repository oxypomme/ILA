using System;
using System.Collections.Generic;
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

        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        VarType IValue.Type => Type;
        public float Value { get; set; }

        #endregion Public Properties
    }
}