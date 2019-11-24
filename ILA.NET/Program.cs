using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public partial class Program : IExecutable
    {
        #region Public Properties

        Comment IExecutable.AboveComment => AlgoComment;
        public Comment AlgoComment { get; set; }
        string IExecutable.Comment => InlineComment;
        public List<IDeclaration> Declarations { get; set; }
        IDeclaration[] IExecutable.Declarations => Declarations.ToArray();
        public List<Comment> FileComments { get; set; }
        public string InlineComment { get; set; }
        Instruction[] IExecutable.Instructions => Instructions.ToArray();

        public List<Instruction> Instructions { get; set; }
        public List<Module> Methods { get; set; }
        public string Name { get; set; }
        string IExecutable.Name => Name;

        #endregion Public Properties

        #region Public Methods

        public void WritePython(TextWriter textWriter)
        {
            /*
             * attention aux Methods, elles contiennent les instances de Print et Read
             * qui ne doivent pas être définies
             */
            foreach (var declaration in Declarations)
            {
                declaration.WritePython(textWriter);
            }

            foreach (var module in Methods)
            {
                module.WritePython(textWriter);
            }

            textWriter.Write("def " + Name + "() :\n");
            // ident++
            foreach (var instruction in Instructions)
            {
                instruction.WritePython(textWriter);
            }
            // ident--
            textWriter.Write(Name + "()");
        }

        #endregion Public Methods
    }
}