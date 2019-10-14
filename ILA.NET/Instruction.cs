using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public abstract class Instruction : IBaseObject
    {
        #region Protected Properties

        string IBaseObject.LuaCode => LuaCode;
        string IBaseObject.PythonCode => PythonCode;

        #endregion Protected Properties

        #region Public Properties

        protected abstract string PythonCode { get; }
        protected abstract string LuaCode { get; }

        #endregion Public Properties
    }
}