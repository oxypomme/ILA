using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Returns the value just before the given scalar value
    /// </summary>
    public class Prev : Function, Native
    {
        /// <summary>
        /// The only instance of the function
        /// </summary>
        public static readonly Prev Instance = new Prev();

        internal Prev()
        {
            Name = "pred";
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