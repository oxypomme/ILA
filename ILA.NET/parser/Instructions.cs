using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET.Parser
{
    public partial class Parser
    {
        internal static Instruction ParseInstru(string code, Program mainProg, IExecutable currentBlock, ref int index)
        {
            if (code.Substring(index, 2) == "//")
            {
                index += 2;
                var comm = new Comment();
                comm.MultiLine = false;
                comm.Message = "";
                while (code[index] != '\n')
                    comm.Message += code[index++];
                return comm;
            }
            else if (code.Substring(index, 2) == "/*")
            {
                index += 2;
                var comm = new Comment();
                comm.MultiLine = true;
                comm.Message = "";
                while (code.Substring(index, 2) != "*/")
                    comm.Message += code[index++];
                index += 2;
                return comm;
            }
            else if (code.Substring(index, 3) == "si " ||
                code.Substring(index, 3) == "si(")
            {
                if (code.Substring(index, 3) == "si(")
                    index--;
                var instru = new If();
                index += 3;
                var condition = "";
                bool parsing = true;
                while (true)
                {
                    if (code[index] == '\\')
                    {
                        condition += code[index];
                        index++;
                    }
                    if (code[index] == '"')
                        parsing = !parsing;
                    if (code.Substring(index, 6) == " alors" && parsing)
                        break;
                    condition += code[index];
                    index++;
                }
                instru.IfCondition = ParseValue(condition, mainProg, currentBlock);
                index += 6;
                FastForward(code, ref index, true);
                if (code.Substring(index, 2) == "//")
                {
                    index += 2;
                    instru.Comment = "";
                    while (code[index] != '\n')
                        instru.Comment += code[index++];
                }
                else
                    instru.Comment = null;
                SkipLine(code, ref index);
                instru.IfInstructions = new List<Instruction>();
                instru.Elif = new List<Tuple<IValue, List<Instruction>>>();
                instru.ElifComments = new List<string>();
                instru.EndComment = null;
                while (true)
                {
                    if (code.Substring(index, 3) == "fsi")
                    {
                        index += 3;
                        FastForward(code, ref index);
                        if (code.Substring(index, 2) == "//")
                        {
                            index += 2;
                            instru.EndComment = "";
                            while (code[index] != '\n')
                                instru.EndComment += code[index++];
                        }
                        return instru;
                    }
                    if (code.Substring(index, 9) == "sinon si ")
                        goto elif;
                    else if (code.Substring(index, 5) == "sinon")
                    {
                        index += 5;
                        FastForward(code, ref index);
                        if (code.Substring(index, 2) == "//")
                        {
                            index += 2;
                            instru.ElseComment = "";
                            while (code[index] != '\n')
                                instru.ElseComment += code[index++];
                        }
                        goto elseEtiq;
                    }
                    instru.IfInstructions.Add(ParseInstru(code, mainProg, currentBlock, ref index));
                    SkipLine(code, ref index);
                }
            elif:
                {
                    var currInstru = new List<Instruction>();
                    index += 9;
                    condition = "";
                    parsing = true;
                    while (true)
                    {
                        if (code[index] == '\\')
                        {
                            condition += code[index];
                            index++;
                        }
                        if (code[index] == '"')
                            parsing = !parsing;
                        if (code.Substring(index, 6) == " alors" && parsing)
                            break;
                        condition += code[index];
                        index++;
                    }
                    instru.Elif.Add(new Tuple<IValue, List<Instruction>>(ParseValue(condition, mainProg, currentBlock), currInstru));
                    index += 6;
                    FastForward(code, ref index, true);
                    if (code.Substring(index, 2) == "//")
                    {
                        index += 2;
                        string comm = "";
                        while (code[index] != '\n')
                            comm += code[index++];
                        instru.ElifComments.Add(comm);
                    }
                    else
                        instru.ElifComments.Add(null);
                    SkipLine(code, ref index);
                    while (true)
                    {
                        if (code.Substring(index, 3) == "fsi")
                        {
                            index += 3;
                            FastForward(code, ref index);
                            if (code.Substring(index, 2) == "//")
                            {
                                index += 2;
                                instru.EndComment = "";
                                while (code[index] != '\n')
                                    instru.EndComment += code[index++];
                            }
                            return instru;
                        }
                        if (code.Substring(index, 9) == "sinon si ")
                            goto elif;
                        else if (code.Substring(index, 5) == "sinon")
                        {
                            index += 5;
                            FastForward(code, ref index);
                            if (code.Substring(index, 2) == "//")
                            {
                                index += 2;
                                instru.ElseComment = "";
                                while (code[index] != '\n')
                                    instru.ElseComment += code[index++];
                            }
                            goto elseEtiq;
                        }
                        currInstru.Add(ParseInstru(code, mainProg, currentBlock, ref index));
                        SkipLine(code, ref index);
                    }
                }
            elseEtiq:
                {
                    instru.ElseInstructions = new List<Instruction>();
                    SkipLine(code, ref index);
                    while (true)
                    {
                        if (code.Substring(index, 3) == "fsi")
                        {
                            index += 3;
                            FastForward(code, ref index);
                            if (code.Substring(index, 2) == "//")
                            {
                                index += 2;
                                instru.EndComment = "";
                                while (code[index] != '\n')
                                    instru.EndComment += code[index++];
                            }
                            return instru;
                        }
                        instru.ElseInstructions.Add(ParseInstru(code, mainProg, currentBlock, ref index));
                        SkipLine(code, ref index);
                    }
                }
            }
            else if (code.Substring(index, 5) == "pour " ||
                code.Substring(index, 5) == "pour(")
            {
                if (code.Substring(index, 5) == "pour(")
                    index--;
                var instru = new For();
                index += 5;
                string variable = "";
                while (code.Substring(index, 2) != "<-")
                    variable += code[index++];
                {
                    var indexer = ParseValue(variable, mainProg, currentBlock);
                    if (!(indexer is Variable))
                        throw new ILAException("Erreur : l'idexeur doit être une variable");
                    instru.Index = indexer as Variable;
                }
                index += 2;
                string startValue = "";
                while (code.Substring(index, 3) != " a ")
                    startValue += code[index++];
                instru.Start = ParseValue(startValue, mainProg, currentBlock);
                index += 3;
                string endValue = "";
                while (code.Substring(index, 5) != " pas " && code.Substring(index, 5) != " pas(" &&
                    code.Substring(index, 6) != " faire")
                    endValue += code[index++];
                instru.End = ParseValue(endValue, mainProg, currentBlock);
                if (code.Substring(index, 4) == " pas")
                {
                    index += 4;
                    string stepValue = "";
                    while (code.Substring(index, 6) != " faire")
                        stepValue += code[index++];
                    instru.Step = ParseValue(stepValue, mainProg, currentBlock);
                }
                else
                    instru.Step = new ConstantInt() { Value = 1 };
                index += 6;
                FastForward(code, ref index);
                if (code.Substring(index, 2) == "//")
                {
                    index += 2;
                    instru.Comment = "";
                    while (code[index] != '\n')
                        instru.Comment += code[index++];
                }
                else
                    instru.Comment = null;
                SkipLine(code, ref index);
                instru.Instructions = new List<Instruction>();
                while (code.Substring(index, 5) != "fpour")
                {
                    instru.Instructions.Add(ParseInstru(code, mainProg, currentBlock, ref index));
                    SkipLine(code, ref index);
                }
                index += 5;
                FastForward(code, ref index);
                if (code.Substring(index, 2) == "//")
                {
                    index += 2;
                    instru.EndComment = "";
                    while (code[index] != '\n')
                        instru.EndComment += code[index++];
                }
                else
                    instru.EndComment = null;
                return instru;
            }
            else if (code.Substring(index, 8) == "tantque " ||
                code.Substring(index, 8) == "tantque(")
            {
                if (code.Substring(index, 8) == "tantque(")
                    index--;
                index += 8;
                var instru = new While();
                instru.Instructions = new List<Instruction>();
                var condition = "";
                while (code.Substring(index, 6) != " faire")
                    condition += code[index++];
                instru.Condition = ParseValue(condition, mainProg, currentBlock);
                index += 6;
                FastForward(code, ref index);
                if (code.Substring(index, 2) == "//")
                {
                    index += 2;
                    instru.Comment = "";
                    while (code[index] != '\n')
                        instru.Comment += code[index++];
                }
                else
                    instru.Comment = null;
                SkipLine(code, ref index);
                while (code.Substring(index, 8) != "ftantque" &&
                        code.Substring(index, 3) != "ftq")
                {
                    instru.Instructions.Add(ParseInstru(code, mainProg, currentBlock, ref index));
                    SkipLine(code, ref index);
                }
                if (code.Substring(index, 8) == "ftantque")
                    index += 8;
                else
                    index += 3;
                FastForward(code, ref index);
                if (code.Substring(index, 2) == "//")
                {
                    index += 2;
                    instru.EndComment = "";
                    while (code[index] != '\n')
                        instru.EndComment += code[index++];
                }
                else
                    instru.EndComment = null;
                return instru;
            }
            else if (code.Substring(index, 7) == "repeter")
            {
                index += 7;
                var instru = new DoWhile();
                instru.Instructions = new List<Instruction>();
                FastForward(code, ref index);
                if (code.Substring(index, 2) == "//")
                {
                    index += 2;
                    instru.Comment = "";
                    while (code[index] != '\n')
                        instru.Comment += code[index++];
                }
                else
                    instru.Comment = null;
                SkipLine(code, ref index);
                while (code.Substring(index, 7) != "jusqua ")
                {
                    instru.Instructions.Add(ParseInstru(code, mainProg, currentBlock, ref index));
                    SkipLine(code, ref index);
                }
                index += 7;
                var condition = "";
                while (code.Substring(index, 2) != "//" &&
                        code[index] != '\n')
                    condition += code[index++];
                instru.Condition = ParseValue(condition, mainProg, currentBlock);
                if (code.Substring(index, 2) == "//")
                {
                    index += 2;
                    instru.EndComment = "";
                    while (code[index] != '\n')
                        instru.EndComment += code[index++];
                }
                else
                    instru.EndComment = null;
                return instru;
            }
            else if (code.Substring(index, currentBlock.Name.Length) == currentBlock.Name)
            {
                var instru = new Return();
                var oldIndex = index;
                index += currentBlock.Name.Length;
                FastForward(code, ref index);
                if (code.Substring(index, 2) != "<-")
                {
                    if (code[index] == '(')
                    {
                        index++;
                        var param = "";
                        var opened = 0;
                        while (code[index] != ')' || opened > 0)
                        {
                            if (code[index] == '(')
                                opened++;
                            if (code[index] == ')')
                                opened--;
                            param += code[index++];
                        }
                        index++;
                        var args = new List<string>();
                        opened = 0;
                        var currArg = "";
                        foreach (var item in param)
                        {
                            if (item == '[' || item == '(')
                                opened++;
                            if (item == ']' || item == ')')
                                opened--;
                            if (item == ',' && opened == 0)
                            {
                                args.Add(currArg);
                                currArg = "";
                            }
                        }
                        args.Add(currArg);
                        var instru2 = new ModuleCall();
                        instru2.CalledModule = currentBlock as Module;
                        instru2.Args = new List<IValue>();
                        foreach (var item in args)
                            instru2.Args.Add(ParseValue(item, mainProg, currentBlock));
                        FastForward(code, ref index);
                        if (code.Substring(index, 2) == "//")
                        {
                            index += 2;
                            instru2.Comment = "";
                            while (code[index] != '\n')
                                instru2.Comment += code[index++];
                        }
                        else
                            instru2.Comment = null;
                        return instru2;
                    }
                    else
                        throw new ILAException("Erreur : operateur attendu : '<-'");
                }
                if (!(currentBlock is Function))
                    throw new ILAException("Erreur : impossible de créer une retour en dehors d'une fonction");
                instru.Function = currentBlock as Function;
                index += 2;
                FastForward(code, ref index);
                var value = "";
                while (code.Substring(index, 2) != "//" &&
                        code[index] != '\n')
                    value += code[index++];
                instru.Value = ParseValue(value, mainProg, currentBlock);
                if (code.Substring(index, 2) == "//")
                {
                    index += 2;
                    instru.Comment = "";
                    while (code[index] != '\n')
                        instru.Comment += code[index++];
                }
                else
                    instru.Comment = null;
                return instru;
            }
            else
            {
                var content = "";
                while (true)
                {
                    if (code.Substring(index, 2) == "<-")
                    {
                        var v = ParseValue(content, mainProg, currentBlock);
                        if (!(v is Variable))
                            throw new ILAException("Seules les variables peuvent être assignées");
                        var instru = new Assign();
                        instru.Left = v as Variable;
                        index += 2;
                        FastForward(code, ref index);
                        var value = "";
                        while (code.Substring(index, 2) != "//" &&
                                code[index] != '\n')
                            value += code[index++];
                        instru.Right = ParseValue(value, mainProg, currentBlock);
                        if (code.Substring(index, 2) == "//")
                        {
                            index += 2;
                            instru.Comment = "";
                            while (code[index] != '\n')
                                instru.Comment += code[index++];
                        }
                        else
                            instru.Comment = null;
                        return instru;
                    }
                    if (code[index] == '(')
                    {
                        index++;
                        var param = "";
                        var opened = 0;
                        while (code[index] != ')' || opened > 0)
                        {
                            if (code[index] == '(')
                                opened++;
                            if (code[index] == ')')
                                opened--;
                            param += code[index++];
                        }
                        index++;
                        var args = new List<string>();
                        opened = 0;
                        var currArg = "";
                        foreach (var item in param)
                        {
                            if (item == '[' || item == '(')
                                opened++;
                            if (item == ']' || item == ')')
                                opened--;
                            if (item == ',' && opened == 0)
                            {
                                args.Add(currArg);
                                currArg = "";
                            }
                            else
                                currArg += item;
                        }
                        args.Add(currArg);
                        var instru = new ModuleCall();
                        instru.CalledModule = null;
                        foreach (var item in mainProg.Methods)
                        {
                            if (item.Name == content.Trim())
                                instru.CalledModule = item;
                        }
                        if (instru.CalledModule == null)
                            throw new ILAException("Erreur : aucun module nommé '" + content.Trim() + "' trouvé");
                        instru.Args = new List<IValue>();
                        foreach (var item in args)
                            instru.Args.Add(ParseValue(item, mainProg, currentBlock));
                        FastForward(code, ref index);
                        if (code.Substring(index, 2) == "//")
                        {
                            index += 2;
                            instru.Comment = "";
                            while (code[index] != '\n')
                                instru.Comment += code[index++];
                        }
                        else
                            instru.Comment = null;
                        return instru;
                    }
                    content += code[index++];
                    if (index < code.Length && code[index] == '\n')
                    {
                        Console.WriteLine(content);
                        throw new ILAException("Erreur de synthaxe");
                    }
                }
            }
        }
    }
}