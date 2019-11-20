using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class TableCall : IValue
    {
        #region Public Properties

        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        public Variable Table { get; internal set; }
        VarType IValue.Type => ((TableType)Table.Type).InternalType;
        public List<int> DimensionsIndex { get; set; }

        #endregion Public Properties
    }
}