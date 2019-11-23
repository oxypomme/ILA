using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Range : IBaseObject
    {
        #region Public Fields
        public readonly VarType Type;

        public Range(int min, int max, VarType type)
        {
            Min = min;
            Max = max;
            Type = type;
        }

        public readonly int Max;
        public readonly int Min;

        #endregion Public Fields

        #region Public Constructors

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
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