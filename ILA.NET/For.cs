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
        /// <summary>
        /// Constructor
        /// </summary>
        public For()
        {
            Comment = "";
            End = null;
            EndComment = "";
            Index = null;
            Instructions = new List<Instruction>();
            Start = null;
            Step = null;
        }

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
        /// The comment after then end tag
        /// </summary>
        public string EndComment { get; set; }

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
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("pour ");
            Index.WriteILA(textWriter);
            textWriter.Write(" <- ");
            Start.WriteILA(textWriter);
            textWriter.Write(" a ");
            End.WriteILA(textWriter);
            if (!(Step is ConstantInt ci && ci.Value == 1))
            {
                textWriter.Write(" pas ");
                Step.WriteILA(textWriter);
            }
            textWriter.Write(" faire");
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
            textWriter.Write("fpour");
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
            // Index initialiser
            Program.GenerateIndent(textWriter);
            Index.WritePython(textWriter);
            textWriter.Write(" = ");
            Start.WritePython(textWriter);
            textWriter.Write("\n");

            // While condition
            Program.GenerateIndent(textWriter);
            textWriter.Write("while (");
            Index.WritePython(textWriter);
            textWriter.Write(" != ");
            End.WritePython(textWriter);
            textWriter.Write(") :\n");

            // While content
            foreach (Instruction instruction in Instructions)
            {
                Program.Indent++;
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                Program.Indent--;
            }
            Program.Indent++;
            Program.GenerateIndent(textWriter);
            Index.WritePython(textWriter);
            textWriter.Write("+=");
            Step.WritePython(textWriter);
            Program.Indent--;
        }

        #endregion Public Properties
    }
}