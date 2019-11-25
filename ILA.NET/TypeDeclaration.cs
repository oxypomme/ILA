using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class TypeDeclaration : IDeclaration
    {
        #region Public Properties

        public VarType CreatedType { get; set; }

        #endregion Public Properties

        #region Internal Properties

        public string InlineComment { get; set; }

        #endregion Internal Properties

        public Comment AboveComment { get; set; }
        Comment IDeclaration.AboveComment { get => AboveComment; set => AboveComment = value; }
        string IDeclaration.Comment { get => InlineComment; set => InlineComment = value; }

        public void WriteILA(TextWriter textWriter)
        {
            AboveComment?.WriteILA(textWriter);
            Program.GenerateIndent(textWriter);
            CreatedType.WriteILA(textWriter);
            textWriter.Write("type ");
            if (CreatedType is TableType t)
            {
                textWriter.Write("tableau[");
                for (int i = 0; i < t.DimensionsSize.Count; i++)
                {
                    if (i > 0)
                        textWriter.Write(", ");
                    t.DimensionsSize[i].WriteILA(textWriter);
                }
                textWriter.Write("]:");
                t.InternalType.WriteILA(textWriter);
            }
            else if (CreatedType is StructType s)
            {
                textWriter.WriteLine("structure(");
                Program.ilaIndent++;
                int i = 1;
                foreach (var item in s.Members)
                {
                    Program.GenerateIndent(textWriter);
                    textWriter.Write(item.Key);
                    textWriter.Write(':');
                    item.Value.WriteILA(textWriter);
                    if (i < s.Members.Count)
                        textWriter.Write(',');
                    textWriter.WriteLine();
                    i++;
                }
                Program.ilaIndent--;
                Program.GenerateIndent(textWriter);
                textWriter.Write(')');
            }
            else if (CreatedType is EnumType e)
            {
                if (e.Values.Count > 3)
                {
                    textWriter.Write("enumeration(");
                    for (int i = 0; i < e.Values.Count; i++)
                    {
                        if (i > 0)
                            textWriter.Write(", ");
                        textWriter.Write(e.Values[i]);
                    }
                    textWriter.Write(')');
                }
                else
                {
                    textWriter.WriteLine("enumeration(");
                    Program.ilaIndent++;
                    for (int i = 0; i < e.Values.Count; i++)
                    {
                        Program.GenerateIndent(textWriter);
                        textWriter.Write(e.Values[i]);
                        if (i < e.Values.Count - 1)
                            textWriter.Write(',');
                        textWriter.WriteLine();
                    }
                    Program.ilaIndent--;
                    Program.GenerateIndent(textWriter);
                    textWriter.Write(')');
                }
            }
            if (InlineComment != null && InlineComment.Length > 0)
            {
                textWriter.Write(" //");
                textWriter.Write(InlineComment);
            }
            textWriter.WriteLine();
        }

        public void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}