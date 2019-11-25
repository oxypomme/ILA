using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Assign : Instruction
    {
        #region Public Properties

        public Variable Left { get; set; }
        public IValue Right { get; set; }
        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            Left.WritePython(textWriter);
            textWriter.Write(" = ");
            Right.WritePython(textWriter);
        }

        #endregion Public Properties
    }
}