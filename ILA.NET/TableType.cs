using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Range : IBaseObject
    {
        #region Public Fields

        public readonly IValue Max;

        public readonly IValue Min;

        public Range(IValue min, IValue max)
        {
            Min = min;
            Max = max;
        }

        #endregion Public Fields

        #region Public Constructors

        public void WritePython(TextWriter textWriter)
        {
            Max.WritePython(textWriter);
            textWriter.Write(",");
            Min.WritePython(textWriter);
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
            textWriter.Write(Name);
        }

        #endregion Public Properties
    }
}