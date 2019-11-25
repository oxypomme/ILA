using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Intruction that calls an element of a table
    /// </summary>
    public class TableCall : Variable
    {
        #region Public Properties

        /// <summary>
        /// Index of the element
        /// </summary>
        public List<IValue> DimensionsIndex { get; set; }

        public override string Name { get => ""; set { } }

        /// <summary>
        /// Table to call from
        /// </summary>
        public Variable Table { get; set; }

        /// <summary>
        /// Type of the element
        /// </summary>
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
            base.WritePython(textWriter);
        }

        #endregion Public Properties
    }
}