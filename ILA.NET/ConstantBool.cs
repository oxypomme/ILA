using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A hard coded bool value
    /// </summary>
    public class ConstantBool : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ConstantBool()
        {
            Type = GenericType.Bool;
        }

        #endregion Public Constructors

        #region Public Properties

        VarType IValue.Type => Type;

        /// <summary>
        /// Value of the bool
        /// </summary>
        public bool Value { get; set; }

        public void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(Value ? "vrai" : "faux");
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}