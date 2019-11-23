using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class ModuleCall : Instruction
    {
        #region Public Properties

        public List<IValue> Args { get; set; }
        public Module CalledModule { get; set; }

        public string Comment { get; set; }
        string Instruction.Comment => Comment;

        public void WritePython(TextWriter textWriter)
        {
            CalledModule.WritePython(textWriter);
            textWriter.Write(" (");
            for (int i = 0; i < Args.Count; i++)
                textWriter.Write("ERROR"/*Args[i].WritePython(textWriter)*/); //! Erreur CS1503  Argument 1: conversion impossible de 'void' en 'bool'
            //TODO : fix CS1503
            textWriter.Write(")");
        }

        #endregion Public Properties
    }
}