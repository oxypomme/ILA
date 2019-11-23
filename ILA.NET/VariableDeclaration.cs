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
            //_ = CreatedVariable.PythonCode;
            // tenir compte de constante or not
            throw new NotImplementedException();
        }
    }
}