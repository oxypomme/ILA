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
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}