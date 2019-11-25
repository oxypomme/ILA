using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class VariableDeclaration : IDeclaration
    {
        #region Public Properties

        public Variable CreatedVariable { get; set; }

        #endregion Public Properties

        public Comment AboveComment { get; set; }
        Comment IDeclaration.AboveComment { get => AboveComment; set => AboveComment = value; }
        string IDeclaration.Comment { get => InlineComment; set => InlineComment = value; }
        public string InlineComment { get; set; }

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

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}