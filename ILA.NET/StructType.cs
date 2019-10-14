using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class StructType : VarType
    {
        #region Public Properties

        public VarType[] Members { get; internal set; }

        #endregion Public Properties

        #region Protected Properties

        protected override string LuaCode => throw new NotImplementedException();

        protected override string PythonCode => throw new NotImplementedException();

        #endregion Protected Properties
    }
}