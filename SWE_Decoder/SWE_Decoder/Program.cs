using System;
using System.Collections.Generic;
using System.IO;

namespace SWE_Decoder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SWE Decoder"; 
            Console.SetWindowSize(100, 50);

            ProblemInstance pi = LoadChecker.LoadAndCheck();
            if (pi == null)
                return;

            Console.WriteLine("press enter to exit, or t+enter to test the file");
            string userSelection = Console.ReadLine();

            if (userSelection != "t")
                return;            

            String resultString1, resultString2;
            resultString1 = Solver.newAlgo(pi);

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
        

        
    }
}
