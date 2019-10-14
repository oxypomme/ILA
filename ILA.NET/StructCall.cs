using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class StructCall : IValue
    {
        #region Public Properties

        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        public Variable Struct { get; internal set; }
        VarType IValue.Type => ((StructType)Struct.Type).Members[Value];
        public string Value { get; internal set; }

        #endregion Public Properties
    }
}