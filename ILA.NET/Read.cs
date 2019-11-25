using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// The read() method, that is the input of the console
    /// </summary>
    public sealed class Read : Module
    {
        /// <summary>
        /// The only instance of the method
        /// </summary>
        public static readonly Read Instance = new Read();

        internal Read()
        {
            Name = "lire";
            Parameters = new List<Parameter>();
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
            base.WritePython(textWriter);
        }
    }
}