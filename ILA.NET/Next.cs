using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Length function, returns
    /// </summary>
    public class Next : Function
    {
        /// <summary>
        /// The only instance of the function
        /// </summary>
        public static readonly Next Instance = new Next();

        internal Next()
        {
            Name = "succ";
            Parameters = new List<Parameter>()
            {
                new Parameter()
                {
                 ImportedVariable = new Variable()
                 {
                      Constant = false,
                       Name = "input",
                        Type = null
                 }
                }
            };
            ReturnType = null;
            Instructions = null;
        }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WriteILA(TextWriter textWriter)
        {
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
        }
    }
}