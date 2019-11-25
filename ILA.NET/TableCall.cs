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

        /// <summary>
        /// Returns nothing
        /// </summary>
        public override string Name { get => ""; set { } }

        /// <summary>
        /// Table to call from
        /// </summary>
        public Variable Table { get; set; }

        /// <summary>
        /// Type of the element
        /// </summary>
        public override VarType Type { get => ((TableType)Table.Type).InternalType; set => ((TableType)Table.Type).InternalType = value; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
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

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
            base.WritePython(textWriter);
        }

        #endregion Public Properties
    }
}