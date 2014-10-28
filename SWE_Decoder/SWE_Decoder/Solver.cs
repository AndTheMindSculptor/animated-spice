using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SWE_Decoder.AlgoLib;

namespace SWE_Decoder
{
    public class Solver
    {
        private static bool UseConsole = false;

        public static String BruteForce(ProblemInstance pi, bool usePreproc, bool useConsole)
        {
            // HACK: Brute Force Approach
            ProblemInstance ppi = pi;
            UseConsole = useConsole;

            if (usePreproc)
            {
                //0:preprocessing - brug en række hurtige algoritmer til muligvis at falsificere
                ppi = Preprocessing(pi);
                if (ppi == null)
                    return "NO"; //if preprocessing fails (example: "A" can only be expanded to 'q', and s = "cdcdcdcd") the main function should return "NO"
                foreach (KeyValuePair<Char, List<String>> kvp in pi.Expansion1)
                {
                    if (!ppi.Expansion1.ContainsKey(kvp.Key))
                        ppi.UnussedGammas.Add(kvp.Key, kvp.Value[0]);
                }
            }

            //2.a:for hver kombination af oversættelser til listen i 1 opret en <char,char> dictionary (alle permutationer af oversættelser)
            return BruteForceAlgorithm6.Run(ppi);
            //2.b:afprøv kombinationen, hvis den opfylder kravene i a.1
                        

            //a.1: 
            //      findes oversættelsen i s (s=sdsdsd oversættelse: A=q -> skip fordi q ikke findes i s)  (pruning)
            //      lav pattern matching. Tjek om patterns såsom AA, AAA, A*A findes i s


            //3:for hver dictionary fra 2 se om alle t er substrings af s, ved at følge den givne oversættelse
        }



        private static ProblemInstance Preprocessing(ProblemInstance pi)
        {
            if (UseConsole) Console.WriteLine("Starting \"Preprocessing\"");
            ProblemInstance ppi = pi;
            ppi = Cut(pi);
            ppi = Prune(ppi);
            if (UseConsole) Console.WriteLine("Ending \"Pruning\"");
            if (ppi == null)
                return null;
            //if (PatternMatchingsNotFound(ppi))
            //    return null;
            // TODO: tjek på længden af translations vs længden af s
            // TODO: man kunne ligge permutations med meget lange translation bagerst så de bliver forsøgt validated sidst? (de er for det meste forkerte?)
            if (UseConsole) Console.WriteLine("Ending \"Preprocessing\"");
            return ppi;
        }

        #region preprocessing functions
        private static ProblemInstance Prune(ProblemInstance pi)
        {
            if (UseConsole) Console.WriteLine("Starting \"Pruning\"");
            Dictionary<Char, List<String>> prunedexp = new Dictionary<Char, List<String>>();
            HashSet<String> prunedExpStrings = new HashSet<String>();
            foreach (KeyValuePair<Char, List<String>> kvp in pi.Expansion1)
            {
                foreach (String exp in kvp.Value)
                {
                    if (pi.s.Contains(exp))
                        prunedExpStrings.Add(exp);
                }
                if (prunedExpStrings.Count < 1)
                    return null;
                prunedexp.Add(kvp.Key, prunedExpStrings.ToList());
                prunedExpStrings = new HashSet<String>();
            }
            ProblemInstance ppi = new ProblemInstance(pi.k, pi.s, pi.t, prunedexp);
            return ppi;
        }

        private static ProblemInstance Cut(ProblemInstance pi)
        {
            if (UseConsole) Console.WriteLine("Starting \"Cutting\"");
            List<Char> usedGammas = new List<Char>();
            Dictionary<Char, List<String>> CuttedDict = new Dictionary<Char, List<String>>();
            foreach (String str in pi.t) 
                foreach (char c in str) 
                    if(IsCapital(c)) usedGammas.Add(c);
            foreach (Char key in usedGammas)
            {
                if (!CuttedDict.Keys.Contains(key))
                    CuttedDict.Add(key, pi.Expansion1[key]);
            }
            if (UseConsole) Console.WriteLine("Ending \"Cutting\"");
            return new ProblemInstance(pi.k, pi.s, pi.t, CuttedDict);
        }
        #endregion

        #region helper functions
        public static bool IsCapital(char c)
        {
            return Regex.Match(c.ToString(), "[A-Z]").Length == 1;
        }

        public static Dictionary<char, string> CloneDict(Dictionary<char, string> dict)
        {
            Dictionary<char, string> newdict = new Dictionary<char, string>();
            foreach (KeyValuePair<char, string> kvp in dict) newdict.Add(kvp.Key, kvp.Value);
            return newdict;
        }
        #endregion
    }

    public static class DictExtensions
    {
        public static String ToPrintFormat(this Dictionary<Char,String> dict)
        {
            String ret = "";
            foreach(KeyValuePair<Char,String> kvp in dict)
                ret += "" + kvp.Key + ":" + kvp.Value+"\n";

            return ret;
        }
    }
}
