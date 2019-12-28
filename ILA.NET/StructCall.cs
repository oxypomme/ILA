using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// An instruction that calls a subvariable of a struct
    /// </summary>
    public class StructCall : Variable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StructCall() : base()
        {
            Name = "";
            Struct = null;
        }

        #region Public Properties

        /// <summary>
        /// Name of the member
        /// </summary>
        public override string Name { get => base.Name; set => base.Name = value; }

        /// <summary>
        /// Structure to call from
        /// </summary>
        public Variable Struct { get; set; }

        /// <summary>
        /// Type of the subvariable. read only.
        /// </summary>
        public override VarType Type { get => ((StructType)Struct.Type).Members[Name]; set { } }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WriteILA(TextWriter textWriter)
        {
            Struct.WriteILA(textWriter);
            textWriter.Write('.');
            textWriter.Write(Name);
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
            Struct.WritePython(textWriter);
            textWriter.Write('.');
            textWriter.Write(Name);
        }

        #endregion Public Properties
    }
}