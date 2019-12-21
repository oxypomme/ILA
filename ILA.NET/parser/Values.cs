using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ILANET.Parser
{
    /// <summary>
    /// The parser has all the methods to generate the structure from ILA code
    /// </summary>
    public static partial class Parser
    {
        /// <summary>
        /// Parse a string and returns a value
        /// </summary>
        /// <param name="code">The ILA coded string</param>
        /// <param name="mainProg">The algo from where the value comes</param>
        /// <param name="currentBlock">the scope from where the value comes</param>
        /// <param name="constLock">true if the value has to be constant (false by default)</param>
        /// <returns>parsed value</returns>
        public static IValue ParseValue(string code, Program mainProg, IExecutable currentBlock, bool constLock = false)
        {
            var decomposed = Parenthesis.Generate(code);
            var res = ParseParenthesis(decomposed, mainProg, currentBlock, constLock);
            return res;
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
            if (code.Length == 0)
            {
                //fct/tab call
                if (p.FunctionName != null)
                {
                    if (constLock)
                        throw new ILAException("Erreur impossible de donner une valeur non constante");
                    var call = new FunctionCall();
                    call.CalledFunction = null;
                    call.Args = new List<IValue>();
                    foreach (var item in mainProg.Methods)
                    {
                        if (item.Name == p.FunctionName && item is Function f)
                        {
                            call.CalledFunction = f;
                            break;
                        }
                    }
                    if (call.CalledFunction == null)
                        throw new ILAException("Aucune fonction nommée '" + p.FunctionName + "' trouvée");
                    foreach (var item in p.FctIndexes)
                        call.Args.Add(ParseParenthesis(item, mainProg, currentBlock, constLock));
                    return call;
                }
                else if (p.TabName != null)
                {
                    if (constLock)
                        throw new ILAException("Erreur impossible de donner une valeur non constante");
                    var call = new TableCall();
                    call.Table = null;
                    call.DimensionsIndex = new List<IValue>();
                    foreach (var item in currentBlock.Declarations)
                    {
                        if (item is VariableDeclaration vd && vd.CreatedVariable.Name == p.TabName)
                        {
                            call.Table = vd.CreatedVariable;
                            break;
                        }
                    }
                    if (call.Table == null)
                        throw new ILAException("Aucune variable nommée '" + p.TabName + "' trouvée");
                    foreach (var item in p.TabIndexes)
                        call.DimensionsIndex.Add(ParseParenthesis(item, mainProg, currentBlock, constLock));
                    return call;
                }
                else
                    return null;
            }
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
                             code.LastIndexOf('+'),
                             code.LastIndexOf('-')
                             );
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
                                //non <val>, ?####, variable, constant, enum, unary minus
                                if (code.First() == '○') //non
                                {
                                    var op = new Operator();
                                    op.Left = null;
                                    op.OperatorType = Operator.Tag.NOT;
                                    op.Right = ParseOperand(code.Substring(1), mainProg, currentBlock, p, constLock);
                                    return op;
                                }
                                else if (code.First() == '◙') //unary minus
                                {
                                    var op = new Operator();
                                    op.Left = null;
                                    op.OperatorType = Operator.Tag.MINUS;
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
                                        //variable, enum, bool constant
                                        string n = "";
                                        int i = 0;
                                        while (IsLetterOrDigit(code[i]))
                                        {
                                            n += code[i++];
                                            if (i == code.Length)
                                                break;
                                        }
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
                                        index = -1;
                                        {
                                            int opened = 0;
                                            for (int j = 0; j < code.Length; j++)
                                            {
                                                if ((code[j] == '[' || code[j] == '.') && opened == 0)
                                                    index = j;
                                                if (code[j] == '[')
                                                    opened++;
                                                if (code[j] == ']')
                                                    opened--;
                                            }
                                        }
                                        if (index == -1)
                                        {
                                            //simple variable
                                            foreach (var decl in currentBlock.Declarations)
                                            {
                                                if (decl is VariableDeclaration vd && vd.CreatedVariable.Name == n)
                                                {
                                                    if (constLock && !vd.CreatedVariable.Constant)
                                                        throw new ILAException("Erreur impossible de donner une valeur non constante");
                                                    return vd.CreatedVariable;
                                                }
                                            }
                                            if (currentBlock is Module m)
                                                foreach (var par in m.Parameters)
                                                {
                                                    if (par.ImportedVariable.Name == n)
                                                        return par.ImportedVariable;
                                                }
                                            throw new ILAException("Aucune variable nommée '" + n + "' trouvée");
                                        }
                                        else if (code[index] == '.')
                                        {
                                            //struct call
                                            if (constLock)
                                                throw new ILAException("Erreur impossible de donner une valeur non constante");
                                            var left = code.Substring(0, index);
                                            var right = code.Substring(index + 1);
                                            var leftVar = ParseValue(left, mainProg, currentBlock, constLock) as Variable;
                                            return new StructCall()
                                            {
                                                Constant = false,
                                                Name = right,
                                                Struct = leftVar
                                            };
                                            throw new ILAException("Erreur : variable '" + n + "' introuvable dans cette portée");
                                        }
                                        else
                                        {
                                            //table call
                                            if (constLock)
                                                throw new ILAException("Erreur impossible de donner une valeur non constante");
                                            var left = code.Substring(0, index);
                                            var right = code.Substring(index + 1);
                                            var leftVar = ParseValue(left, mainProg, currentBlock, constLock) as Variable;
                                            var opened = 0;
                                            var args = new List<string>();
                                            var currentArg = "";
                                            int j = 0;
                                            while (right[j] != ']' || opened > 0)
                                            {
                                                if (right[j] == '[')
                                                    opened++;
                                                if (right[j] == ']')
                                                    opened--;
                                                if (opened == 0 && right[j] == ',')
                                                {
                                                    args.Add(currentArg);
                                                    currentArg = "";
                                                }
                                                else
                                                    currentArg += right[j];
                                                j++;
                                            }
                                            args.Add(currentArg);
                                            return new TableCall()
                                            {
                                                Constant = false,
                                                Table = leftVar,
                                                DimensionsIndex = args.Select(a => ParseValue(a, mainProg, currentBlock, constLock)).ToList()
                                            };
                                            throw new ILAException("Erreur : variable '" + n + "' introuvable dans cette portée");
                                        }
                                    }
                                    else
                                    {
                                        //constant
                                        if (char.IsDigit(code.First()))
                                        {
                                            if (code.Contains('.'))
                                            {
                                                //float
                                                try
                                                {
                                                    return new ConstantFloat() { Value = float.Parse(code, new CultureInfo("en")) };
                                                }
                                                catch (Exception)
                                                {
                                                    throw new ILAException("Erreur, format de nombre incorrect");
                                                }
                                            }
                                            else
                                            {
                                                //int
                                                try
                                                {
                                                    return new ConstantInt() { Value = int.Parse(code, new CultureInfo("en")) };
                                                }
                                                catch (Exception)
                                                {
                                                    throw new ILAException("Erreur, format de nombre incorrect");
                                                }
                                            }
                                        }
                                        else if (code.First() == '\'')
                                        {
                                            //char
                                            if (code[1] == '\\')
                                            {
                                                if (code[3] != '\'')
                                                    throw new ILAException("Erreur, format de caractere incorrect");
                                                return (code[2]) switch
                                                {
                                                    '\'' => new ConstantChar() { Value = '\'' },
                                                    '"' => new ConstantChar() { Value = '"' },
                                                    '\\' => new ConstantChar() { Value = '\\' },
                                                    'n' => new ConstantChar() { Value = '\n' },
                                                    'r' => new ConstantChar() { Value = '\r' },
                                                    't' => new ConstantChar() { Value = '\t' },
                                                    'b' => new ConstantChar() { Value = '\b' },
                                                    'f' => new ConstantChar() { Value = '\f' },
                                                    _ => throw new ILAException("Erreur : caractère échapé inconnu '\\" + code[2] + "'"),
                                                };
                                            }
                                            else
                                            {
                                                if (code[2] != '\'')
                                                    throw new ILAException("Erreur, format de caractere incorrect");
                                                return new ConstantChar() { Value = code[1] };
                                            }
                                        }
                                        else if (code.First() == '"')
                                        {
                                            //string
                                            var str = "";
                                            int i = 1;
                                            while (code[i] != '"')
                                            {
                                                if (code[i] == '\\')
                                                {
                                                    i++;
                                                    str += (code[i]) switch
                                                    {
                                                        '\'' => '\'',
                                                        '"' => '\"',
                                                        '\\' => '\\',
                                                        'n' => '\n',
                                                        'r' => '\r',
                                                        't' => '\t',
                                                        'b' => '\b',
                                                        'f' => '\f',
                                                        _ => throw new ILAException("Erreur : caractère échapé inconnu '\\" + code[2] + "'"),
                                                    };
                                                }
                                                else
                                                    str += code[i];
                                                i++;
                                                if (i >= code.Length)
                                                    throw new ILAException("Erreur de synthaxe");
                                            }
                                            return new ConstantString() { Value = str };
                                        }
                                        else
                                        {
                                            //wtf is this supposed to be ????
                                            throw new ILAException("Erreur de synthaxe '" + code + "' illisible");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var op = new Operator();
                                switch (code[index])
                                {
                                    case '*':
                                        op.OperatorType = Operator.Tag.MULT;
                                        break;

                                    case '/':
                                        op.OperatorType = Operator.Tag.DIV;
                                        break;

                                    case '♠':
                                        op.OperatorType = Operator.Tag.INT_DIV;
                                        break;

                                    case '♣':
                                        op.OperatorType = Operator.Tag.MOD;
                                        break;
                                }
                                op.Left = ParseOperand(code.Substring(0, index), mainProg, currentBlock, p, constLock);
                                op.Right = ParseOperand(code.Substring(index + 1), mainProg, currentBlock, p, constLock);
                                return op;
                            }
                        }
                        else
                        {
                            var op = new Operator();
                            if (code[index] == '+')
                                op.OperatorType = Operator.Tag.ADD;
                            else
                                op.OperatorType = Operator.Tag.SUB;
                            op.Left = ParseOperand(code.Substring(0, index), mainProg, currentBlock, p, constLock);
                            op.Right = ParseOperand(code.Substring(index + 1), mainProg, currentBlock, p, constLock);
                            return op;
                        }
                    }
                    else
                    {
                        var op = new Operator();
                        op.OperatorType = Operator.Tag.OR;
                        op.Left = ParseOperand(code.Substring(0, index), mainProg, currentBlock, p, constLock);
                        op.Right = ParseOperand(code.Substring(index + 1), mainProg, currentBlock, p, constLock);
                        return op;
                    }
                }
                else
                {
                    var op = new Operator();
                    op.OperatorType = Operator.Tag.AND;
                    op.Left = ParseOperand(code.Substring(0, index), mainProg, currentBlock, p, constLock);
                    op.Right = ParseOperand(code.Substring(index + 1), mainProg, currentBlock, p, constLock);
                    return op;
                }
            }
            else
            {
                var op = new Operator();
                switch (code[index])
                {
                    case '=':
                        op.OperatorType = Operator.Tag.EQUAL;
                        break;

                    case '☻':
                        op.OperatorType = Operator.Tag.DIFFRENT;
                        break;

                    case '♥':
                        op.OperatorType = Operator.Tag.SMALLER_EQUAL;
                        break;

                    case '♦':
                        op.OperatorType = Operator.Tag.BIGGER_EQUAL;
                        break;

                    case '<':
                        op.OperatorType = Operator.Tag.SMALLER;
                        break;

                    case '>':
                        op.OperatorType = Operator.Tag.BIGGER;
                        break;
                }
                op.Left = ParseOperand(code.Substring(0, index), mainProg, currentBlock, p, constLock);
                op.Right = ParseOperand(code.Substring(index + 1), mainProg, currentBlock, p, constLock);
                return op;
            }
        }

        internal static IValue ParseParenthesis(Parenthesis p, Program mainProg, IExecutable currentBlock, bool constLock)
        {
            //We reduce the size of operators to one char using another code
            p.CodeInside = p.CodeInside.Replace("!=", "☻");
            p.CodeInside = p.CodeInside.Replace("<=", "♥");
            p.CodeInside = p.CodeInside.Replace(">=", "♦");
            p.CodeInside = p.CodeInside.Replace(" mod ", "♣");
            p.CodeInside = p.CodeInside.Replace(" div ", "♠");
            p.CodeInside = p.CodeInside.Replace(" et ", "•");
            p.CodeInside = p.CodeInside.Replace(" ou ", "◘");
            {
                //unary "non" detection
                string copy = "";
                var lastChar = false;
                for (int i = 0; i < p.CodeInside.Length - 4; i++)
                {
                    var item = p.CodeInside[i];
                    if (p.CodeInside.Substring(i, 3) == "non" && !lastChar && !IsLetter(p.CodeInside[i + 3]))
                    {
                        copy += '○';
                        i += 3;
                    }
                    else
                        copy += item;
                    lastChar = IsLetter(item);
                }
                copy += p.CodeInside.Substring(Max(0, p.CodeInside.Length - 4));
                p.CodeInside = copy;
            }
            {
                //unary '-' detection
                bool lastCharIsOperator = true;
                string copy = "";
                foreach (var item in p.CodeInside)
                {
                    if (!IsWhiteSpace(item))
                    {
                        if (item == '-')
                        {
                            if (lastCharIsOperator)
                                copy += '◙';
                            else
                                copy += '-';
                        }
                        else
                            copy += item;
                        lastCharIsOperator = !IsLetterOrDigit(item);
                    }
                    else
                        copy += item;
                }
                p.CodeInside = copy;
            }
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
            internal List<Parenthesis> FctIndexes;
            internal string FunctionName;
            internal List<Parenthesis> RecursiveParenthesis;
            internal List<Parenthesis> TabIndexes;
            internal string TabName;

            internal static Parenthesis Generate(string code, ref int index)
            {
                var res = new Parenthesis();
                res.CodeInside = "";
                res.FunctionName = null;
                res.TabName = null;
                res.RecursiveParenthesis = new List<Parenthesis>();
                while (code.Length > index && code[index] != ')')
                {
                    if (code[index] == '(')
                    {
                        if (index > 0 && IsLetterOrDigit(code[index - 1]))
                        {
                            var parenthesis = new Parenthesis();
                            parenthesis.CodeInside = "";
                            parenthesis.TabName = null;
                            parenthesis.FctIndexes = new List<Parenthesis>();
                            {
                                int i = index - 1;
                                parenthesis.FunctionName = "";
                                while (i >= 0 && IsLetterOrDigit(code[i]))
                                    parenthesis.FunctionName = code[i--] + parenthesis.FunctionName;
                                res.CodeInside = res.CodeInside.Substring(0, res.CodeInside.Length - parenthesis.FunctionName.Length);
                                string tableIndex = "";
                                index++;
                                int opened = 0;
                                while (code[index] != ')' || opened > 0)
                                {
                                    if (code[index] == '(')
                                        opened++;
                                    else if (code[index] == ')')
                                        opened--;
                                    tableIndex += code[index++];
                                }
                                var args = new List<string>();
                                opened = 0;
                                var currExpr = "";
                                foreach (var item in tableIndex)
                                {
                                    if (item == '[' || item == '(')
                                        opened++;
                                    else if (item == ']' || item == ')')
                                        opened--;
                                    if (opened == 0 && item == ',')
                                    {
                                        args.Add(currExpr);
                                        currExpr = "";
                                    }
                                    else
                                        currExpr += item;
                                }
                                if (currExpr.Trim() != "")
                                    args.Add(currExpr);
                                foreach (var item in args)
                                    parenthesis.FctIndexes.Add(Generate(item));
                            }
                            int nb = res.RecursiveParenthesis.Count;
                            res.RecursiveParenthesis.Add(parenthesis);
                            res.CodeInside += " ?" + nb.ToString("0000") + " ";
                        }
                        else
                        {
                            index++;
                            var parenthesis = Generate(code, ref index);
                            int nb = res.RecursiveParenthesis.Count;
                            res.RecursiveParenthesis.Add(parenthesis);
                            res.CodeInside += " ?" + nb.ToString("0000") + " ";
                        }
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