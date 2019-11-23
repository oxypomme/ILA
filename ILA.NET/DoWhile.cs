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
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}