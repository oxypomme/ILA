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
            Parameters.Add(new Parameter()
            {
                ImportedVariable = new Variable()
                {
                    Constant = false,
                    Name = "printed",
                    Type = null
                }
            });
            Instructions = null;
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