using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET
{
    public partial class Program
    {
        public static string CatchString(string str, ref int index)
        {
            var res = "";
            if (index >= str.Length || !IsLetter(str[index]))
                return res;

            while (index < str.Length && IsLetterOrDigit(str[index]))
            {
                res += str[index];
                index++;
            }
            return res;
        }

        public static int CountRow(string str, int index)
        {
            var row = 1;
            for (int i = 0; i < str.Length && i < index; i++)
                if (str[i] == '\n')
                    row++;
            return row;
        }

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
            while (index < str.Length && IsWhiteSpace(str[index]))
                index++;
            if (requireData && index == str.Length)
                throw new ILAException("Erreur : données manquantes : ligne " + CountRow(str, index));
        }

        /// <summary>
        /// Skips blanks characters and line spacing
        /// </summary>
        /// <param name="str">string to parse</param>
        /// <param name="index">index to start from</param>
        /// <param name="requireData">
        /// True if it has to throw an exception if it reach the end of string
        /// </param>
        public static void SkipLine(string str, ref int index, bool requireData = false)
        {
            while (index < str.Length && (IsWhiteSpace(str[index]) || str[index] == '\n' || str[index] == '\r'))
                index++;
            if (requireData && index == str.Length)
                throw new ILAException("Erreur : programme non terminé");
        }

        internal static bool IsLetter(char c) => char.IsLetter(c) || c == '_';

        internal static bool IsLetterOrDigit(char c) => IsLetter(c) || char.IsDigit(c);

        internal static bool IsWhiteSpace(char c) => c == ' ' || c == '\t';

        internal static string RemoveBlanks(string str)
        {
            var res = "";
            foreach (var item in str)
                if (!IsWhiteSpace(item))
                    res += item;
            return res;
        }

        public class ILAException : Exception
        {
            #region Public Constructors

            public ILAException(string mess = "", Exception inner = null) : base(mess, inner)
            {
            }

            #endregion Public Constructors
        }
    }
}