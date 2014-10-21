﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWE_Decoder
{
    public class LoadChecker
    {
        public static ProblemInstance LoadAndCheck()
        {
            int k = 0;
            String s = "";//lowercase only
            List<String> t = new List<string>();//both lower- and uppercase
            Dictionary<Char, List<String>> Expansion1 = new Dictionary<char, List<string>>();
            ProblemInstance pi;

            string line = "";
            int linecounter = 0, errorCounter = 0;
            Console.WriteLine("input name of file (without extension)");
            string filename = Console.ReadLine();
            if (!File.Exists(filename + ".SWE"))
            {
                Console.WriteLine("file not found!");
                Console.ReadLine();
                return null;
            }
            StreamReader file = new StreamReader(filename + ".SWE");
            Console.WriteLine();
            while ((line = file.ReadLine()) != null)
            {
                switch (linecounter)
                {
                    case 0:
                        try
                        {
                            k = Convert.ToInt32(line);
                        }
                        catch
                        {
                            Console.WriteLine("k was not a number");
                            Console.Read();
                            return null;
                        }
                        break;
                    case 1:
                        if (Regex.Match(line, "[a-z]*").Length == line.Length)
                            s = line;
                        else
                        {
                            Console.WriteLine("s contained letters not in Sigma");
                            errorCounter++;
                        }
                        break;
                    case 2:
                        for (int i = 0; i < k; i++)
                        {
                            if (Regex.Match(line, "[a-zA-Z]*").Length == line.Length)
                                t.Add(line);
                            else
                            {
                                Console.WriteLine("wrong input reading t at line: " + linecounter);
                                errorCounter++;
                            }
                            if (i + 1 < k)
                            {
                                line = file.ReadLine();
                                linecounter++;
                            }
                        }
                        break;
                    default:
                        Char GammaChar = line.ElementAt(0);
                        if (Regex.Match(GammaChar.ToString(), "[A-Z]").Length == 0)
                        {
                            Console.WriteLine("Dictionary key invalid at line: " + linecounter + k);
                            errorCounter++;
                        }
                        String SigmaWords = line.Substring(2);
                        List<String> sWordList = SigmaWords.Split(',').ToList<String>();
                        foreach (String teststring in sWordList)
                        {
                            if (Regex.Match(teststring, "[a-z]*").Length == 0)
                            {
                                Console.WriteLine("Listvalue invalid at line: " + linecounter);
                                errorCounter++;
                            }
                        }
                        Expansion1.Add(GammaChar, sWordList.ToList<String>());
                        break;
                }

                linecounter++;
            }

            file.Close();

            pi = new ProblemInstance(k, s, t, Expansion1);

            Console.WriteLine(pi.ToString());

            Console.WriteLine("numbers of errors found: " + errorCounter);
            Console.WriteLine();
            return pi;
        }
    }
}
