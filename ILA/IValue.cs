using System;
using System.Collections.Generic;
using System.Text;

namespace ILA
{
    internal interface IValue : IInstruction
    {
        #region Public Properties

        dynamic Value { get; }

        #endregion Public Properties
    }
}