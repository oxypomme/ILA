using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public abstract class VarType : IBaseObject
    {
        #region Protected Properties

        string IBaseObject.PythonCode => PythonCode;
        string IBaseObject.LuaCode => LuaCode;

        #endregion Protected Properties

        #region Public Properties

        public virtual string Name { get; set; }
        protected abstract string LuaCode { get; }
        protected abstract string PythonCode { get; }

        #endregion Public Properties
    }
}