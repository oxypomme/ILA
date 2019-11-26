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
        /// The comments after the different Elif
        /// </summary>
        public List<string> ElifComments { get; set; }

        /// <summary>
        /// The comment after the else tag
        /// </summary>
        public string ElseComment { get; set; }

        /// <summary>
        /// Else block of instructions
        /// </summary>
        public List<Instruction> ElseInstructions { get; set; }

        /// <summary>
        /// The commend after the end tag
        /// </summary>
        public string EndComment { get; set; }

        /// <summary>
        /// Condition of the main block
        /// </summary>
        public IValue IfCondition { get; set; }

        /// <summary>
        /// Main block of instructions
        /// </summary>
        public List<Instruction> IfInstructions { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write("si ");
            IfCondition.WriteILA(textWriter);
            textWriter.Write(" alors");
            if (Comment != null && Comment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(Comment);
            }
            textWriter.WriteLine();
            Program.Indent++;
            foreach (var item in IfInstructions)
                item.WriteILA(textWriter);
            Program.Indent--;
            for (int i = 0; i < Elif.Count; i++)
            {
                var item = Elif[i];
                Program.GenerateIndent(textWriter);
                textWriter.Write("sinon si ");
                item.Item1.WriteILA(textWriter);
                textWriter.Write(" alors");
                if (ElifComments[i] != null && ElifComments[i].Length > 0)
                {
                    textWriter.Write(" //");
                    textWriter.Write(ElifComments[i]);
                }
                textWriter.WriteLine();
                Program.Indent++;
                foreach (var item2 in item.Item2)
                    item2.WriteILA(textWriter);
                Program.Indent--;
            }
            if (ElseInstructions != null && ElseInstructions.Count > 0)
            {
                Program.GenerateIndent(textWriter);
                textWriter.Write("sinon ");
                if (ElseComment != null && ElseComment.Length > 0)
                {
                    textWriter.Write(" //");
                    textWriter.Write(ElseComment);
                }
                textWriter.WriteLine();
                Program.Indent++;
                foreach (var item in ElseInstructions)
                    item.WriteILA(textWriter);
                Program.Indent--;
            }
            Program.GenerateIndent(textWriter);
            textWriter.Write("fsi");
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
            textWriter.Write("if (");
            IfCondition.WritePython(textWriter);
            textWriter.Write(") :\n");

            foreach (var instruction in IfInstructions)
            {
                Program.Indent++;
                instruction.WritePython(textWriter);
                textWriter.Write("\n");
                Program.Indent--;
            }

            foreach (var elif in Elif)
            {
                Program.GenerateIndent(textWriter);
                textWriter.Write("elif");
                IfCondition.WritePython(textWriter);
                textWriter.Write(") :\n");

                foreach (var instruction in elif.Item2)
                {
                    Program.Indent++;
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    Program.Indent--;
                }
            }

            if (ElseInstructions != null && ElseInstructions.Count > 0)
            {
                Program.GenerateIndent(textWriter);
                textWriter.Write("else (");
                IfCondition.WritePython(textWriter);
                textWriter.Write(") :\n");

                foreach (var instruction in ElseInstructions)
                {
                    Program.Indent++;
                    instruction.WritePython(textWriter);
                    textWriter.Write("\n");
                    Program.Indent--;
                }
            }
        }

        #endregion Public Properties
    }
}