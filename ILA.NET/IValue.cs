using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// An interface for all elements that can be used as a value
    /// </summary>
    public interface IValue : IBaseObject
    {
        #region Public Properties

        /// <summary>
        /// Type of the value
        /// </summary>
        public VarType Type { get; }

        #endregion Public Properties
    }
}