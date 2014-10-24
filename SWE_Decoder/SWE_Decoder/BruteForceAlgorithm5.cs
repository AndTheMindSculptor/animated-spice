using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Decoder.AlgoLib
{
    public class BruteForceAlgorithm5
    {
        private const int GAMMA_START_INDEX_TO_PARTIAL_VALIDATE_FROM = 4;
        private static int Partial_validate_delay = 0;

        private static ProblemInstance Problem = null;
        private static int[] CurrentIndexOf, MaxForIndex;        
        private static bool ValidationFound = false;
        private static Dictionary<Char, String> foundTranslation;

                
        public static string Run(ProblemInstance pi)
        {
            Problem = pi;

            int numberOfGammas = Problem.Expansion1.Count();

            CurrentIndexOf = new int[numberOfGammas];
            for (int i = 0; i < numberOfGammas; i++ )
                CurrentIndexOf[i] = 0;

            MaxForIndex = new int[numberOfGammas];
            for (int i = 0; i < numberOfGammas; i++)
                MaxForIndex[i] = Problem.Expansion1.ElementAt(i).Value.Count();

            for (int i = 0; i < MaxForIndex[0]; i++)
            {
                if (ValidationFound)
                    return "YES" + foundTranslation.ToPrintFormat();
                recurse(1);
                CurrentIndexOf[0]++;
            }

            return "NO";
        }


        private static void recurse(int nextCurrentIndex)
        {
            if (ValidationFound)
                return;

            Dictionary<Char, String> translation;
            int counter;
            if (MaxForIndex.Length <= nextCurrentIndex)
                return;
            for (int i = 0; i < MaxForIndex[nextCurrentIndex]; i++)
            {
                translation = new Dictionary<Char, String>();
                counter = 0;
                if (nextCurrentIndex+1 == CurrentIndexOf.Length)
                {
                    foreach (int j in CurrentIndexOf)
                    {
                        translation.Add(Problem.Expansion1.ElementAt(counter).Key, Problem.Expansion1.ElementAt(counter).Value[j]);
                        counter++;
                    }
                    if (Problem.Validate(translation))
                    {
                        ValidationFound = true;
                        foundTranslation = translation;
                        return;
                    }        
                }
                else if (nextCurrentIndex > GAMMA_START_INDEX_TO_PARTIAL_VALIDATE_FROM + Partial_validate_delay)
                {
                    foreach (int j in CurrentIndexOf)
                    {
                        if (counter > nextCurrentIndex)
                            break;
                        translation.Add(Problem.Expansion1.ElementAt(counter).Key, Problem.Expansion1.ElementAt(counter).Value[j]);
                        counter++;
                    }
                    if (Problem.PartialValidate(translation) == false)
                        return;
                    else
                        Partial_validate_delay++;
                    recurse(nextCurrentIndex + 1);
                }
                else
                    recurse(nextCurrentIndex + 1);

                CurrentIndexOf[nextCurrentIndex]++;
            }
            CurrentIndexOf[nextCurrentIndex] = 0;
        }
    }
}
