using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class StructType : VarType
    {
        #region Public Properties

        public Dictionary<string, VarType> Members { get; set; }

        public override void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}