using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Decoder.AlgoLib
{
    public class BruteForceAlgorithm2
    {
        private static List<Dictionary<Char, String>> Translations;
        private static ProblemInstance givenProblem;

        public static string Run(ProblemInstance pi)
        {
            // denne funktion virker ikke pga ændring i collection under foreach (i recursion)
            String validationResult = "";
            StartAddingPermutations(pi);
            bool noInterestingFound = true;
            foreach (Dictionary<Char, String> translation in Translations)
            {
                //validationResult = pi.Validate(translation);
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

        #region recursively adding perms
        private static void StartAddingPermutations(ProblemInstance pi)
        {
            givenProblem = pi;
            Translations = new List<Dictionary<Char, String>>();
            Translations.Add(new Dictionary<Char, String>());
            RecursivelyAddPermutations(new Dictionary<Char, String>(), givenProblem.Expansion1.First().Key);
        }

        private static void RecursivelyAddPermutations(Dictionary<Char, String> baseDict, Char nextLetter)
        {
            Dictionary<Char, String> newDict = Solver.CloneDict(baseDict);
            List<Dictionary<Char, String>> buffer = new List<Dictionary<Char, String>>();
            Dictionary<Char, String> originalBaseDict = Solver.CloneDict(baseDict);

            foreach (String possibleTranslation in givenProblem.Expansion1[nextLetter])
            {
                baseDict.Add(nextLetter, possibleTranslation);
                buffer.Add(baseDict);
                baseDict = new Dictionary<Char, String>();
            }

            Translations.AddRange(buffer);
            buffer = null;

            Char nextNextLetter = GetNextLetterInGivenProblem(nextLetter);
            foreach (Dictionary<Char, String> dict in Translations)
            {
                if (nextNextLetter == '*')
                    break;
                else
                {
                    RecursivelyAddPermutations(dict, nextNextLetter);
                }
            }
            Translations.Remove(originalBaseDict);
            //dict_list.remove (base)

            //clone base 

            //buffer

            //for each possible translation of nextL
            //{
            //    buffer.add ( base.add translation_i )
            //}

            //dict_list.addRange(buffer)

            //for each dict in dict_list
            //{
            //    recur(dict_i, nextL.getNextLeter)
            //}
        }
        #endregion

        #region helpers
        private static Char GetNextLetterInGivenProblem(Char currentLetter)
        {
            bool currentLetterFound = false;
            foreach (KeyValuePair<Char, List<String>> kvp in givenProblem.Expansion1)
            {
                if (currentLetterFound)
                    return kvp.Key;
                if (kvp.Key == currentLetter)
                    currentLetterFound = true;
            }
            return '*';//Wont happen until we are at the last letter in the expansion
        }
        #endregion
    }
}
