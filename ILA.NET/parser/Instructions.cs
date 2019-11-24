﻿using System;
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
            else if (code.Substring(index, 3) == "si ")
            {
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
                SkipLine(code, ref index);
                instru.IfInstructions = new List<Instruction>();
                instru.Elif = new List<Tuple<IValue, List<Instruction>>>();
                instru.ElifComments = new List<string>();
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
            else if (code.Substring(index, 5) == "pour ")
            {
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
                while (code.Substring(index, 5) != " pas " &&
                    code.Substring(index, 6) != " faire")
                    endValue += code[index++];
                instru.End = ParseValue(endValue, mainProg, currentBlock);
                if (code.Substring(index, 5) == " pas ")
                {
                    index += 5;
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
                return instru;
            }
            return null;
        }
    }
}