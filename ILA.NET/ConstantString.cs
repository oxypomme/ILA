using System;
using System.Collections.Generic;
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

        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        VarType IValue.Type => Type;

        public string Value { get; set; }

        #endregion Public Properties
    }
}