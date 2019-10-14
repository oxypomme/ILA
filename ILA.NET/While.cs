using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class While : Instruction
    {
        #region Public Properties

        public IValue Condition { get; internal set; }
        public Instruction[] Instructions { get; internal set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();

        string IBaseObject.PythonCode => throw new NotImplementedException();

        #endregion Public Properties
    }
}