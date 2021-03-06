﻿using System;
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
        /// <summary>
        /// Constructor
        /// </summary>
        public While()
        {
            Comment = "";
            Condition = null;
            EndComment = "";
            Instructions = new List<Instruction>();
        }

        #region Public Properties

        /// <summary>
        /// The integrated comment
        /// </summary>
        public string Comment { get; set; }

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
            Program.Indent++;
            textWriter.WriteLine();
            foreach (var item in Instructions)
                item.WriteILA(textWriter);
            Program.Indent--;
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
            Program.GenerateIndent(textWriter);
            textWriter.Write("while (");
            if (Condition != null)
                Condition.WritePython(textWriter);
            else
                textWriter.Write("True");
            textWriter.Write(") :\n");
            foreach (var instruction in Instructions)
            {
                Program.Indent++;
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                Program.Indent--;
            }
            if (Instructions.Count == 0)
            {
                Program.Indent++;
                Program.GenerateIndent(textWriter);
                textWriter.Write("pass\n");
                Program.Indent--;
            }
        }

        #endregion Public Properties
    }
}