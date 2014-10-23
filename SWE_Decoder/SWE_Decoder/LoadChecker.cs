using System;
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
        private static StreamReader file = null;
        private static TextReader standardIn = Console.In;

        private static ProblemInstance LoadAndCheck(bool useConsole)
        {
            int k = 0;
            String s = "";//lowercase only
            List<String> t = new List<string>();//both lower- and uppercase
            Dictionary<Char, List<String>> Expansion1 = new Dictionary<char, List<string>>();
            ProblemInstance pi;

            string line = "";
            int linecounter = 0, errorCounter = 0;

            if (useConsole)
            {
                Console.WriteLine("input name of file (without extension)");
                string filename = Console.ReadLine();
                if (!File.Exists(filename + ".SWE"))
                {
                    Console.WriteLine("file not found!");
                    Console.ReadLine();
                    return null;
                }
                file = new StreamReader(filename + ".SWE");
                Console.WriteLine();
            }

            while ((line = ReadLine(useConsole)) != null)
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
                            if (useConsole)
                            {
                                Console.WriteLine("k was not a number");
                                Console.Read();
                            }
                            return null;
                        }
                        break;
                    case 1:
                        if (Regex.Match(line, "[a-z]*").Length == line.Length)
                            s = line;
                        else
                        {
                            if (useConsole)
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
                                if (useConsole)
                                    Console.WriteLine("wrong input reading a t at line: " + linecounter);
                                errorCounter++;
                            }
                            if (i + 1 < k)
                            {
                                line = ReadLine(useConsole);
                                linecounter++;
                            }
                        }
                        break;
                    default:
                        if (line.Length < 3 || line.ElementAt(1) != ':')
                        {
                            if (useConsole)
                                Console.WriteLine("Expected translation but was of the wrong format. Possible incorrect k value.");
                            return null;
                        }
                        Char GammaChar = line.ElementAt(0);
                        if (Regex.Match(GammaChar.ToString(), "[A-Z]").Length == 0)
                        {
                            if (useConsole)
                                Console.WriteLine("Dictionary key invalid at line: " + linecounter + k);
                            errorCounter++;
                        }
                        String SigmaWords = line.Substring(2);
                        List<String> sWordList = SigmaWords.Split(',').ToList<String>();
                        foreach (String teststring in sWordList)
                        {
                            if (Regex.Match(teststring, "[a-z]*").Length == 0)
                            {
                                if (useConsole)
                                    Console.WriteLine("Listvalue invalid at line: " + linecounter);
                                errorCounter++;
                            }
                        }
                        Expansion1.Add(GammaChar, sWordList.ToList<String>());
                        break;
                }

                linecounter++;
            }

            if (useConsole)
                file.Close();

            pi = new ProblemInstance(k, s, t, Expansion1);

            if (useConsole)
            {
                Console.WriteLine(pi.ToString(false));
                Console.WriteLine("numbers of errors found: " + errorCounter);
                Console.WriteLine();
            }

            if (errorCounter > 0)
                pi = null;

            return pi;
        }

        #region helpers
        public static ProblemInstance LoadAndCheckConsole()
        {
            return LoadAndCheck(true);
        }
        public static ProblemInstance LoadAndCheckStandardInOut()
        {
            return LoadAndCheck(false);
        }

        private static string ReadLine(bool useConsole)
        {
            if (useConsole)
                return file.ReadLine();
            else
                return standardIn.ReadLine();
        }
        #endregion
    }
}
