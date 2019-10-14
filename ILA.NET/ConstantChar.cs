using System;
using System.Collections.Generic;
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

        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        VarType IValue.Type { get => Type; }
        public char Value { get; internal set; }

        #endregion Public Properties
    }
}