using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Base interface for all elements
    /// </summary>
    public interface IBaseObject
    {
        public void WriteILA(System.IO.TextWriter textWriter);

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(System.IO.TextWriter textWriter);
    }
}