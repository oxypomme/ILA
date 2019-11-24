using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Parameter : IBaseObject
    {
        #region Public Enums

        public enum Flags
        {
            INPUT = 0x1 << 0,
            OUTPUT = 0x1 << 1,
            IO = INPUT | OUTPUT
        }

        #endregion Public Enums

        #region Public Properties

        public Variable ImportedVariable { get; set; }
        public Flags Mode { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            textWriter.Write(ImportedVariable.Name);
        }

        #endregion Public Properties
    }
}