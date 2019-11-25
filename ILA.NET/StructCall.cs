using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class StructCall : Variable
    {
        #region Public Properties

        public Variable Struct { get; set; }
        public override VarType Type { get => ((StructType)Struct.Type).Members[Name]; set => ((StructType)Struct.Type).Members[Name] = value; }

        public override void WriteILA(TextWriter textWriter)
        {
            Struct.WriteILA(textWriter);
            textWriter.Write('.');
            textWriter.Write(Name);
        }

        public override void WritePython(TextWriter textWriter)
        {
            Struct.WritePython(textWriter);
            textWriter.Write('.');
            textWriter.Write(Name);
        }

        #endregion Public Properties
    }
}