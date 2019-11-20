using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public sealed class GenericType : VarType
    {
        #region Public Fields

        public static readonly GenericType Bool = new GenericType(Flags.BOOL) { name = "bool" };
        public static readonly GenericType Char = new GenericType(Flags.CHAR) { name = "char" };
        public static readonly GenericType Float = new GenericType(Flags.FLOAT) { name = "float" };
        public static readonly GenericType Int = new GenericType(Flags.INT) { name = "int" };
        public static readonly GenericType String = new GenericType(Flags.STRING) { name = "string" };

        #endregion Public Fields

        #region Private Constructors
        internal string name;
        public override string Name { get => name; set => throw new InvalidOperationException("Unable to change the name of a generic type."); }
        private GenericType(Flags type) => Type = type;

        #endregion Private Constructors

        #region Public Enums

        public enum Flags
        {
            INT,
            FLOAT,
            CHAR,
            STRING,
            BOOL
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