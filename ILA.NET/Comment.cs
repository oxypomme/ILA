using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Comment : Instruction
    {
        #region Public Properties

        string Instruction.Comment => Message;
        public string Message { get; set; }
        public bool MultiLine { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            if (MultiLine)
                textWriter.Write("/*");
            else
                textWriter.Write("//");
            textWriter.Write(Message);
            textWriter.WriteLine();
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}