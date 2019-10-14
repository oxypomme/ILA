using System;
using System.Collections.Generic;
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

        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        VarType IValue.Type => Type;
        public int Value { get; internal set; }

        #endregion Public Properties
    }
}