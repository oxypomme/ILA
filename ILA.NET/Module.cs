using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public class Module : IExecutable
    {
        #region Public Properties
        public string InlineComment { get; set; }

        public Comment AboveComment { get; set; }
        Comment IExecutable.AboveComment => AboveComment;
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        public List<Parameter> Parameters { get; set; }

        #endregion Public Properties

        #region Internal Properties

        internal List<Instruction> Instructions { get; set; }

        public string Name { get; set; }
        string IExecutable.Comment => InlineComment;

        public virtual void WritePython(TextWriter textWriter)
        {
            throw new NotImplementedException();
        }


        #endregion Internal Properties
    }
}