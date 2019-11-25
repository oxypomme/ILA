using System;
using System.Collections.Generic;
using System.IO;
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

        private GenericType(Flags type) => Type = type;

        public override string Name { get => name; set => throw new InvalidOperationException("Unable to change the name of a generic type."); }

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



        #region Private Properties

        private Flags Type { get; set; }

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

        public override void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }

        #endregion Private Properties
    }
}