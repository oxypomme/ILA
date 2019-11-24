using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Operator : IValue
    {
        #region Public Enums

        public enum Tag
        {
            /// <summary>
            /// the MINUS operator has only a right value and no left value.
            /// </summary>
            MINUS,

            ADD,
            SUB,
            DIV,
            MULT,
            INT_DIV,
            MOD,
            AND,
            OR,

            /// <summary>
            /// the NOT operator has only a right value and no left value.
            /// </summary>
            NOT,

            EQUAL,
            DIFFRENT,
            BIGGER,
            BIGGER_EQUAL,
            SMALLER,
            SMALLER_EQUAL
        }

        #endregion Public Enums

        #region Public Properties

        public Tag OpTag { get; set; }
        public IValue Left { get; set; }
        public Tag OperatorType { get; set; }
        public IValue Right { get; set; }

        #endregion Public Properties

        #region Internal Properties

        VarType IValue.Type => Type;

        #endregion Internal Properties

        internal VarType Type { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            switch (OpTag)
            {
                case Tag.MINUS:
                    textWriter.Write("-");
                    Right.WritePython(textWriter);
                    break;

                case Tag.ADD:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" + ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.SUB:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" - ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.DIV:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" / ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.MULT:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" * ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.INT_DIV:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" // ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.MOD:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" % ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.AND:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" and ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.OR:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" or ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.NOT:
                    textWriter.Write("not ");
                    Right.WritePython(textWriter);
                    break;

                case Tag.EQUAL:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" == ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.DIFFRENT:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" != ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.BIGGER:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" > ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.BIGGER_EQUAL:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" >= ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.SMALLER:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" < ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                case Tag.SMALLER_EQUAL:
                    textWriter.Write("(");
                    Left.WritePython(textWriter);
                    textWriter.Write(" <= ");
                    Right.WritePython(textWriter);
                    textWriter.Write(")");
                    break;

                default:
                    throw new Exception();
            }
        }
    }
}