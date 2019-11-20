using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public partial class Program : IExecutable
    {
        #region Public Enums

        public enum Language
        {
            PYTHON,
            LUA
        }

        #endregion Public Enums

        #region Public Properties

        public Comment AlgoComment { get; set; }
        Comment IExecutable.Comment => AlgoComment;
        IDeclaration[] IExecutable.Declarations => Declarations.ToArray();
        public List<IDeclaration> Declarations { get; set; }
        public List<Comment> FileComments { get; set; }
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        public List<Instruction> Instructions { get; set; }
        string IBaseObject.LuaCode => LuaCode;
        public string Name { get; set; }
        string IBaseObject.PythonCode => PythonCode;

        #endregion Public Properties

        #region Internal Properties

        internal string LuaCode => throw new NotImplementedException();
        internal string PythonCode => throw new NotImplementedException();

        #endregion Internal Properties

        #region Public Methods

        /// <summary>
        /// Skips every blank character
        /// </summary>
        /// <param name="str">string to parse</param>
        /// <param name="index">index to start from</param>
        /// <param name="requireData">
        /// True if it has to throw an exception if it reach the end of string
        /// </param>
        public static void FastForward(string str, ref int index, bool requireData = false)
        {
            while (index < str.Length && char.IsWhiteSpace(str[index]))
                index++;
            if (requireData && index == str.Length)
                throw new ILAException("Erreur : programme non terminé");
        }

        /// <summary>
        /// Skips blanks characters and line spacing
        /// </summary>
        /// <param name="str">string to parse</param>
        /// <param name="requireData">
        /// True if it has to throw an exception if it reach the end of string
        /// </param>
        /// <param name="requireData"></param>
        public static void SkipLine(string str, ref int index, bool requireData = false)
        {
            while (index < str.Length && (char.IsWhiteSpace(str[index]) || str[index] == '\n' || str[index] == '\r'))
                index++;
            if (requireData && index == str.Length)
                throw new ILAException("Erreur : programme non terminé");
        }

        public string GenerateCode(Language language)
        {
            switch (language)
            {
                case Language.PYTHON:
                    return PythonCode;

                case Language.LUA:
                    return LuaCode;

                default:
                    return null;
            }
        }

        #endregion Public Methods
    }
}