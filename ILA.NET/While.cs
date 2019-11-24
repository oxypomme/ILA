using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class While : Instruction
    {
        #region Public Properties

        public IValue Condition { get; set; }
        public List<Instruction> Instructions { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            //x .generateIdent()
            textWriter.Write("while (");
            Condition.WritePython(textWriter);
            textWriter.Write(") :\n");
            foreach (var instruction in Instructions)
            {
                // ident++
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                // ident--
            }
        }

        #endregion Public Properties
    }
}