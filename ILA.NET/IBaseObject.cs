using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IBaseObject
    {
        #region Protected Internal Properties

        protected internal string LuaCode { get; }
        protected internal string PythonCode { get; }

        #endregion Protected Internal Properties
    }
}