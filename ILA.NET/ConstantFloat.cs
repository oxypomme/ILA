using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public class ConstantFloat : Value
    {
        #region Public Constructors

        public ConstantFloat()
        {
            Type = GenericType.Float;
        }

        #endregion Public Constructors

        #region Public Properties

        public float Value { get; internal set; }

        #endregion Public Properties

        #region Protected Properties

        protected override string LuaCode => throw new NotImplementedException();

        protected override string PythonCode => throw new NotImplementedException();

        #endregion Protected Properties
    }
}