﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class ConstantBool : IValue
    {
        #region Internal Fields

        internal VarType Type;

        #endregion Internal Fields

        #region Public Constructors

        public ConstantBool()
        {
            Type = GenericType.Bool;
        }

        #endregion Public Constructors

        #region Public Properties

        string IBaseObject.LuaCode => throw new NotImplementedException();
        string IBaseObject.PythonCode => throw new NotImplementedException();
        VarType IValue.Type => Type;
        public bool Value { get; internal set; }

        #endregion Public Properties
    }
}