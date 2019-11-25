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

        public Comment AboveComment { get; set; }
        string IDeclaration.Comment { get => InlineComment; set => InlineComment = value; }
        public string InlineComment { get; set; }

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