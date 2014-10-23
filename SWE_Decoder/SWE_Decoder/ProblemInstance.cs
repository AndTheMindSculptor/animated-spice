using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Decoder
{
    public class ProblemInstance
    {
        public int k = 0;
        public String s = "";//lowercase only
        public List<String> t = new List<String>();//both lower- and uppercase
        public Dictionary<Char, List<String>> Expansion1 = new Dictionary<Char, List<String>>();

        public ProblemInstance(int k, String s, List<String> t, Dictionary<Char, List<String>> Expansion1)
        {
            this.k = k;
            this.s = s;
            this.t = t;
            this.Expansion1 = Expansion1;
        }

        public String Validate(Dictionary<Char,String> assignment)
        {
            List<String> newt = new List<String>();
            String newString = "";

            foreach (String testt in t)
            {
                foreach (Char c in testt)
                {
                    if (Char.IsUpper(c))
                    {
                        if (!assignment.ContainsKey(c))
                            return "the given assignment does not contain translation for "+c;
                        newString = String.Concat(newString, assignment[c]);
                    }
                    else
                        newString = String.Concat(newString, c.ToString());
                }
                if (!s.Contains(newString))
                    return "the given assignment generated the string "+newString+" from "+testt+", which is not a substring of s.";

                newt.Add(newString);
                newString = "";
            }

            return "YES";
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
