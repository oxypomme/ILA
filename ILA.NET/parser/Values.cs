using System;
using System.Collections.Generic;
using System.Linq;

namespace ILANET.Parser
{
    public partial class Parser
    {
        public static IValue ParseValue(string code, Program mainProg, bool constLock = false)
        {
            var decomposed = Parenthesis.Generate(code);
            //var res = ParseParenthesis(decomposed, mainProg, constLock);
            int i = 0;
            return null;
        }

        internal static T Max<T>(params T[] elements) where T : IComparable<T>
        {
            T max = elements.First();
            foreach (var item in elements)
            {
                if (max.CompareTo(item) < 0)
                    max = item;
            }
            return max;
        }

        internal static T Min<T>(params T[] elements) where T : IComparable<T>
        {
            T min = elements.First();
            foreach (var item in elements)
            {
                if (min.CompareTo(item) > 0)
                    min = item;
            }
            return min;
        }

        internal static IValue ParseOperand(string code, Program mainProg, Parenthesis p, bool constLock)
        {
            code = code.Trim();
            int index = Max(
                code.LastIndexOf('='),
                code.LastIndexOf('☻'),
                code.LastIndexOf('♥'),
                code.LastIndexOf('♦'),
                code.LastIndexOf('<'),
                code.LastIndexOf('>')
                );
            if (index == -1)
            {
                index = code.LastIndexOf('•');
                if (index == -1)
                {
                    index = code.LastIndexOf('◘');
                    if (index == -1)
                    {
                        index = Max(
                            code.LastIndexOf('♣'),
                            code.LastIndexOf('♠'),
                            code.LastIndexOf('*'),
                            code.LastIndexOf('/')
                            );
                        if (index == -1)
                        {
                            index = Max(
                                code.LastIndexOf('+'),
                                code.LastIndexOf('-')
                                );
                            if (index == -1)
                            {
                                //non <val>, ?####, variable, constant
                                if (code.First() == '○') //non
                                {
                                    var op = new Operator();
                                    op.Left = null;
                                    op.
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static IValue ParseParenthesis(Parenthesis p, Program mainProg, bool constLock)
        {
            //We reduce the size of operators to one char using another code
            p.CodeInside = p.CodeInside.Replace("!=", "☻");
            p.CodeInside = p.CodeInside.Replace("<=", "♥");
            p.CodeInside = p.CodeInside.Replace(">=", "♦");
            p.CodeInside = p.CodeInside.Replace("mod", "♣");
            p.CodeInside = p.CodeInside.Replace("div", "♠");
            p.CodeInside = p.CodeInside.Replace("et", "•");
            p.CodeInside = p.CodeInside.Replace("ou", "◘");
            p.CodeInside = p.CodeInside.Replace("non", "○");
            if (p.FunctionName == null)
            {
            }
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