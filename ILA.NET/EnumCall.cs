using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class EnumCall : IValue
    {
        #region Public Properties

        public EnumType Enum { get; set; }
        public int Index { get; set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        VarType IValue.Type => Enum;

        #endregion Public Properties
    }
}