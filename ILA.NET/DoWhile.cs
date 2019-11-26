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

        /// <summary>
        /// Condition in the while()
        /// </summary>
        public IValue Condition { get; set; }

        /// <summary>
        /// Comment after the while(), on the same line
        /// </summary>
        public string EndComment { get; set; }

        /// <summary>
        /// Block of instructions
        /// </summary>
        public List<Instruction> Instructions { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
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
            Program.Indent++;
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            Program.Indent--;
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
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("while True :\n");
            foreach (var instruction in Instructions)
            {
                Program.Indent++;
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                Program.Indent--;
            }
            Program.Indent++;
            Program.GenerateIndent(textWriter);
            textWriter.Write("if not (");
            Condition.WritePython(textWriter);
            textWriter.Write(") :\n");
            Program.Indent++;
            Program.GenerateIndent(textWriter);
            textWriter.Write("break \n");
            Program.Indent -= 2;
        }

        #endregion Public Properties
    }
}