using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Decoder.AlgoLib
{
    public class BruteForceAlgorithm3
    {        
        public static string Run(ProblemInstance pi)
        {
            bool noInterestingFound = true, notEnd = true;
            String validationResult = "";
            int expansionIndex = 0;
            List<Dictionary<Char, String>> translations;
            while(true)
            {
                translations = FindInterestingTranslations(pi, expansionIndex);
                expansionIndex++;
                if (translations == null)
                    break;

                foreach (Dictionary<Char, String> translation in translations)
                {
                    //validationResult = pi.Validate(translation);
                    if (validationResult == "YES")
                        return "YES" + translation.ToPrintFormat();
                    else
                        noInterestingFound = false;
                }
            }
            if (noInterestingFound)
                return "NO";//"Fatal error! No interesting translations found!";
            else
                return "NO";
        }

        //optimalt: find 1 permutation, test den, find 1 permutation, test den etc

        private static List<Dictionary<Char, String>> FindInterestingTranslations(ProblemInstance pi, int expansionIndex)
        {
            if (expansionIndex >= pi.Expansion1.Count())
                return null;
            Console.WriteLine("Starting \"Findings translations\"");
            List<Dictionary<char, string>> output = new List<Dictionary<char, string>>();
            List<Dictionary<char, string>> buffer;
            Dictionary<char, string> newdict;
            output.Add(new Dictionary<char, string>());

            //foreach (KeyValuePair<char, List<string>> kvp in pi.Expansion1)
            //{
                buffer = new List<Dictionary<char, string>>();
                foreach (string s in pi.Expansion1.ElementAt(expansionIndex).Value)
                {
                    foreach (Dictionary<char, string> dict in output)
                    {
                        newdict = Solver.CloneDict(dict);
                        newdict.Add(pi.Expansion1.ElementAt(expansionIndex).Key, s);
                        buffer.Add(newdict);
                    }
                }
                output = buffer;
            //}
            Console.WriteLine("Ending \"Findings translations\"");
            return output;
        }
    }
}
