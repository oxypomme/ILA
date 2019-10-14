using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public sealed class GenericType : VarType
    {
        #region Public Fields

        public static readonly GenericType Char = new GenericType(Flags.CHAR) { Name = "char" };
        public static readonly GenericType Float = new GenericType(Flags.FLOAT) { Name = "float" };
        public static readonly GenericType Int = new GenericType(Flags.INT) { Name = "int" };
        public static readonly GenericType String = new GenericType(Flags.STRING) { Name = "string" };

        #endregion Public Fields

        #region Private Constructors

        private GenericType(Flags type) => Type = type;

        #endregion Private Constructors

        #region Public Enums

        public enum Flags
        {
            INT,
            FLOAT,
            CHAR,
            STRING
        }

        #endregion Public Enums

        #region Protected Properties

        protected override string LuaCode => throw new NotImplementedException();
        protected override string PythonCode => throw new NotImplementedException();

        #endregion Protected Properties

        #region Private Properties

        private Flags Type { get; set; }

        #endregion Private Properties
    }
}