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

        public string Comment { get; set; }
        string IDeclaration.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}