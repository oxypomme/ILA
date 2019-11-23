using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A module is a block of code that can be called from any executable
    /// </summary>
    public class Module : IExecutable
    {
        #region Public Properties

        /// <summary>
        /// The comment above its declaration
        /// </summary>
        public Comment AboveComment { get; set; }

        Comment IExecutable.AboveComment => AboveComment;

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string InlineComment { get; set; }

        Instruction[] IExecutable.Instructions => Instructions.ToArray();

        /// <summary>
        /// The name to call to execute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The parameters required to execute
        /// </summary>
        public List<Parameter> Parameters { get; set; }

        #endregion Public Properties

        #region Internal Properties

        string IExecutable.Comment => InlineComment;
        internal List<Instruction> Instructions { get; set; }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public virtual void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Internal Properties
    }
}