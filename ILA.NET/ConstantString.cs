using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class ConstantString : Value
    {
        #region Public Constructors

        public ConstantString()
        {
            Type = GenericType.String;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Value { get; internal set; }

        #endregion Public Properties

        #region Protected Properties

        protected override string LuaCode => throw new NotImplementedException();

        protected override string PythonCode => throw new NotImplementedException();

        #endregion Protected Properties
    }
}