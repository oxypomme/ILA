using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IExecutable : IBaseObject
    {
        #region Public Properties
        public string Name { get; }
        public Comment Comment { get; }
        public Instruction[] Instructions { get; }

        #endregion Public Properties
    }
}