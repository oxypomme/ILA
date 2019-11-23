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
        /// Block of instructions
        /// </summary>
        public Instruction[] Instructions { get; }

        #endregion Public Properties
    }
}