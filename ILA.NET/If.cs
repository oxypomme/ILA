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

        public List<string> ElifComments { get; set; }
        public string ElseComment { get; set; }
        public IValue ElseCondition { get; set; }

        /// <summary>
        /// Else block of instructions
        /// </summary>
        public List<Instruction> ElseInstructions { get; set; }

        /// <summary>
        /// Condition of the main block
        /// </summary>
        public IValue IfCondition { get; set; }
        public string EndComment { get; set; }

        /// <summary>
        /// Main block of instructions
        /// </summary>
        public List<Instruction> IfInstructions { get; set; }

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
            Program.ilaIndent++;
            foreach (var item in IfInstructions)
                item.WriteILA(textWriter);
            Program.ilaIndent--;
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
                Program.ilaIndent++;
                foreach (var item2 in item.Item2)
                    item2.WriteILA(textWriter);
                Program.ilaIndent--;
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
                Program.ilaIndent++;
                foreach (var item in ElseInstructions)
                    item.WriteILA(textWriter);
                Program.ilaIndent--;
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
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}