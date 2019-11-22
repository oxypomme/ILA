using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class EnumType : VarType
    {
        #region Public Properties

        public List<string> Values { get; set; }

        public override void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties

        #region Protected Properties


        #endregion Protected Properties
    }
}