using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// An instruction that calls a function
    /// </summary>
    public class FunctionCall : IValue
    {
        #region Public Properties

        /// <summary>
        /// The arguments to pass to the function
        /// </summary>
        public List<IValue> Args { get; set; }

        /// <summary>
        /// The called function
        /// </summary>
        public Function CalledFunction { get; set; }

        VarType IValue.Type => CalledFunction.ReturnType;

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}