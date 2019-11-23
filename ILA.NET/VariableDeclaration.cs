using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class VariableDeclaration : IDeclaration
    {
        #region Public Properties

        public Variable CreatedVariable { get; set; }

        #endregion Public Properties

        public Comment AboveComment { get; set; }
        public string InlineComment { get; set; }
        string IDeclaration.Comment => InlineComment;

        Comment IDeclaration.AboveComment => AboveComment;

        public void WritePython(TextWriter textWriter)
        {
            CreatedVariable.WritePython(textWriter);
            textWriter.Write(" = ");
            if (CreatedVariable.Constant)
                CreatedVariable.ConstantValue.WritePython(textWriter);
            else if (CreatedVariable.Type is TableType table)
            {
                var indexList = new List<string>();

                // var baseIdent = ident
                //x .generateIdent()
                table.WritePython(textWriter);
                textWriter.Write(" = []\n");

                //x .generateIdent()
                for (int i = 0; i <= table.DimensionsSize.Count; i++)
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
            else if (!(CreatedVariable.Type is GenericType))
                CreatedVariable.Type.WritePython(textWriter);
            else
                textWriter.Write(0);
        }
    }
}