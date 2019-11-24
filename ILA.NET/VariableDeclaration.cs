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
        Comment IDeclaration.AboveComment => AboveComment;
        string IDeclaration.Comment => InlineComment;
        public string InlineComment { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}