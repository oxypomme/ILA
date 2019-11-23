using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A conditionnal block of instructions
    /// </summary>
    public class If : Instruction
    {
        #region Public Properties

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string Comment { get; set; }

        string Instruction.Comment => Comment;

        /// <summary>
        /// Else if condition and block of instructions
        /// </summary>
        public List<Tuple<IValue, List<Instruction>>> Elif { get; set; }

        /// <summary>
        /// Else condition
        /// </summary>
        public IValue ElseCondition { get; set; }

        /// <summary>
        /// Else block of instructions
        /// </summary>
        public List<Instruction> ElseInstructions { get; set; }

        /// <summary>
        /// Condition of the main block
        /// </summary>
        public IValue IfCondition { get; set; }

        /// <summary>
        /// Main block of instructions
        /// </summary>
        public List<Instruction> IfInstructions { get; set; }

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