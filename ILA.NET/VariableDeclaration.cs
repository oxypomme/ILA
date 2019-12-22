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
        /// <summary>
        /// Constructor
        /// </summary>
        public VariableDeclaration()
        {
            CreatedVariable = null;
            AboveComment = null;
            InlineComment = "";
        }

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
            if (CreatedVariable.Constant)
            {
                textWriter.Write("def ");
                CreatedVariable.WritePython(textWriter);
                textWriter.Write(" :\n");
                Program.Indent++;
                Program.GenerateIndent(textWriter);
                textWriter.Write("return ");
                CreatedVariable.ConstantValue.WritePython(textWriter);
                Program.Indent--;
            }
            else
            {
                CreatedVariable.WritePython(textWriter);
                textWriter.Write(" = ");
                if (!(CreatedVariable.Type is GenericType || CreatedVariable.Type is StringType))
                    CreatedVariable.Type.WritePython(textWriter);
                else
                    textWriter.Write(0);
            }
            textWriter.Write("\n");
        }
    }
}