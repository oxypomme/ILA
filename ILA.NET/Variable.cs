using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class Variable : Value
    {
        #region Public Properties

        public bool Constant { get; internal set; }
        public Value ConstantValue { get; internal set; }
        public string Name { get; internal set; }

        #endregion Public Properties

        #region Protected Properties

        protected override string LuaCode => throw new NotImplementedException();

        protected override string PythonCode => throw new NotImplementedException();

        #endregion Protected Properties
    }
}