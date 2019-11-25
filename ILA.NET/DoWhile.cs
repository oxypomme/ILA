using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class DoWhile : Instruction
    {
        #region Public Properties

        public string Comment { get; set; }
        string Instruction.Comment => Comment;
        public IValue Condition { get; set; }
        public string EndComment { get; set; }
        public List<Instruction> Instructions { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("while True :\n");
            foreach (var instruction in Instructions)
            {
                Program.Indent++;
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                Program.Indent--;
            }
            Program.Indent++;
            Program.GenerateIndent(textWriter);
            textWriter.Write("if not (");
            Condition.WritePython(textWriter);
            textWriter.Write(") :\n");
            Program.Indent++;
            Program.GenerateIndent(textWriter);
            textWriter.Write("break \n");
            Program.Indent -= 2;
        }

        #endregion Public Properties
    }
}