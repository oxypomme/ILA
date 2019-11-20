using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Function : Module
    {
        #region Public Properties

        public VarType ReturnType { get; set; }

        #endregion Public Properties

        #region Internal Properties

        internal override string LuaCode => base.LuaCode;

        internal override string PythonCode => base.PythonCode;

        #endregion Internal Properties
    }
}