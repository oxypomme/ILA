using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A while loop
    /// </summary>
    public class While : Instruction
    {
        #region Public Properties

        /// <summary>
        /// The integrated comment
        /// </summary>
        public string Comment { get; set; }

        string Instruction.Comment => Comment;

        /// <summary>
        /// The condition of the loop
        /// </summary>
        public IValue Condition { get; set; }

        /// <summary>
        /// The comment on the end tag
        /// </summary>
        public string EndComment { get; set; }

        /// <summary>
        /// The block of instructions
        /// </summary>
        public List<Instruction> Instructions { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("tantque ");
            Condition.WriteILA(textWriter);
            textWriter.Write(" faire");
            if (Comment != null && Comment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(Comment);
            }
            Program.ilaIndent++;
            textWriter.WriteLine();
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            Program.ilaIndent--;
            Program.GenerateIndent(textWriter);
            textWriter.Write("ftantque");
            if (EndComment != null && EndComment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(EndComment);
            }
            textWriter.WriteLine();
        }

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