using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Comment : Instruction
    {
        #region Public Properties


        public bool MultiLine { get; set; }

        public string Message { get; set; }
        string Instruction.Comment => Message;

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}
