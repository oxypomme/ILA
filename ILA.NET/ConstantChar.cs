using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class ConstantChar : Value
    {
        #region Public Constructors

        public ConstantChar()
        {
            Type = GenericType.Char;
        }

        #endregion Public Constructors

        #region Public Properties

        public char Value { get; internal set; }

        #endregion Public Properties

        #region Protected Properties

        protected override string LuaCode => throw new NotImplementedException();

        protected override string PythonCode => throw new NotImplementedException();

        #endregion Protected Properties
    }
}