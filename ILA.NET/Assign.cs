using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Assign : Instruction
    {
        #region Public Properties

        public string Comment { get; set; }
        string Instruction.Comment => Comment;
        public Variable Left { get; set; }
        public IValue Right { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        public void WritePython(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            Left.WritePython(textWriter);
            textWriter.Write(" <- ");
            Right.WritePython(textWriter);
            if (Comment != null && Comment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(Comment);
            }
            textWriter.WriteLine();
        }

        #endregion Public Properties
    }
}