using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// An instruction is a line of code that is executed in a executable block
    /// </summary>
    public interface Instruction : IBaseObject
    {
        /// <summary>
        /// Integrated comment
        /// </summary>
        public string Comment { get; }
    }
}