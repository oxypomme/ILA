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
        }
    }
}