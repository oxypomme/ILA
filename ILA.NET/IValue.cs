using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IValue : IBaseObject
    {
        #region Public Properties

        public VarType Type { get; }

        #endregion Public Properties
    }
}