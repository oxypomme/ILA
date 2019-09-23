using System;
using System.Collections.Generic;
using System.Text;

namespace ILA
{
    internal class Variable : IValue
    {
        #region Public Properties

        public virtual string Name { get; set; }
        public virtual dynamic Value => Program.CurrentVars.Find((v) => v.Name == Name);

        #endregion Public Properties

        #region Public Methods

        public virtual void Execute()
        {
        }

        #endregion Public Methods
    }
}