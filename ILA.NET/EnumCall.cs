using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class EnumCall : IValue
    {
        #region Public Properties

        public EnumType Enum { get; set; }
        public int Index { get; set; }
        VarType IValue.Type => Enum;

        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write(Index);
        }

        #endregion Public Properties
    }
}