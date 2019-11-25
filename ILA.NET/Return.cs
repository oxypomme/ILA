using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Return : Instruction
    {
        public IValue Value { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;
        public Function Function { get; set; }
        public IValue Type { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write(Function.Name);
            textWriter.Write(" <- ");
            Type.WriteILA(textWriter);
            if (Comment != null && Comment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(Comment);
            }
            textWriter.WriteLine();
        }

        public void WritePython(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("return ");
            Value.WritePython(textWriter);
            textWriter.Write("\n");
        }
    }
}