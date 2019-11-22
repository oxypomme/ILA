using System;
using System.Collections.Generic;
using System.IO;
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

        public List<Range> DimensionsSize { get; internal set; }
        public VarType InternalType { get; internal set; }

        public override void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties

        #region Protected Properties


        #endregion Protected Properties
    }
}