using System;
using System.Collections.Generic;
using System.IO;

namespace SWE_Decoder
{
    class Program
    {
        private const bool useConsole = true;

        static void Main(string[] args)
        {
            ProblemInstance pi;
            String resultString1, resultString2, userSelection;

            if (useConsole)
            {
                Console.Title = "SWE Decoder";
                Console.SetWindowSize(100, 50);

                pi = LoadChecker.LoadAndCheckConsole();
                if (pi == null)
                    return;
                Console.WriteLine("press enter to exit, or t+enter to test the file");
                userSelection = Console.ReadLine();

                if (userSelection != "t")
                    return;           
            }
            else
                pi = LoadChecker.LoadAndCheckStandardInOut();
            
                        
            resultString1 = Solver.BruteForce(pi,true);


            if (useConsole)
            {
                if (resultString1.Contains("Fatal"))
                    Console.WriteLine(resultString1);
                else if (resultString1 == "NO")
                    Console.WriteLine("NO");
                else
                {
                    resultString2 = resultString1.Substring(3);
                    resultString1 = resultString1.Substring(0, 3);
                    Console.WriteLine(resultString1);
                    Console.WriteLine(resultString2);
                }
                Console.WriteLine("");
                Console.WriteLine("press enter to exit");
                Console.ReadLine();
            }
            else
            {
                Console.In.Close();
                if (resultString1 == "NO" || resultString1.Contains("Fatal"))
                    Console.Out.WriteLine("NO");
                else
                    Console.Out.WriteLine(resultString1.Substring(3));
                Console.Out.Close();
            }
        }
    }
}
