﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public interface IExecutable : IBaseObject
    {
        #region Public Properties

        public IDeclaration[] Declarations { get; }
        public Instruction[] Instructions { get; }
        public Comment[] Comments { get; }

        #endregion Public Properties
    }
}