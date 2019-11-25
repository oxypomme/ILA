using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class For : Instruction
    {
        #region Public Properties

        public string Comment { get; set; }
        string Instruction.Comment => Comment;
        public IValue End { get; set; }
        public string EndComment { get; set; }
        public Variable Index { get; set; }
        public List<Instruction> Instructions { get; set; }

        public IValue Start { get; set; }
        public IValue Step { get; set; }

        #endregion Public Properties

        public void WritePython(TextWriter textWriter)
        {
            // Index initialiser
            Program.GenerateIndent(textWriter);
            textWriter.Write("if True :\n");
            Program.Indent++;
            Program.GenerateIndent(textWriter);
            Index.WritePython(textWriter);
            textWriter.Write(" = ");
            Start.WritePython(textWriter);
            textWriter.Write("\n");

            // While condition
            Program.GenerateIndent(textWriter);
            textWriter.Write("while (");
            Index.WritePython(textWriter);
            textWriter.Write(" != ");
            Step.WritePython(textWriter);
            textWriter.Write(") :\n");

            // While content
            foreach (Instruction instruction in Instructions)
            {
                Program.Indent++;
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                Program.Indent--;
            }
            Program.Indent++;
            Program.GenerateIndent(textWriter);
            Index.WritePython(textWriter);
            // TODO : avec un pas négatif !
            textWriter.Write("+=");
            Step.WritePython(textWriter);
            Program.Indent -= 2;
        }
    }
}