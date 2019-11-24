using System;
using System.Collections.Generic;
using System.Linq;

namespace ILANET.Parser
{
    public partial class Parser
    {
        public static IValue ParseValue(string code, Program mainProg, IExecutable currentBlock, bool constLock = false)
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

        internal static IValue ParseOperand(string code, Program mainProg, IExecutable currentBlock, Parenthesis p, bool constLock)
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
                                //non <val>, ?####, variable, constant, enum
                                if (code.First() == '○') //non
                                {
                                    var op = new Operator();
                                    op.Left = null;
                                    op.OperatorType = Operator.Tag.NOT;
                                    op.Right = ParseOperand(code.Substring(1), mainProg, currentBlock, p, constLock);
                                    return op;
                                }
                                else if (code.First() == '?')//parenthesis group
                                {
                                    int pNumber = int.Parse(code.Substring(1, 4));
                                    return ParseParenthesis(p.RecursiveParenthesis[pNumber], mainProg, currentBlock, constLock);
                                }
                                else
                                {
                                    //variable, constant, enum
                                    if (IsLetter(code.First()))
                                    {
                                        //variable, enum, bool
                                        string n = "";
                                        int i = 0;
                                        while (IsLetterOrDigit(code[i]))
                                            n += code[i++];
                                        if (n == "vrai")
                                            return new ConstantBool() { Value = true };
                                        if (n == "faux")
                                            return new ConstantBool() { Value = false };
                                        //variable, enum
                                        foreach (var decl in mainProg.Declarations)
                                        {
                                            if (decl is TypeDeclaration td && td.CreatedType is EnumType en)
                                            {
                                                for (int j = 0; j < en.Values.Count; j++)
                                                {
                                                    if (en.Values[j] == n)
                                                    {
                                                        //enum call
                                                        return new EnumCall()
                                                        {
                                                            Enum = en,
                                                            Index = j
                                                        };
                                                    }
                                                }
                                            }
                                        }
                                        //variable
                                        if (code[n.Length] == '.')
                                        {
                                            //struct call
                                            var child = "";
                                            int j = n.Length + 1;
                                            while (IsLetterOrDigit(code[j]) || code[j] == '.')
                                                child += code[j];
                                            foreach (var item in currentBlock.Declarations)
                                            {
                                                if (item is VariableDeclaration vd && vd.CreatedVariable.Type is StructType)
                                                {
                                                    if (vd.CreatedVariable.Name == n)
                                                        return new StructCall() { Constant = false, Name = child, Struct = vd.CreatedVariable };
                                                }
                                            }
                                            throw new ILAException("Erreur : variable '" + n + "' introuvable dans cette portée");
                                        }
                                        if (code[n.Length] == '[')
                                        {
                                            //table call
                                            var indexValue = "";
                                            int j = n.Length + 1;
                                            int opened = 1;
                                            while (opened > 0)
                                            {
                                                if (code[j] == '[')
                                                    opened++;
                                                else if (code[j] == ']')
                                                    opened--;
                                                else
                                                    indexValue += code[j];
                                                j++;
                                                if (j == code.Length && opened > 0)
                                                    throw new ILAException("Erreur de lecture du tableau");
                                            }
                                            var args = new List<string>();
                                            opened = 0;
                                            var currentIndex = "";
                                            foreach (var c in indexValue)
                                            {
                                                if (code[j] == ',' && opened == 0)
                                                {
                                                    args.Add(currentIndex);
                                                    currentIndex = "";
                                                    continue;
                                                }
                                                if (code[j] == '[')
                                                    opened++;
                                                else if (code[j] == ']')
                                                    opened--;
                                                currentIndex += code[j];
                                            }
                                            foreach (var item in currentBlock.Declarations)
                                            {
                                                if (item is VariableDeclaration vd && vd.CreatedVariable.Type is TableType)
                                                {
                                                    if (vd.CreatedVariable.Name == n)
                                                    {
                                                        return new TableCall()
                                                        {
                                                            Constant = false,
                                                            Table = vd.CreatedVariable,
                                                            DimensionsIndex
                                                        }
                                                    }
                                                }
                                            }
                                            throw new ILAException("Erreur : variable '" + n + "' introuvable dans cette portée");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static IValue ParseParenthesis(Parenthesis p, Program mainProg, IExecutable currentBlock, bool constLock)
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
                return ParseOperand(p.CodeInside, mainProg, currentBlock, p, constLock);
            }
            else
            {
                return ParseOperand(p.CodeInside, mainProg, currentBlock, p, constLock);
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