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

        public abstract void WriteILA(TextWriter textWriter);

        public abstract void WritePython(TextWriter textWriter);

        #endregion Public Properties
    }
}