using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Decoder.AlgoLib
{
    public class BruteForceAlgorithm1
    {
        public static string Run(ProblemInstance pi)
        {
            bool noInterestingFound = true;
            String validationResult = "";
            List<Dictionary<Char, String>> translations = FindInterestingTranslations(pi);
            foreach (Dictionary<Char, String> translation in translations)
            {
                validationResult = pi.Validate(translation);
                if (validationResult == "YES")
                    return "YES" + translation.ToPrintFormat();
                else
                    noInterestingFound = false;
            }
            if (noInterestingFound)
                return "Fatal error! No interesting translations found!";
            else
                return "NO";
        }


        private static List<Dictionary<Char, String>> FindInterestingTranslations(ProblemInstance pi)
        {
            Console.WriteLine("Starting \"Findings translations\"");
            List<Dictionary<char, string>> output = new List<Dictionary<char, string>>();
            List<Dictionary<char, string>> buffer;
            Dictionary<char, string> newdict;
            output.Add(new Dictionary<char, string>());
            // TODO: denne funktion bruger kæmpe mængder memory (RAM) og ender med at smide Out of Memory Exception hvis ikke computeren fryser først
            foreach (KeyValuePair<char, List<string>> kvp in pi.Expansion1)
            {
                buffer = new List<Dictionary<char, string>>();
                foreach (string s in kvp.Value)
                {
                    foreach (Dictionary<char, string> dict in output)
                    {
                        newdict = Solver.CloneDict(dict);
                        newdict.Add(kvp.Key, s);
                        buffer.Add(newdict);
                    }
                }
                output = buffer;
            }
            Console.WriteLine("Ending \"Findings translations\"");
            return output;
        }

    }
}
