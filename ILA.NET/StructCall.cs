using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class StructCall : Variable
    {
        #region Public Properties
        public override VarType Type { get => ((StructType)Struct.Type).Members[Name]; set => ((StructType)Struct.Type).Members[Name] = value; }
        internal override string LuaCode => throw new NotImplementedException();
        internal override string PythonCode => throw new NotImplementedException();
        public Variable Struct { get; set; }

        #endregion Public Properties
    }
}