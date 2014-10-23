﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWE_Decoder
{
    public class Solver
    {
        public static String BruteForce(ProblemInstance pi, bool usePreproc) 
        {
            ProblemInstance ppi = pi;
            if (usePreproc)
            {
                //0:preprocessing - brug en række hurtige algoritmer til muligvis at falsificere
                ppi = Preprocessing(pi);
                if (ppi == null)
                    return "NO"; //if preprocessing fails (example: "A" can only be expanded to 'q', and s = "cdcdcdcd") the main function should return "NO"
            }
            String validationResult = "";
            bool noInterestingFound = true;
            //2.a:for hver kombination af oversættelser til listen i 1 opret en <char,char> dictionary (alle permutationer af oversættelser)
            foreach (Dictionary<Char, String> translation in FindInterestingTranslations(ppi))
            {
                validationResult = ppi.Validate(translation);
                if (validationResult == "YES")
                    return "YES" + translation;
                else
                    noInterestingFound = false; //return "NOO" + validationResult;
            }
            if (noInterestingFound)
                return "Fatal error! No interesting translations found!";
            else
                return "NO";
            //2.b:afprøv kombinationen, hvis den opfylder kravene i a.1


            //a.1: 
            //      findes oversættelsen i s (s=sdsdsd oversættelse: A=q -> skip fordi q ikke findes i s)  (pruning)
            //      lav pattern matching. Tjek om patterns såsom AA, AAA, A*A findes i s


            //3:for hver dictionary fra 2 se om alle t er substrings af s, ved at følge den givne oversættelse
        }

        private static List<Dictionary<Char, String>> FindInterestingTranslations(ProblemInstance pi)
        {
            Console.WriteLine("Starting \"Findings translations\"");
            List<Dictionary<char, string>> output = new List<Dictionary<char, string>>();
            List<Dictionary<char, string>> buffer;
            output.Add(new Dictionary<char, string>());
            // HACK: Brute Force Approach
            // TODO: denne funktion bruger kæmpe mængder memory (RAM)
            foreach (KeyValuePair<char, List<string>> kvp in pi.Expansion1)
            {
                buffer = new List<Dictionary<char, string>>();
                foreach (string s in kvp.Value)
                {
                    foreach (Dictionary<char, string> dict in output)
                    {
                        Dictionary<char, string> newdict = CloneDict(dict);
                        newdict.Add(kvp.Key, s);
                        buffer.Add(newdict);
                    }
                }
                output = buffer;
            }
            Console.WriteLine("Ending \"Findings translations\"");
            return output;
        }

        private static ProblemInstance Preprocessing(ProblemInstance pi)
        {
            Console.WriteLine("Starting \"Preprocessing\"");
            ProblemInstance ppi;
            ppi = Prune(pi);
            Console.WriteLine("Ending \"Pruning\"");
            if (ppi == null)
                return null;
            //if (PatternMatchingsNotFound(ppi))
            //    return null;
            // TODO: tjek på længden af translations vs længden af s
            // TODO: man kunne ligge permutations med meget lange translation bagerst så de bliver forsøgt validated sidst? (de er for det meste forkerte?)
            Console.WriteLine("Ending \"Pattern Mathcing\"");
            Console.WriteLine("Ending \"Preprocessing\"");
            return ppi;
        }

        #region preprocessing functions
        private static ProblemInstance Prune(ProblemInstance pi)
        {
            Console.WriteLine("Starting \"Pruning\"");
            Dictionary<Char, List<String>> prunedexp = new Dictionary<Char, List<String>>();
            List<String> prunedExpStrings = new List<String>();
            foreach (KeyValuePair<Char, List<String>> kvp in pi.Expansion1)
            {
                foreach (String exp in kvp.Value)
                {
                    if (pi.s.Contains(exp))
                        prunedExpStrings.Add(exp);
                }
                if (prunedExpStrings.Count < 1)
                    return null;
                prunedexp.Add(kvp.Key, prunedExpStrings);
                prunedExpStrings = new List<String>();
            }
            ProblemInstance ppi = new ProblemInstance(pi.k, pi.s, pi.t, prunedexp);
            return ppi;
        }

        private static bool PatternMatchingsNotFound(ProblemInstance pi)
        {
            Console.WriteLine("Starting \"Pattern Mathcing\"");
            if (pi == null)
                return false;

            MatchCollection tmc;
            //  TODO: add pattern matchings here

            MatchCollection smc = Regex.Matches(pi.s, "lav en collection af patterns af formen såsom aa, aaa, a*a (wildcard * må kun være et bogstav)");

            foreach (String test in pi.t)
            {
                if ((tmc = Regex.Matches(test, "find et pattern såsom AA, AAA, A*A (wildcard * må kun være et bogstav)")).Count > 0)
                {
                    foreach (Match m in tmc)
                    {
                        //hvis pattern ikke findes i smc => return true
                    }
                }
            }

            return false;//all patterns in t's also exists in s
        }
        #endregion

        #region helper functions
        private static bool IsCapital(char c)
        {
            return Regex.Match(c.ToString(), "[A-Z]").Length == 1;
        }

        private static Dictionary<char, string> CloneDict(Dictionary<char, string> dict)
        {
            Dictionary<char, string> newdict = new Dictionary<char, string>();
            foreach (KeyValuePair<char, string> kvp in dict) newdict.Add(kvp.Key, kvp.Value);
            return newdict;
        }
        #endregion
    }
}