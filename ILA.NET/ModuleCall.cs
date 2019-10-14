using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class ModuleCall : Instruction
    {
        #region Public Properties

        public Module CalledModule { get; internal set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        public IValue[] Parameters { get; internal set; }
        string IBaseObject.PythonCode => throw new NotImplementedException();

        #endregion Public Properties
    }
}