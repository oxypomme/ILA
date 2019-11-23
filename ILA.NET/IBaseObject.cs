using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IBaseObject
    {
        #region Protected Internal Properties

        public void WritePython(System.IO.TextWriter textWriter);

        #endregion Protected Internal Properties
    }
}