using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A function, a module that returns something
    /// </summary>
    public class Function : Module
    {
        #region Public Properties

        /// <summary>
        /// Type of the returned value. null if it is any type.
        /// </summary>
        public VarType ReturnType { get; set; }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
            base.WritePython(textWriter);
        }

        #endregion Public Properties
    }
}