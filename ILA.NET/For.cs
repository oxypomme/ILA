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

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}