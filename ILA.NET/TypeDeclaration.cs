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
        Comment IDeclaration.AboveComment => AboveComment;
        string IDeclaration.Comment => InlineComment;
        public string InlineComment { get; set; }

        public void WritePython(TextWriter textWriter)
        {
            if (CreatedType is TableType table)
            {
                var indexList = new List<string>();

                // var baseIdent = ident
                //x .generateIdent()
                table.WritePython(textWriter);
                textWriter.Write(" = []\n");

                //x .generateIdent()
                for (int i = 0; i < table.DimensionsSize.Count; i++)
                {
                    //x .generateIdent()
                    indexList.Add("index" + i);
                    textWriter.Write("for " + indexList[i] + " in range(");
                    table.DimensionsSize[i].WritePython(textWriter);
                    textWriter.Write("):\n");
                    // ident++
                    //x .generateIdent()
                    table.WritePython(textWriter);
                    if (i < table.DimensionsSize.Count && i > 0)
                    {
                        for (int j = 0; j < i; j++)
                            textWriter.Write("[" + indexList[i - j] + "]");
                        textWriter.Write(")\n");
                    }
                    textWriter.Write(".append(");
                    if (i < table.DimensionsSize.Count)
                        textWriter.Write("[]");
                    else
                        textWriter.Write(0);
                    textWriter.Write(")\n");
                }
                // ident = baseIdent
            }
            else if (CreatedType is StructType struc)
            {
                //x .generateIdent()
                textWriter.Write("class ");
                struc.WritePython(textWriter);
                textWriter.Write(" :\n");

                foreach (var member in struc.Members)
                {
                    // ident++
                    //x .generateIdent()
                    textWriter.Write(member.Key + " = ");
                    member.Value.WritePython(textWriter);
                    textWriter.Write("\n");
                    // ident--
                }
            }
        }
    }
}