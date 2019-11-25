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
            textWriter.Write(":type ");
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
                Program.Indent++;
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
                Program.Indent--;
                Program.GenerateIndent(textWriter);
                textWriter.Write(')');
            }
            else if (CreatedType is EnumType e)
            {
                if (e.Values.Count <= 3)
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
                    Program.Indent++;
                    for (int i = 0; i < e.Values.Count; i++)
                    {
                        Program.GenerateIndent(textWriter);
                        textWriter.Write(e.Values[i]);
                        if (i < e.Values.Count - 1)
                            textWriter.Write(',');
                        textWriter.WriteLine();
                    }
                    Program.Indent--;
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
            if (CreatedType is TableType table)
            {
                var indexList = new List<string>();

                var baseIdent = Program.Indent;
                Program.GenerateIndent(textWriter);
                table.WritePython(textWriter);
                textWriter.Write(" = []\n");

                Program.GenerateIndent(textWriter);
                for (int i = 0; i < table.DimensionsSize.Count; i++)
                {
                    Program.GenerateIndent(textWriter);
                    indexList.Add("index" + i);
                    textWriter.Write("for " + indexList[i] + " in range(");
                    table.DimensionsSize[i].WritePython(textWriter);
                    textWriter.Write("):\n");
                    Program.Indent++;
                    Program.GenerateIndent(textWriter);
                    table.WritePython(textWriter);
                    if (i < table.DimensionsSize.Count && i > 0)
                    {
                        for (int j = 0; j < i; j++)
                            textWriter.Write("[" + indexList[i - j] + "]");
                    }
                    textWriter.Write(".append(");
                    if (i < table.DimensionsSize.Count)
                        textWriter.Write("[]");
                    else
                        textWriter.Write(0);
                    textWriter.Write(")\n");
                }
                Program.Indent = baseIdent;
            }
            else if (CreatedType is StructType struc)
            {
                Program.GenerateIndent(textWriter);
                textWriter.Write("class ");
                struc.WritePython(textWriter);
                textWriter.Write(" :\n");

                foreach (var member in struc.Members)
                {
                    Program.Indent++;
                    Program.GenerateIndent(textWriter);
                    textWriter.Write(member.Key + " = 0 ");
                    textWriter.Write("\n");
                    Program.Indent--;
                }
            }
        }
    }
}