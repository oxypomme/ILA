using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public abstract class Value : BaseObject
    {
        #region Public Properties

        public VarType Type { get; internal set; }

        #endregion Public Properties
    }
}