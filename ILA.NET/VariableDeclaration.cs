using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A declaration of a variable
    /// </summary>
    public class VariableDeclaration : IDeclaration
    {
        #region Public Properties

        /// <summary>
        /// The variable to declare
        /// </summary>
        public Variable CreatedVariable { get; set; }

        #endregion Public Properties

        /// <summary>
        /// The comment block above this declaration
        /// </summary>
        public Comment AboveComment { get; set; }

        Comment IDeclaration.AboveComment { get => AboveComment; set => AboveComment = value; }

        string IDeclaration.Comment { get => InlineComment; set => InlineComment = value; }

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string InlineComment { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            AboveComment?.WriteILA(textWriter);
            Program.GenerateIndent(textWriter);
            CreatedVariable.WriteILA(textWriter);
            textWriter.Write(':');
            if (CreatedVariable.Constant)
            {
                textWriter.Write("const ");
                CreatedVariable.Type.WriteILA(textWriter);
                textWriter.Write(" <- ");
                CreatedVariable.ConstantValue.WriteILA(textWriter);
            }
            else
                CreatedVariable.Type.WriteILA(textWriter);
            if (InlineComment != null && InlineComment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(InlineComment);
            }
            textWriter.WriteLine();
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            CreatedVariable.WritePython(textWriter);
            textWriter.Write(" = ");
            if (CreatedVariable.Constant)
                CreatedVariable.ConstantValue.WritePython(textWriter);
            else if (!(CreatedVariable.Type is GenericType))
                CreatedVariable.Type.WritePython(textWriter);
            else
                textWriter.Write(0);
            textWriter.Write("\n");
        }
    }
}