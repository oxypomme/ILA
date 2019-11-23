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
            var indexList = new List<string>();

            // var baseIdent = ident
            //x .generateIdent()
            InternalType.WritePython(textWriter);
            textWriter.Write(" = []\n");

            //x .generateIdent()
            for (int i = 0; i <= DimensionsSize.Count; i++)
            {
                //x .generateIdent()
                indexList.Add("index" + i);
                textWriter.Write("for " + indexList[i] + " in range(");
                DimensionsSize[i].WritePython(textWriter);
                textWriter.Write("):\n");
                // ident++
                //x .generateIdent()
                InternalType.WritePython(textWriter);
                if (i < DimensionsSize.Count && i > 0)
                {
                    for (int j = 0; j < i; j++)
                        textWriter.Write("[" + indexList[i - j] + "]");
                    textWriter.Write(")\n");
                }
                textWriter.Write(".append(");
                if (i < DimensionsSize.Count)
                    textWriter.Write("[]");
                else
                    textWriter.Write(0);
                textWriter.Write(")\n");
            }
            // ident = baseIdent
        }

        #endregion Public Properties
    }
}