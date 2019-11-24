using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILANET
{
    public partial class Program
    {
        public static IValue ParseValue(string code, bool constLock = false)
        {
            IValue res;
            var decomposed = Parenthesis.Generate(code);
            res = null;
            return res;
        }

        internal class Parenthesis
        {
            internal string CodeInside;
            internal string FunctionName;
            internal List<Parenthesis> RecursiveParenthesis;

            internal static Parenthesis Generate(string code, ref int index)
            {
                var res = new Parenthesis();
                res.CodeInside = "";
                res.FunctionName = null;
                res.RecursiveParenthesis = new List<Parenthesis>();
                while (code.Length > index && code[index] != ')')
                {
                    if (code[index] == '(')
                    {
                        int secIndex = index + 1;
                        var parenthesis = Generate(code, ref secIndex);
                        if (index > 0 && IsLetterOrDigit(code[index - 1]))
                        {
                            int i = index - 1;
                            parenthesis.FunctionName = "";
                            while (i >= 0 && IsLetterOrDigit(code[i]))
                                parenthesis.FunctionName = code[i--] + parenthesis.FunctionName;
                            res.CodeInside = res.CodeInside.Substring(0, res.CodeInside.Length - parenthesis.FunctionName.Length);
                        }
                        index = secIndex;
                        int nb = res.RecursiveParenthesis.Count;
                        res.RecursiveParenthesis.Add(parenthesis);
                        res.CodeInside += " ?" + nb.ToString("0000") + " ";
                    }
                    else
                        res.CodeInside += code[index];
                    index++;
                }
                return res;
            }

            internal static Parenthesis Generate(string code)
            {
                var i = 0;
                return Generate(code, ref i);
            }
        }
    }
}