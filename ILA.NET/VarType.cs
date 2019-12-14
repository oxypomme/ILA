using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// A base class for a type of variable
    /// </summary>
    public interface VarType : IBaseObject
    {
        #region Public Properties

        /// <summary>
        /// The name of the type
        /// </summary>
        public string Name { get; set; }

        #endregion Public Properties
    }
}