using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// The print() method that write in the console
    /// </summary>
    public sealed class Print : Module
    {
        /// <summary>
        /// The only instance of the method
        /// </summary>
        public static readonly Print Instance = new Print();

        internal Print()
        {
            Name = "ecrire";
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