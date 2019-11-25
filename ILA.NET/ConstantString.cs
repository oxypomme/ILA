using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A hard coded string value
    /// </summary>
    public class ConstantString : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ConstantString()
        {
            Type = GenericType.String;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type => Type;

        /// <summary>
        /// Value of the string
        /// </summary>
        public string Value { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write('"');
            textWriter.Write(Value);
            textWriter.Write('"');
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}