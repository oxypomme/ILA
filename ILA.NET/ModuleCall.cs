﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class ModuleCall : Instruction
    {
        #region Public Properties

        public IValue[] Args { get; internal set; }
        public Module CalledModule { get; internal set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();

        #endregion Public Properties
    }
}