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
            //x .generateIndent()
            textWriter.Write("while True :\n");
            foreach (var instruction in Instructions)
            {
                Program.Indent++;
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                Program.Indent--;
            }
            Program.Indent++;
            //x .generateIndent()
            textWriter.Write("if not (");
            Condition.WritePython(textWriter);
            textWriter.Write(") :\n");
            Program.Indent++;
            //x .generateIndent()
            textWriter.Write("break \n");
            // ident-=2
        }

        #endregion Public Properties
    }
}