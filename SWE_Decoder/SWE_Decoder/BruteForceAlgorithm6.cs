using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWE_Decoder.AlgoLib
{
    public class BruteForceAlgorithm6
    {
        private const int GAMMA_START_INDEX_TO_PARTIAL_VALIDATE_FROM = 3;
        private static int Partial_validate_delay = 0;

        private static ProblemInstance Problem;

        private static Object currentIndexLock = new Object();
        private static int[] currentIndexOf;
        public static int[] CurrentIndexOf
        {
            get 
            {
                lock (currentIndexLock)
                {
                    return BruteForceAlgorithm6.currentIndexOf;
                }
            }
            set 
            {
                lock (currentIndexLock)
                {
                    BruteForceAlgorithm6.currentIndexOf = value;
                }
            }
        }

        private static int[] maxForIndex;

        private static bool ValidationFound = false;
        private static Dictionary<Char, String> foundTranslation;

        private static Object mergedDictLock = new Object();
        private static Dictionary<Char, String> mergedDict = new Dictionary<Char, String>();
        private static Dictionary<Char, String> MergedDict
        {
            get 
            {
                lock (mergedDictLock)
                {
                    return mergedDict;
                }
            }
            set 
            {
                lock (mergedDictLock)
                {
                    mergedDict = value;
                }
            }
        }

        private static Object completedThreadsCounterLock = new Object();
        private static int completedThreadsCounter = 0;
        public static int CompletedThreadsCounter
        {
            get 
            {
                lock (completedThreadsCounterLock)
                {
                    return BruteForceAlgorithm6.completedThreadsCounter;
                }
            }
            set 
            {
                lock (completedThreadsCounterLock)
                {
                    BruteForceAlgorithm6.completedThreadsCounter = value;
                }
            }
        }
        private static void IncrementCompletedThreadsCounter()
        {
            lock (completedThreadsCounterLock)
            {
                CompletedThreadsCounter++;
            }
        }

        private static System.ComponentModel.BackgroundWorker[] BackgroundWorkers;
                
        public static string Run(ProblemInstance pi)
        {
            Problem = pi;

            int numberOfGammas = Problem.Expansion1.Count();

            CurrentIndexOf = new int[numberOfGammas];
            for (int i = 0; i < numberOfGammas; i++ )
                CurrentIndexOf[i] = 0;

            maxForIndex = new int[numberOfGammas];
            for (int i = 0; i < numberOfGammas; i++)
                maxForIndex[i] = Problem.Expansion1.ElementAt(i).Value.Count();

            Thread[] threads = new Thread[maxForIndex[0]];
            BackgroundWorkers = new System.ComponentModel.BackgroundWorker[maxForIndex[0]];
            //Thread t;

            for (int i = 0; i < maxForIndex[0]; i++)
            {
                //disable partial validation når man tester om multithreading giver nogen fordel, og se på test06 (hvis multithreading, så skal ting som CurrentIndexOf være thread-safe)
                BackgroundWorkers[i] = new System.ComponentModel.BackgroundWorker();
                BackgroundWorkers[i].RunWorkerAsync();
            }

            //returns too early. Need to wait for threads to terminate before returning.

            return "threading";
        }        

        private static void recurse(int nextCurrentIndex)
        {
            if (ValidationFound)
                return;

            Dictionary<Char, String> translation;
            int counter;
            if (maxForIndex.Length <= nextCurrentIndex)
                return;
            for (int i = 0; i < maxForIndex[nextCurrentIndex]; i++)
            {
                if (ValidationFound)
                    return;
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
                    if (Problem.PartialValidate(translation) == true)
                    {
                        Partial_validate_delay++;
                        recurse(nextCurrentIndex + 1);
                    }
                }
                else
                    recurse(nextCurrentIndex + 1);

                CurrentIndexOf[nextCurrentIndex]++;
            }
            CurrentIndexOf[nextCurrentIndex] = 0;
        }

        private static void DoWork(Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            recurse(1);

            CurrentIndexOf[0]++;
            if (ValidationFound)
            {
                foreach (var v in foundTranslation)
                    MergedDict.Add(v.Key, v.Value);
                foreach (var v in Problem.UnussedGammas)
                    MergedDict.Add(v.Key, v.Value);

                var list = MergedDict.Keys.ToList();
                list.Sort();

                Dictionary<Char, String> final = new Dictionary<Char, String>();

                foreach (var key in list)
                    final.Add(key, MergedDict[key]);


                e.Result = "YES\n" + final.ToPrintFormat();
            }
            else
                e.Result = "NO";
        }

        private static void RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            IncrementCompletedThreadsCounter();
            if (CompletedThreadsCounter >= BackgroundWorkers.Length)
                Console.Out.WriteLine(e.Result);
        }

        private static void InitializeBackgroundWorkers()
        {
            // Attach event handlers to the BackgroundWorker object.
            foreach(System.ComponentModel.BackgroundWorker bgw in BackgroundWorkers)
            {
                bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(DoWork);
                bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(RunWorkerCompleted);
            }            
        }
    }
}
