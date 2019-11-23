using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A declaration of a custom type
    /// </summary>
    public class TypeDeclaration : IDeclaration
    {
        #region Public Properties

        /// <summary>
        /// The custom type declared
        /// </summary>
        public VarType CreatedType { get; set; }

        #endregion Public Properties

        #region Internal Properties

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string InlineComment { get; set; }

        #endregion Internal Properties

        /// <summary>
        /// Comment block above this declaration
        /// </summary>
        public Comment AboveComment { get; set; }

        Comment IDeclaration.AboveComment => AboveComment;
        string IDeclaration.Comment => InlineComment;

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