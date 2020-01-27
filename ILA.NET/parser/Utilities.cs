using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET.Parser
{
    public static class Extensions
    {
        public static string GetILAString(IBaseObject baseObject)
        {
            using var tw = new System.IO.StringWriter();
            baseObject.WriteILA(tw);
            return tw.ToString();
        }

        public static string GetPythonString(IBaseObject baseObject)
        {
            using var tw = new System.IO.StringWriter();
            baseObject.WritePython(tw);
            return tw.ToString();
        }
    }

    public partial class Parser
    {
        internal static string CatchString(string str, ref int index)
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

        internal static int CountRow(string str, int index)
        {
            var row = 1;
            for (int i = 0; i < str.Length && i < index; i++)
                if (str[i] == '\n')
                    row++;
            return row;
        }

        internal static void FastForward(string str, ref int index, bool requireData = false)
        {
            while (index < str.Length && IsWhiteSpace(str[index]))
                index++;
            if (requireData && index == str.Length)
                throw new ILAException("Erreur : données manquantes : ");
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

        internal static void SkipLine(string str, ref int index, bool requireData = false)
        {
            while (index < str.Length && (IsWhiteSpace(str[index]) || str[index] == '\n' || str[index] == '\r'))
                index++;
            if (requireData && index == str.Length)
                throw new ILAException("Erreur : programme non terminé");
        }

        /// <summary>
        /// This is the type of exception thrown by this assembly
        /// </summary>
        public class ILAException : Exception
        {
            #region Public Constructors

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="mess">Message of the Exception</param>
            /// <param name="inner">Inner Exception</param>
            public ILAException(string mess = "", Exception inner = null) : base(mess, inner)
            {
            }

            #endregion Public Constructors
        }
    }
}