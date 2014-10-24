using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Decoder.AlgoLib
{
    public class BruteForceAlgorithm4
    {
        public static string Run(ProblemInstance pi)
        {
            String validationResult = "", currentlyChosenTranslation;
            List<String> possibleTranslations = new List<String>();
            Dictionary<Char, String> translation = new Dictionary<Char, String>(); ;
            int numberOfGammas = pi.Expansion1.Count();
            int[] currentIndexOf = new int[numberOfGammas];
            for (int i = 0; i < numberOfGammas; i++ )
                currentIndexOf[i] = 0;

            int[] maxForIndex = new int[numberOfGammas];
            for (int i = 0; i < numberOfGammas; i++)
                maxForIndex[i] = pi.Expansion1.Values.Count();

            while (true)
            {
                for (int i = 0; i < numberOfGammas; i++)
                {
                    possibleTranslations = pi.Expansion1.ElementAt(i).Value;
                    for (int j = 0; j < maxForIndex[i]; j++)
                    {
                        currentlyChosenTranslation = possibleTranslations.ElementAt(currentIndexOf[j]);
                        translation.Add(pi.Expansion1.ElementAt(i).Key, currentlyChosenTranslation);
                        //if (translation.Count == numberOfGammas)
                        //    validationResult = pi.Validate(translation);
                    }
                    currentIndexOf[i]++;
                }

                //validationResult = pi.Validate(translation);

                if (validationResult == "YES")
                    return "YES" + translation.ToPrintFormat();
                //else
                //    return "NO";
                translation.Clear();
            }
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


   




            Console.WriteLine("Ending \"Findings translations\"");
            return output;
        }
    }
}
