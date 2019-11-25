using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A base class for a type of variable
    /// </summary>
    public abstract class VarType : IBaseObject
    {
        #region Public Properties

        /// <summary>
        /// The name of the type
        /// </summary>
        public virtual string Name { get; set; }

        public virtual void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(Name);
        }

        public abstract void WritePython(TextWriter textWriter);

        #endregion Public Properties
    }
}