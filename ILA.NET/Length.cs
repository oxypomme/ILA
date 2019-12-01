using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Length function, returns
    /// </summary>
    public class Length : Function, Native
    {
        /// <summary>
        /// The only instance of the function
        /// </summary>
        public static readonly Function Instance = new Length();

        internal Length()
        {
            Name = "longueur";
            Parameters = new List<Parameter>()
            {
                new Parameter()
                {
                 ImportedVariable = new Variable()
                 {
                      Constant = false,
                       Name = "input",
                        Type = GenericType.String
                 }
                }
            };
            ReturnType = GenericType.Int;
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