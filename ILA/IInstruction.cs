using System;
using System.Collections.Generic;
using System.Text;

namespace ILA
{
    internal interface IInstruction
    {
        #region Public Methods

        void Execute();

        #endregion Public Methods
    }
}