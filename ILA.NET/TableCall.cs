using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class TableCall : Variable
    {
        #region Public Properties

        public List<int> DimensionsIndex { get; set; }
        public Variable Table { get; set; }
        public override string Name { get => ""; set { } }
        public override VarType Type { get => ((TableType)Table.Type).InternalType; set => ((TableType)Table.Type).InternalType = value; }
        public override void WritePython(TextWriter textWriter)
        {
            base.WritePython(textWriter);
        }

        #endregion Public Properties
    }
}