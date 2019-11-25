using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public abstract class VarType : IBaseObject
    {
        #region Public Properties

        public virtual string Name { get; set; }

        public virtual void WriteILA(TextWriter textWriter)
        {
            textWriter.Write(Name);
        }

        public abstract void WritePython(TextWriter textWriter);

        #endregion Public Properties
    }
}