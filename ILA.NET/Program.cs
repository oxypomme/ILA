using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ILANET
{
    public partial class Program : IExecutable
    {
        #region Public Properties

        internal static int Indent;
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
            Indent = 0;

            foreach (var declaration in Declarations)
            {
                declaration.WritePython(textWriter);
            }

            foreach (var module in Methods)
            {
                if (!(module is Read || module is Print))
                    module.WritePython(textWriter);
            }

            textWriter.Write("def " + Name + "() :\n");
            Indent++;
            foreach (var instruction in Instructions)
            {
                instruction.WritePython(textWriter);
            }
            Indent--;
            textWriter.Write(Name + "()");
        }

        internal static void GenerateIndent(TextWriter textWriter, int spaces = 4)
        {
            for (int i = 0; i < Indent * spaces; i++)
                textWriter.Write(' ');
        }

        #endregion Public Methods
    }
}