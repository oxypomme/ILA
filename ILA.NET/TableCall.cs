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
        /// <summary>
        /// Constructor
        /// </summary>
        public TableCall() : base()
        {
            DimensionsIndex = new List<IValue>();
            Table = new Variable();
        }

        #region Public Properties

        /// <summary>
        /// Index of the element
        /// </summary>
        public List<IValue> DimensionsIndex { get; set; }

        /// <summary>
        /// Not supported on a table call
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.Bindable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override string Name
        {
            get => null;
            set { }
        }

        /// <summary>
        /// Table to call from
        /// </summary>
        public Variable Table { get; set; }

        /// <summary>
        /// Type of the element. read only
        /// </summary>
        public override VarType Type { get => ((TableType)Table.Type).InternalType; set { } }

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