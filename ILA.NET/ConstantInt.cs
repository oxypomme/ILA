using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class ConstantInt : Value
    {
        #region Public Constructors

        public ConstantInt()
        {
            Type = GenericType.Int;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Value { get; internal set; }

        #endregion Public Properties

        #region Protected Properties

        protected override string LuaCode => throw new NotImplementedException();

        protected override string PythonCode => throw new NotImplementedException();

        #endregion Protected Properties
    }
}