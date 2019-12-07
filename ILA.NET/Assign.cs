using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// The assign instruction sets a variable to a given value
    /// </summary>
    public class Assign : Instruction
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Assign()
        {
            Comment = "";
            Left = null;
            Right = null;
        }

        #region Public Properties

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string Comment { get; set; }

        string Instruction.Comment => Comment;

        /// <summary>
        /// Left operand, the variable
        /// </summary>
        public Variable Left { get; set; }

        /// <summary>
        /// The right operand, the value to assign to
        /// </summary>
        public IValue Right { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            Left.WriteILA(textWriter);
            textWriter.Write(" <- ");
            Right.WriteILA(textWriter);
            if (Comment != null && Comment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(Comment);
            }
            textWriter.WriteLine();
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            Left.WritePython(textWriter);
            textWriter.Write(" = ");
            Right.WritePython(textWriter);
            textWriter.Write("\n");
        }

        #endregion Public Properties
    }
}