using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class TableCall : Variable
    {
        #region Public Properties

        public List<IValue> DimensionsIndex { get; set; }
        public override string Name { get => ""; set { } }
        public Variable Table { get; set; }
        public override VarType Type { get => ((TableType)Table.Type).InternalType; set => ((TableType)Table.Type).InternalType = value; }

        public override void WriteILA(TextWriter textWriter)
        {
            Table.WriteILA(textWriter);
            textWriter.Write('[');
            for (int i = 0; i < DimensionsIndex.Count; i++)
            {
                if (i > 0)
                    textWriter.Write(", ");
                DimensionsIndex[i].WriteILA(textWriter);
            }
            textWriter.Write(']');
        }

        public override void WritePython(TextWriter textWriter)
        {
            Table.WritePython(textWriter);
            textWriter.Write('[');
            for (int i = 0; i < DimensionsIndex.Count; i++)
            {
                if (i > 0)
                    textWriter.Write("][");
                DimensionsIndex[i].WritePython(textWriter);
                textWriter.Write("-1");
            }
            textWriter.Write(']');
        }

        #endregion Public Properties
    }
}