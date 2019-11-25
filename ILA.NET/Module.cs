using System;
using System.Collections.Generic;
using System.IO;

namespace ILANET
{
    public class Module : IExecutable
    {
        #region Public Properties

        public Comment AboveComment { get; set; }
        Comment IExecutable.AboveComment => AboveComment;
        public List<IDeclaration> Declarations { get; set; }
        IDeclaration[] IExecutable.Declarations => Declarations.ToArray();
        public string InlineComment { get; set; }
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        public string Name { get; set; }
        public List<Parameter> Parameters { get; set; }

        #endregion Public Properties

        string IExecutable.Comment => InlineComment;
        public List<Instruction> Instructions { get; set; }

        public virtual void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }
    }
}