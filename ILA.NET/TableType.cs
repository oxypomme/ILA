using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public struct Range
    {
        #region Public Fields

        public readonly int Max;
        public readonly int Min;

        #endregion Public Fields

        #region Public Constructors

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        #endregion Public Constructors
    }

    public class TableType : VarType
    {
        #region Public Properties

        public Range[] DimensionsSize { get; internal set; }
        public VarType InternalType { get; internal set; }

        #endregion Public Properties

        #region Protected Properties

        protected override string LuaCode => throw new NotImplementedException();

        protected override string PythonCode => throw new NotImplementedException();

        #endregion Protected Properties
    }
}