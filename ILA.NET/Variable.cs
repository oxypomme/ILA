using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Variable : IValue
    {
        #region Public Properties

        public virtual bool Constant { get; set; }
        public virtual IValue ConstantValue { get; set; }
        string IBaseObject.LuaCode => LuaCode;
        public virtual string Name { get; set; }
        string IBaseObject.PythonCode => PythonCode;
        VarType IValue.Type => Type;

        #endregion Public Properties

        #region Internal Properties

        internal virtual string LuaCode => throw new NotImplementedException();
        internal virtual string PythonCode => throw new NotImplementedException();
        public virtual VarType Type { get; set; }

        #endregion Internal Properties
    }
}