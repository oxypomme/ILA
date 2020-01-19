using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// The read() method, that is the input of the console
    /// </summary>
    public sealed class Read : Module, Native
    {
        /// <summary>
        /// The only instance of the method
        /// </summary>
        public static readonly Read Instance = new Read();

        internal Read()
        {
            Name = "lire";
            Parameters = new List<Parameter>();
            for (int i = 0; i < 200; i++) //enough parameters that we can imagine lmao
                Parameters.Add(new Parameter { Mode = Parameter.Flags.OUTPUT });
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