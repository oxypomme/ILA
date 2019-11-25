using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Basic interface for all executables blocks
    /// </summary>
    public interface IExecutable : IBaseObject
    {
        #region Public Properties

        /// <summary>
        /// Comment block above the executable
        /// </summary>
        public Comment AboveComment { get; }

        /// <summary>
        /// Integrated comment
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// The declarations of the block
        /// </summary>
        public IDeclaration[] Declarations { get; }

        /// <summary>
        /// Block of instructions
        /// </summary>
        public Instruction[] Instructions { get; }

        /// <summary>
        /// The name of the block
        /// </summary>
        public string Name { get; }

        #endregion Public Properties
    }
}