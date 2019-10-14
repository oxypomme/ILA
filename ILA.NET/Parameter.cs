using System;
using System.Collections.Generic;
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

        public Variable ImportedVariable { get; internal set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        public Flags Mode { get; internal set; }
        string IBaseObject.PythonCode => throw new NotImplementedException();

        #endregion Public Properties
    }
}