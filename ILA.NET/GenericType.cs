using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    /// <summary>
    /// Generic types are the native one, bool char float int string
    /// </summary>
    public sealed class GenericType : VarType
    {
        #region Public Fields

        /// <summary>
        /// The bool type
        /// </summary>
        public static readonly GenericType Bool = new GenericType(Flags.BOOL) { name = "bool" };

        /// <summary>
        /// The char type
        /// </summary>
        public static readonly GenericType Char = new GenericType(Flags.CHAR) { name = "char" };

        /// <summary>
        /// The float type
        /// </summary>
        public static readonly GenericType Float = new GenericType(Flags.FLOAT) { name = "float" };

        /// <summary>
        /// The int type
        /// </summary>
        public static readonly GenericType Int = new GenericType(Flags.INT) { name = "int" };

        /// <summary>
        /// The string type
        /// </summary>
        public static readonly GenericType String = new GenericType(Flags.STRING) { name = "string" };

        #endregion Public Fields

        #region Private Constructors

        internal string name;

        private GenericType(Flags type) => Type = type;

        /// <summary>
        /// The name of the type.
        /// </summary>
        public override string Name { get => name; set => throw new InvalidOperationException("Unable to change the name of a generic type."); }

        #endregion Private Constructors

        #region Public Enums

        private enum Flags
        {
            INT,
            FLOAT,
            CHAR,
            STRING,
            BOOL
        }

        #endregion Public Enums

        #region Private Properties

        private Flags Type { get; set; }

        /// <summary>
        /// Generate ila code to for this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WriteILA(TextWriter textWriter)
        {
            switch (Type)
            {
                case Flags.BOOL:
                    textWriter.Write("booleen");
                    break;

                case Flags.INT:
                    textWriter.Write("entier");
                    break;

                case Flags.FLOAT:
                    textWriter.Write("reel");
                    break;

                case Flags.CHAR:
                    textWriter.Write("caractere");
                    break;

                case Flags.STRING:
                    textWriter.Write("chaine");
                    break;
            }
        }

        /// <summary>
        /// Generate python code to run this element.
        /// </summary>
        /// <param name="textWriter">TextWriter to write in.</param>
        public override void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Private Properties
    }
}