using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A basic variable
    /// </summary>
    public class Variable : IValue
    {
        #region Public Properties

        /// <summary>
        /// True if the variable is constant
        /// </summary>
        public virtual bool Constant { get; set; }

        /// <summary>
        /// If Constant is true, this is the value of the variable
        /// </summary>
        public virtual IValue ConstantValue { get; set; }

        /// <summary>
        /// The name of the variable
        /// </summary>
        public virtual string Name { get; set; }

        VarType IValue.Type => Type;

        #endregion Public Properties

        /// <summary>
        /// The type of the variable
        /// </summary>
        public virtual VarType Type { get; set; }

        public virtual void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(Name);
        }

        public virtual void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}