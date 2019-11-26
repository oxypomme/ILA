using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A hard coded float value
    /// </summary>
    public class ConstantFloat : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ConstantFloat()
        {
            Type = GenericType.Float;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type => Type;

        /// <summary>
        /// Value of the float
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(Value.ToString(new System.Globalization.CultureInfo("en")));
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write(Value.ToString(new System.Globalization.CultureInfo("en")));
        }

        #endregion Public Properties
    }
}