﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class FunctionCall : IValue
    {
        #region Public Properties

        public List<IValue> Args { get; set; }
        public Function CalledFunction { get; set; }
        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();

        VarType IValue.Type => CalledFunction.ReturnType;

        #endregion Public Properties
    }
}