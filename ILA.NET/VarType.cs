using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public abstract class VarType : BaseObject
    {
        #region Public Properties

        public virtual string Name { get; internal set; }

        #endregion Public Properties
    }
}