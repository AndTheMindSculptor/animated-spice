using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE_Decoder
{
    class ProblemInstance
    {
        public int k = 0;
        public String s = "";//lowercase only
        public List<String> t = new List<string>();//both lower- and uppercase
        public Dictionary<Char, List<String>> Expansion1 = new Dictionary<char, List<string>>();

        public ProblemInstance(int k, string s, List<string> t, Dictionary<char, List<string>> Expansion1)
        {
            this.k = k;
            this.s = s;
            this.t = t;
            this.Expansion1 = Expansion1;
        }

        public override string ToString()
        {
            string ret = "";

            ret = String.Concat(ret, "k: "+k+"\n");
            ret = String.Concat(ret, "s: " + s + "\n");
            int count = 0, innercount = 0;
            foreach (String tString in t)
            {
                ret = String.Concat(ret, "t"+count+": " + tString + "\n");
                count++;
            }
            count = 0;
            foreach (KeyValuePair<Char, List<String>> kvp in Expansion1)
            {
                ret = String.Concat(ret, "gamma" + count + ": " + kvp.Key + "\n");
                foreach (String sSigma in kvp.Value)
                {
                    ret = String.Concat(ret, "---sigma expansion" + innercount + ": " + sSigma + "\n");
                    innercount++;
                }
                innercount = 0;
            }

            return ret;
        }
    }
}
