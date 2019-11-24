using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class DoWhile : Instruction
    {
        #region Public Properties

        public IValue Condition { get; set; }
        public List<Instruction> Instructions { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            //x .generateIndent()
            textWriter.Write("while True :\n");
            foreach (var instruction in Instructions)
            {
                // ident++
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                // ident--
            }
            // ident++
            //x .generateIndent()
            textWriter.Write("if not (");
            Condition.WritePython(textWriter);
            textWriter.Write(") :\n");
            // ident++
            //x .generateIndent()
            textWriter.Write("break \n");
            // ident-=2
        }

        #endregion Public Properties
    }
}