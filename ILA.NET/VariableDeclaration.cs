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

        #region Internal Properties


        #endregion Internal Properties
        public Comment AboveComment { get; set; }
        public string InlineComment { get; set; }
        string IDeclaration.Comment => InlineComment;

        Comment IDeclaration.AboveComment => AboveComment;

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

    }
}