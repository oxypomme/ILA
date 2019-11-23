using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// For loop block
    /// </summary>
    public class For : Instruction
    {
        #region Public Properties

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string Comment { get; set; }

        string Instruction.Comment => Comment;

        /// <summary>
        /// Ending value of the index
        /// </summary>
        public IValue End { get; set; }

        /// <summary>
        /// Variable to change
        /// </summary>
        public Variable Index { get; set; }

        /// <summary>
        /// Block of instructions
        /// </summary>
        public List<Instruction> Instructions { get; set; }

        /// <summary>
        /// Starting value of the index
        /// </summary>
        public IValue Start { get; set; }

        /// <summary>
        /// Step to increase the index each loop
        /// </summary>
        public IValue Step { get; set; }

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