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

        public void WritePython(TextWriter textWriter)
        {
            //x .generateIdent()
            textWriter.Write("return ");
            Value.WritePython(textWriter);
            textWriter.Write("\n");
        }
    }
}