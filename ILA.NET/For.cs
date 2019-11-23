using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class For : Instruction
    {
        #region Public Properties

        public IValue End { get; set; }
        public Variable Index { get; set; }
        public List<Instruction> Instructions { get; set; }

        public IValue Start { get; set; }
        public IValue Step { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        #endregion Public Properties

        public void WritePython(TextWriter textWriter)
        {
            // Index initialiser
            //x .generateIndent()
            Index.WritePython(textWriter);
            textWriter.Write(" = ");
            Start.WritePython(textWriter);

            // While condition
            //x .generateIndent()
            textWriter.Write("\n" +
                "while (");
            Index.WritePython(textWriter);
            textWriter.Write(" != ");
            Step.WritePython(textWriter);
            textWriter.Write(") :\n");

            // While content
            foreach (Instruction instruction in Instructions)
            {
                // ident++
                //x .generateIndent()
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                // ident--
            }
        }
    }
}