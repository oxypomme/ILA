using System;
using System.Collections.Generic;
using System.Text;

namespace ILANET.Parser
{
    public partial class Parser
    {
        /// <summary>
        /// Parse a given line of code representing an instruction
        /// </summary>
        /// <param name="code">code to parse</param>
        /// <param name="mainProg">program used</param>
        /// <param name="currentBlock">scope</param>
        /// <returns>the parsed instruction</returns>
        public static Instruction ParseInstruction(string code, Program mainProg, IExecutable currentBlock, bool ignoreConsts)
        {
            int index = 0;
            return ParseInstru(code, mainProg, currentBlock, ref index, ignoreConsts);
        }

        internal static Instruction ParseInstru(string code, Program mainProg, IExecutable currentBlock, ref int index, bool ignoreConsts)
        {
            code = code.TrimStart();
            if (code[^1] != '\n')
                code += '\n';
            code += "        ";
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
                    instru.IfInstructions.Add(ParseInstru(code, mainProg, currentBlock, ref index, ignoreConsts));
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
                        currInstru.Add(ParseInstru(code, mainProg, currentBlock, ref index, ignoreConsts));
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
                        instru.ElseInstructions.Add(ParseInstru(code, mainProg, currentBlock, ref index, ignoreConsts));
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
                        throw new ILAException("Erreur : l'indexeur doit être une variable");
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
                    instru.Instructions.Add(ParseInstru(code, mainProg, currentBlock, ref index, ignoreConsts));
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
                    instru.Instructions.Add(ParseInstru(code, mainProg, currentBlock, ref index, ignoreConsts));
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
                    instru.Instructions.Add(ParseInstru(code, mainProg, currentBlock, ref index, ignoreConsts));
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
            else if (code.Substring(index, 4) == "cas " ||
                code.Substring(index, 4) == "cas(")
            {
                if (code.Substring(index, 4) == "cas(")
                    index--;
                index += 4;
                var instru = new Switch();
                instru.Default = new List<Instruction>();
                instru.Cases = new List<Tuple<List<IValue>, List<Instruction>>>();
                {
                    var value = "";
                    while (code.Substring(index, 6) != " parmi")
                        value += code[index++];
                    instru.Value = ParseValue(value, mainProg, currentBlock);
                }
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
                    instru.EndComment = null;
                SkipLine(code, ref index);
                {
                    var currValues = new List<IValue>();
                    var currInstrus = new List<Instruction>();
                    while (true)
                    {
                        var subIndex = 0;
                        var line = "";
                        while (code[index] != '\n')
                            line += code[index++];
                        if (line.Substring(0, 4) == "fcas")
                        {
                            if (currValues.Count > 0)
                                instru.Cases.Add(new Tuple<List<IValue>, List<Instruction>>(currValues, currInstrus));
                            index += 4;
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
                            break;
                        }
                        if (!line.Contains(':'))
                            currInstrus.Add(ParseInstru(line + "\n       ", mainProg, currentBlock, ref subIndex, ignoreConsts));
                        else if (line.Substring(0, 7) == "defaut " ||
                                line.Substring(0, 7) == "defaut:")
                        {
                            subIndex = 0;
                            if (line.Substring(0, 7) == "defaut ")
                            {
                                subIndex += 7;
                                FastForward(line, ref subIndex);
                                if (line[subIndex] != ':')
                                    throw new ILAException("Erreur, caractère attendu ':'");
                            }
                            index -= line.Length - line.IndexOf(':') - 1;
                            if (currValues.Count > 0)
                                instru.Cases.Add(new Tuple<List<IValue>, List<Instruction>>(currValues, currInstrus));
                            SkipLine(code, ref index);
                            while (code.Substring(index, 4) != "fcas")
                            {
                                instru.Default.Add(ParseInstru(code, mainProg, currentBlock, ref index, ignoreConsts));
                                SkipLine(code, ref index);
                            }
                            index += 4;
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
                            break;
                        }
                        else
                        {
                            index -= line.Length - line.IndexOf(':') - 1;
                            var valuesStr = line.Substring(0, line.IndexOf(':'));
                            var values = new List<string>();
                            int opened = 0;
                            var currValue = "";
                            foreach (var item in valuesStr)
                            {
                                if (item == '[' || item == '(')
                                    opened++;
                                if (item == ']' || item == ')')
                                    opened--;
                                if (opened == 0 && item == ',')
                                {
                                    values.Add(currValue);
                                    currValue = "";
                                }
                                else
                                    currValue += item;
                            }
                            values.Add(currValue);
                            if (currValues.Count > 0)
                                instru.Cases.Add(new Tuple<List<IValue>, List<Instruction>>(currValues, currInstrus));
                            currValues = new List<IValue>();
                            currInstrus = new List<Instruction>();
                            foreach (var item in values)
                                currValues.Add(ParseValue(item, mainProg, currentBlock));
                        }
                        SkipLine(code, ref index);
                    }
                }
                return instru;
            }
            else if (code.Substring(index, currentBlock.Name.Length) == currentBlock.Name)
            {
                var instru = new Return();
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
                        if (instru2.Args.Count > instru2.CalledModule.Parameters.Count && !ignoreConsts)
                            throw new ILAException("Erreur : nombre d'arguments trop élevé");
                        for (int i = 0; i < instru2.Args.Count; i++)
                        {
                            if ((instru2.CalledModule.Parameters[i].Mode & Parameter.Flags.OUTPUT) != 0 && instru2.Args[i] is Variable v && v.Constant && !ignoreConsts)
                                throw new ILAException("Erreur : impossible de passer une variable constante en paramètre de sortie");
                        }
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
                if (!(currentBlock is Function) && !ignoreConsts)
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
                        if (instru.Left.Constant && !ignoreConsts)
                            throw new ILAException("Erreur : impossible de changer la valeur d'une constante");
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
                        if (instru.Args.Count > instru.CalledModule.Parameters.Count && !ignoreConsts)
                            throw new ILAException("Erreur : nombre d'arguments trop élevé");
                        for (int i = 0; i < instru.Args.Count; i++)
                        {
                            if ((instru.CalledModule.Parameters[i].Mode & Parameter.Flags.OUTPUT) != 0 && instru.Args[i] is Variable v && v.Constant && !ignoreConsts)
                                throw new ILAException("Erreur : impossible de passer une variable constante en paramètre de sortie");
                        }
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
                        throw new ILAException("Erreur de synthaxe");
                    }
                }
            }
        }
    }
}