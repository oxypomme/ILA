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

        Comment IDeclaration.AboveComment => AboveComment;

        string IDeclaration.Comment => InlineComment;

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string InlineComment { get; set; }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}