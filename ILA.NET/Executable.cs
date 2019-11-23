using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IExecutable : IBaseObject
    {
        #region Public Properties

        public Comment AboveComment { get; }
        public string Comment { get; }
        public IDeclaration[] Declarations { get; }
        public Instruction[] Instructions { get; }
        public string Name { get; }

        #endregion Public Properties
    }
}