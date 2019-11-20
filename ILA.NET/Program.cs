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

        IDeclaration[] IExecutable.Declarations => Declarations.ToArray();
        Instruction[] IExecutable.Instructions => Instructions.ToArray();
        string IBaseObject.LuaCode => LuaCode;
        public string Name { get; set; }
        string IBaseObject.PythonCode => PythonCode;

        #endregion Public Properties

        #region Internal Properties

        public List<IDeclaration> Declarations { get; set; }
        public List<Instruction> Instructions { get; set; }
        internal string LuaCode => throw new NotImplementedException();
        internal string PythonCode => throw new NotImplementedException();
        public List<Comment> Comments { get; set; }
        Comment[] IExecutable.Comments => Comments.ToArray();

        #endregion Internal Properties

        #region Public Methods
        /// <summary>
        /// Skips every blank character
        /// </summary>
        /// <param name="str">string to parse</param>
        /// <param name="index">index to start from</param>
        /// <returns>new index after skipping blank spaces</returns>
        public static int FastForward(string str, int index = 0)
        {
            while (index < str.Length && char.IsWhiteSpace(str[index]))
                index++;
            return index;
        }
        /// <summary>
        /// Skips blanks characters and line spacing
        /// </summary>
        /// <param name="str">string to parse</param>
        /// <param name="index">index to start from</param>
        /// <returns>new index after skipping blank spaces/lines</returns>
        public static int SkipLine(string str, int index = 0)
        {
            while (index < str.Length && (char.IsWhiteSpace(str[index]) || str[index] == '\n' || str[index] == '\r'))
                index++;
            return index;
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