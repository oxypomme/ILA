using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IExecutable : IBaseObject
    {
        #region Public Properties
        public string Name { get; }
        public string Comment { get; }
        public Comment AboveComment { get; }
        public Instruction[] Instructions { get; }

        #endregion Public Properties
    }
}