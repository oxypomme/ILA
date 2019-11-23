using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class TypeDeclaration : IDeclaration
    {
        #region Public Properties

        public VarType CreatedType { get; set; }

        #endregion Public Properties

        #region Internal Properties

        public string InlineComment { get; set; }

        #endregion Internal Properties

        public Comment AboveComment { get; set; }
        Comment IDeclaration.AboveComment { get => AboveComment; set => AboveComment = value; }
        string IDeclaration.Comment { get => InlineComment; set => InlineComment = value; }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}