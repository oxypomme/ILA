using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Base interface for any declaration. A declaration can be a variable or a custom type used in
    /// an executable block
    /// </summary>
    public interface IDeclaration : IBaseObject
    {
        /// <summary>
        /// The comment block above this declaration
        /// </summary>
        public Comment AboveComment { get; set; }

        /// <summary>
        /// The integrated comment
        /// </summary>
        public string Comment { get; set; }
    }
}