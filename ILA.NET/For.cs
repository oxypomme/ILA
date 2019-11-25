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

        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("pour ");
            Index.WriteILA(textWriter);
            textWriter.Write(" <- ");
            Start.WriteILA(textWriter);
            textWriter.Write(" a ");
            End.WriteILA(textWriter);
            if (!(Step is ConstantInt ci && ci.Value == 1))
            {
                textWriter.Write(" pas ");
                Step.WriteILA(textWriter);
            }
            textWriter.Write(" faire");
            if (Comment != null && Comment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(Comment);
            }
            textWriter.WriteLine();
            Program.ilaIndent++;
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            Program.ilaIndent--;
            Program.GenerateIndent(textWriter);
            textWriter.Write("fpour");
            if (EndComment != null && EndComment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(EndComment);
            }
            textWriter.WriteLine();
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}