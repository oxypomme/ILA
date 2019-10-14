using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class For : Instruction
    {
        #region Public Properties

        public IValue End { get; internal set; }
        public Variable Index { get; internal set; }
        public Instruction[] Instructions { get; internal set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        public IValue Start { get; internal set; }
        public IValue Step { get; internal set; }

        #endregion Public Properties
    }
}