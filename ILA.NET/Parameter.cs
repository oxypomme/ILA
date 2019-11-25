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

        public void WriteILA(TextWriter textWriter)
        {
            if (Mode != Flags.INPUT)
            {
                if ((Mode & Flags.INPUT) != 0)
                    textWriter.Write('e');
                if ((Mode & Flags.OUTPUT) != 0)
                    textWriter.Write('s');
                textWriter.Write("::");
            }
            ImportedVariable.WriteILA(textWriter);
            textWriter.Write(':');
            ImportedVariable.Type.WriteILA(textWriter);
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Public Properties
    }
}