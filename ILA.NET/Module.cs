﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Module : IExecutable
    {
        #region Public Properties

        public Comment AboveComment { get; set; }
        Comment IExecutable.AboveComment => AboveComment;
        public string InlineComment { get; set; }
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }

        #endregion Public Properties

        #region Internal Properties

        string IExecutable.Comment => InlineComment;
        internal List<Instruction> Instructions { get; set; }

        public virtual void WritePython(TextWriter textWriter)
        {
            // Def the module
            textWriter.Write("def ");
            textWriter.Write(Name);
            textWriter.Write(" (");
            for (int i = 0; i < Parameters.Count; i++)
            {
                Parameters[i].WritePython(textWriter);
                if (i < Parameters.Count - 1)
                    textWriter.Write(", ");
            }
            textWriter.Write("):\n");

            // write the input-output / output params
            foreach (var parameter in Parameters)
            {
                // ident++
                //x .generateIdent()
                if (parameter.Mode == Parameter.Flags.OUTPUT || parameter.Mode == Parameter.Flags.IO)
                {
                    parameter.WritePython(textWriter);
                    textWriter.Write(" = 0\n");
                }
                // ident--
            }

            // write the instructions
            foreach (var instruction in Instructions)
            {
                // ident++
                //x .generateIdent()
                instruction.WritePython(textWriter);
                // ident--
            }

            // return out vars
            // ident++
            //x .generateIdent()
            textWriter.Write("return ");
            foreach (var parameter in Parameters)
            {
                for (int i = 0; i < Parameters.Count; i++)
                {
                    Parameters[i].WritePython(textWriter);
                    if (i < Parameters.Count - 1)
                        textWriter.Write(", ");
                }
            }
            // ident--
        }

        #endregion Internal Properties
    }
}