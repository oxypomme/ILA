using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A do { } while(); block
    /// </summary>
    public class DoWhile : Instruction
    {
        #region Public Properties

        /// <summary>
        /// The integrated comment
        /// </summary>
        public string Comment { get; set; }

        string Instruction.Comment => Comment;
        public IValue Condition { get; set; }
        public string EndComment { get; set; }
        public List<Instruction> Instructions { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("repeter");
            if (Comment != null && Comment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(Comment);
            }
            textWriter.WriteLine();
            Program.ilaIndent++;
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            Program.ilaIndent--;
            Program.GenerateIndent(textWriter);
            textWriter.Write("jusqua ");
            Condition.WriteILA(textWriter);
            if (EndComment != null && EndComment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(EndComment);
            }
            textWriter.WriteLine();
        }

        /// <summary>
        /// Condition in the while()
        /// </summary>
        public IValue Condition { get; set; }

        /// <summary>
        /// Block of instructions
        /// </summary>
        public List<Instruction> Instructions { get; set; }

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