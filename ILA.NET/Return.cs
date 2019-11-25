using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// The return instruction. Only usable in a function, it stops the function and returns a value.
    /// </summary>
    public class Return : Instruction
    {
        /// <summary>
        /// Integrated comment
        /// </summary>
        public string Comment { get; set; }
        string Instruction.Comment => Comment;
        public Function Function { get; set; }
        public IValue Type { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            Program.GenerateIndent(textWriter);
            textWriter.Write(Function.Name);
            textWriter.Write(" <- ");
            Type.WriteILA(textWriter);
            if (Comment != null && Comment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(Comment);
            }
            textWriter.WriteLine();
        }

        /// <summary>
        /// Value to return
        /// </summary>
        public IValue Value { get; set; }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}