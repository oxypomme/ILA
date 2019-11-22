using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Return : Instruction
    {
        public IValue Type { get; set; }
        public string Comment { get; set; }

        string Instruction.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}
