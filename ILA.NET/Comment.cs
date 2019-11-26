using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A comment block.
    /// </summary>
    public class Comment : Instruction
    {
        #region Public Properties

        string Instruction.Comment => Message;

        /// <summary>
        /// The description of the comment
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// True if the comment is multiline (using /* */)
        /// </summary>
        public bool MultiLine { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            if (MultiLine)
                textWriter.Write("/*");
            else
                textWriter.Write("//");
            textWriter.Write(Message);
            if (MultiLine)
                textWriter.Write("*/");
            textWriter.WriteLine();
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
        }

        #endregion Public Properties
    }
}