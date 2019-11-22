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


        #endregion Internal Properties

        public string Comment { get; set; }
        string IDeclaration.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}