using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class StructCall : Variable
    {
        #region Public Properties
        public override VarType Type { get => ((StructType)Struct.Type).Members[Name]; set => ((StructType)Struct.Type).Members[Name] = value; }
        public Variable Struct { get; set; }

        public override void WritePython(TextWriter textWriter)
        {
            base.WritePython(textWriter);
        }

        #endregion Public Properties
    }
}