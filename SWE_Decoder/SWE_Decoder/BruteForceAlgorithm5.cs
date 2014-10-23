using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Decoder.AlgoLib
{
    public class BruteForceAlgorithm5
    {
        private static ProblemInstance Problem = null;
        private static int[] CurrentIndexOf, MaxForIndex;
        private static int NumberOfPerms = 0;
        private static List<String> permStrings = new List<String>();

        // TODO: Currently only works for "square" problems (if each gamma variable have as many translations as there is gamma variables). FIX IT
        public static string Run(ProblemInstance pi)
        {
            Problem = pi;

            int numberOfGammas = Problem.Expansion1.Count();
            int[] currentIndexOf = new int[numberOfGammas];
            for (int i = 0; i < numberOfGammas; i++ )
                currentIndexOf[i] = 0;
            CurrentIndexOf = currentIndexOf;

            int[] maxForIndex = new int[numberOfGammas];
            for (int i = 0; i < numberOfGammas; i++)
                maxForIndex[i] = Problem.Expansion1.Values.Count();
            MaxForIndex = maxForIndex;

            for (int i = 0; i < MaxForIndex[0]; i++)
            {
                recurse(1);
                CurrentIndexOf[0]++;
            }

            return "NO";
        }

        private static void recurse(int nextCurrentIndex)
        {
            string s;
            int counter;
            if (MaxForIndex.Length <= nextCurrentIndex)
                return;
            for (int i = 0; i < MaxForIndex[nextCurrentIndex]; i++)
            {
                if (nextCurrentIndex+1 == CurrentIndexOf.Length)
                {
                    s = "";
                    counter = 0;
                    foreach (int j in CurrentIndexOf)
                    {
                        s += Problem.Expansion1.ElementAt(counter).Value[j]+" ";
                        counter++;
                    }
                    permStrings.Add(s);
                    NumberOfPerms++;
                    //permStrings.Add(Problem.Expansion1.ElementAt(0).Value[CurrentIndexOf[0]] + " " + Problem.Expansion1.ElementAt(1).Value[CurrentIndexOf[1]] + " " + Problem.Expansion1.ElementAt(2).Value[CurrentIndexOf[2]]);//permStrings.Add(""+CurrentIndexOf[0]+" "+CurrentIndexOf[1]+" "+CurrentIndexOf[2]);//NumberOfPerms++;
                }
                else
                    recurse(nextCurrentIndex + 1);

                CurrentIndexOf[nextCurrentIndex]++;
            }
            CurrentIndexOf[nextCurrentIndex] = 0;
        }
    }
}
