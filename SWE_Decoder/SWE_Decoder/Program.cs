using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace SWE_Decoder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SWE Decoder";
            Console.SetWindowSize(50, 50);

            int k = 0;
            String s = "";//lowercase only
            List<String> t = new List<string>();//both lower- and uppercase
            Dictionary<Char, List<String>> Expansion1 = new Dictionary<char, List<string>>();

            string line = "";
            int linecounter = 0, errorCounter = 0;
            Console.WriteLine("input name of file (without extension)");
            string filename = Console.ReadLine();
            if (!File.Exists(filename + ".SWE"))
            {
                Console.WriteLine("file not found!");
                Console.ReadLine();
                return;
            }
            StreamReader file = new StreamReader(filename+".SWE");//hard-coded file name   
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
                            return;
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
                        Expansion1.Add(GammaChar,sWordList.ToList<String>());
                        break;
                }

                linecounter++;
            }
            
            file.Close();

            ProblemInstance pi = new ProblemInstance(k,s,t,Expansion1);

            Console.WriteLine(pi.ToString());
            
            Console.WriteLine("numbers of errors found: "+errorCounter);
            Console.WriteLine();
            Console.WriteLine("press enter to exit, or t+enter to test the file");
            string userSelection = Console.ReadLine();

            if (userSelection != "t")
                return;

            //algo(pi);
            //recursive(pi.t.First(), pi.Expansion1);
            //Dictionary<Char,String> assignment = new Dictionary<Char,String>();
            //assignment.Add('A', "b");
            //assignment.Add('B', "d");
            //assignment.Add('C', "d");
            ////assignment.Add('D', "d");
            ////assignment.Add('E', "e");
            //Console.WriteLine(pi.Validate(assignment));

            Console.ReadLine();


            
        }



        private static void newAlgo() //<-- use this approach
        {
            //0:preprocessing - brug en række hurtige algoritmer til muligvis at falsificere

            //1:lav liste med alle store bogstaver der optræder i t

            //2.a:for hver kombination af oversættelser til listen i 1 opret en <char,char> dictionary (alle permutationer af oversættelser)

            //2.b:afprøv kombinationen, hvis den opfylder kravene i a.1





            //a.1: 
            //      findes oversættelsen i s (s=sdsdsd oversættelse: A=q -> skip fordi q ikke findes i s)  (pruning)
            //      lav pattern matching. Tjek om patterns såsom AA, AAA, A*A findes i s


            //3:for hver dictionary fra 2 se om alle t er substrings af s, ved at følge den givne oversættelse
        }

        private static void algo(ProblemInstance pi)
        {

        }

        private static void recursive(List<String> t, Dictionary<Char,Char> assignment, Dictionary<Char,List<String>> exp)
        {
            int whichString = -1, whichChar = -1;
            #region find index of a capital letter
            int stringCounter=0, charCounter=0;
            foreach (String s in t)
            {
                foreach (char c in s)
                {
                    if (IsCapital(c))
                    {
                        whichChar = charCounter;
                        whichString = stringCounter;
                        break;
                    }
                    charCounter++;
                }
                if (whichChar != -1)
                    break;
                stringCounter++;
            }
            #endregion
            
            //getting the letter
            char foundCapitalLetter = t[whichString].ElementAt(whichChar);

            //getting the expansions of the letter
            List<String> expansions = exp[foundCapitalLetter];

            foreach (String str in expansions)
            {
                //replace each instance of the chosen capital letter in all strings in t
                //with each possible expansion
            }
        }

        private static bool IsCapital(char c)
        {
            return Regex.Match(c.ToString(),"[A-Z]").Length == 1;
        }

    //    private static void algo(ProblemInstance pi)
    //    {
    //        bool answerIsYes = true;
    //        List<List<String>> perms = new List<List<string>>();
    //        foreach(String t in pi.t)
    //        {
    //            perms.Add(recursive(t,pi.Expansion1));
    //        }
    //        bool validPermFound;
    //        foreach (List<String> ls in perms)
    //        {
    //            validPermFound = false;
    //            foreach (String str in ls)
    //            {
    //                if (pi.s.Contains(str))
    //                {
    //                    validPermFound = true;
    //                    break;
    //                }
    //            }
    //            if (validPermFound == false)
    //            {
    //                answerIsYes = false;
    //                break;
    //            }
    //        }

    //        if (answerIsYes)
    //            Console.WriteLine("YES");
    //        else
    //            Console.WriteLine("NO");
    //    }

    //    private static List<String> recursive(String t, Dictionary<Char, List<String>> exp)
    //    {
            
    //        List<String> output = new List<string>();
    //        Char head = t.ElementAt(0);
    //        String tail = t.Substring(1);
    //        if(Regex.Match(head.ToString(),"[A-Z]").Length > 0)
    //        {
                
    //            foreach(String translation in exp[head])
    //            {
    //                if (tail.Length == 0)
    //                {
    //                    output.Add(translation);
    //                }
    //                else
    //                {
    //                    List<String> Substrings = recursive(tail,exp);
    //                    foreach(String rest in Substrings) output.Add(translation+rest);
    //                }
    //                //
    //            }
    //        }
    //        else
    //        {
    //            if (tail.Length == 0)
    //            {
    //                output.Add(head.ToString());
    //            }
    //            else
    //            {
    //                List<String> Substrings = recursive(tail,exp);
    //                foreach(String rest in Substrings) output.Add(head+rest);
    //            }
    //        }
    //        return output;

    //    }
    }
}
