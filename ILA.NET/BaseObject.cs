using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public abstract class BaseObject
    {
        #region Protected Properties

        protected abstract string LuaCode { get; }
        protected abstract string PythonCode { get; }

        #endregion Protected Properties
    }
}