using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public abstract class VarType : IBaseObject
    {
        #region Protected Properties

        #endregion Protected Properties

        #region Public Properties

        public virtual string Name { get; set; }

        public abstract void WritePython(TextWriter textWriter);

        #endregion Public Properties
    }
}