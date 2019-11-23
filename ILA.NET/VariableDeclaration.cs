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
        public string InlineComment { get; set; }
        string IDeclaration.Comment => InlineComment;

        Comment IDeclaration.AboveComment => AboveComment;

        public void WritePython(TextWriter textWriter)
        {
            CreatedVariable.WritePython(textWriter);
            textWriter.Write(" = ");
            if (CreatedVariable.Constant)
                CreatedVariable.ConstantValue.WritePython(textWriter);
            else if (CreatedVariable.Type is TableType table)
                textWriter.Write(table.Name);
            else if (!(CreatedVariable.Type is GenericType))
                CreatedVariable.Type.WritePython(textWriter);
            else
                textWriter.Write(0);
        }
    }
}