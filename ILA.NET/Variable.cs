using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Variable : IValue
    {
        #region Public Properties

        public bool Constant { get; internal set; }
        public IValue ConstantValue { get; internal set; }
        string IBaseObject.LuaCode => LuaCode;
        public string Name { get; internal set; }
        string IBaseObject.PythonCode => PythonCode;
        VarType IValue.Type => Type;

        #endregion Public Properties

        #region Internal Properties

        internal string LuaCode => throw new NotImplementedException();
        internal string PythonCode => throw new NotImplementedException();
        internal VarType Type { get; set; }

        #endregion Internal Properties
    }
}