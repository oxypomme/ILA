using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class TableCall : Variable
    {
        #region Public Properties

        public List<int> DimensionsIndex { get; set; }
        internal override string LuaCode => throw new NotImplementedException();
        internal override string PythonCode => throw new NotImplementedException();
        public Variable Table { get; set; }
        public override VarType Type { get => ((TableType)Table.Type).InternalType; set => ((TableType)Table.Type).InternalType = value; }

        #endregion Public Properties
    }
}