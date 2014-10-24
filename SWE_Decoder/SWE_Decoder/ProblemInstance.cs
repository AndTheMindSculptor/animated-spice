using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SWE_Decoder
{
    public class ProblemInstance
    {
        public int k = 0;
        public String s = "";//lowercase only
        public List<String> t = new List<String>();//both lower- and uppercase
        public Dictionary<Char, List<String>> Expansion1 = new Dictionary<Char, List<String>>();
        public Dictionary<Char, String> UnussedGammas = new Dictionary<Char, String>();
        
        public ProblemInstance(int k, String s, List<String> t, Dictionary<Char, List<String>> Expansion1)
        {
            this.k = k;
            this.s = s;
            this.t = t;
            this.Expansion1 = Expansion1;
        }

        public bool Validate(Dictionary<Char,String> assignment)
        {
            String newString = "";

            foreach (String testt in t)
            {
                foreach (Char c in testt)
                {
                    if (Char.IsUpper(c))
                    {
                        if (!assignment.ContainsKey(c))
                            return false;// "the given assignment does not contain translation for " + c;
                        newString = String.Concat(newString, assignment[c]);
                    }
                    else
                        newString = String.Concat(newString, c.ToString());
                }
                if (!s.Contains(newString))
                    return false;//"the given assignment generated the string "+newString+" from "+testt+", which is not a substring of s.";

                newString = "";
            }

            return true;//"YES";
        }

        public bool PartialValidate(Dictionary<Char, String> assignment)
        {
            String partialString = "";

            foreach (String testt in t)
            {
                foreach (Char c in testt)
                {
                    if (Char.IsUpper(c))
                    {
                        if (assignment.ContainsKey(c))
                            partialString = String.Concat(partialString, assignment[c]);
                        else
                            partialString = String.Concat(partialString, '.');
                    }
                    else
                        partialString = String.Concat(partialString, c.ToString());
                }
                if (Regex.Match(s, partialString).Success == false)
                    return false;

                partialString = "";
            }

            return true;
        }

        public override string ToString()
        {
            return this.ToString(false);
        }
        public string ToString(bool verbose)
        {
            string ret = "";

            ret = String.Concat(ret, "1.   k: "+k+"\n");
            ret = String.Concat(ret, "2.   s: " + s + "\n");
            int count = 0, innercount = 0, countOld = 0;
            foreach (String tString in t)
            {
                ret = String.Concat(ret, ""+(count+2)+".   t" + count + ": " + tString + "\n");
                count++;
            }
            countOld = count;
            count = 0;
            foreach (KeyValuePair<Char, List<String>> kvp in Expansion1)
            {
                ret = String.Concat(ret, "" + (countOld + count + 2) + ".   gamma" + count + ": " + kvp.Key + "\n");
                if (verbose)
                {
                    foreach (String sSigma in kvp.Value)
                    {
                        ret = String.Concat(ret, "---sigma expansion" + innercount + ": " + sSigma + "\n");
                        innercount++;
                    }
                    innercount = 0;
                }
                count++;
            }

            return ret;
        }
    }
}
