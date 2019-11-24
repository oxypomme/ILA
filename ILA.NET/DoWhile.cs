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
        public List<Instruction> Instructions { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.WriteLine("repeter");
            Program.ilaIndent++;
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            Program.ilaIndent--;
            Program.GenerateIndent(textWriter);
            textWriter.Write("jusqua ");
            Condition.WriteILA(textWriter);
            textWriter.WriteLine();
        }
        public string EndComment { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}