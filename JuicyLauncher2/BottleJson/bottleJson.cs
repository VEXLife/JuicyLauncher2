using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BottleJson
{
    public sealed class bottleJson
    {
        public static String version = "1.2";

        public Boolean checkJson(String NativeJson)
        {
            int fcounter = 0;
            int acounter = 0;
            Boolean mistaked = false;
            fcounter = NativeJson.Split("{".ToCharArray()).Length;
            acounter = NativeJson.Split("}".ToCharArray()).Length;
            if (fcounter != acounter)
            {
                mistaked = true;
            }
            fcounter = NativeJson.Split("[".ToCharArray()).Length;
            acounter = NativeJson.Split("]".ToCharArray()).Length;
            if (fcounter != acounter)
            {
                mistaked = true;
            }
            /*
            fcounter = NativeJson.Split("(".ToCharArray()).Length;
            acounter = NativeJson.Split(")".ToCharArray()).Length;
            if (fcounter != acounter) {
                mistaked = true;
            }
            */
            if (mistaked == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Boolean checkJson(String NativeJson, int pos)
        {
            int fcounter = 0;
            int acounter = 0;
            Boolean mistaked = false;
            fcounter = NativeJson.Split("{".ToCharArray()).Length;
            acounter = NativeJson.Split("}".ToCharArray()).Length;
            if (fcounter != acounter - 2 && pos == 0)
            {
                mistaked = true;
            }
            if (fcounter - 2 != acounter && pos == 1)
            {
                mistaked = true;
            }
            fcounter = NativeJson.Split("[".ToCharArray()).Length;
            acounter = NativeJson.Split("]".ToCharArray()).Length;
            if (fcounter != acounter - 1 && pos == 0)
            {
                mistaked = true;
            }
            if (fcounter - 1 != acounter && pos == 1)
            {
                mistaked = true;
            }
            /*
            fcounter = NativeJson.Split("(".ToCharArray()).Length;
            acounter = NativeJson.Split(")".ToCharArray()).Length;
            if (fcounter != acounter-1 && pos==0) {
                mistaked = true;
            }
            if (fcounter-1 != acounter && pos==1) {
                mistaked = true;
            }
            */
            if (mistaked == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string buildJson(string source)
        {
            var fr = source;
            int startInd = 0;
            int endInd = 0;
            while (fr.Substring(endInd+1).Contains("\""))
            {
                startInd = fr.IndexOf("\"", endInd+1)+1;
                endInd = fr.IndexOf("\"", startInd);
                fr = fr.Substring(0, startInd) + fr.Substring(startInd, endInd - startInd).Replace("{", "▁").Replace("[", "▂").Replace("]", "▃").Replace("}", "▄").Replace(",", "▅") + fr.Substring(endInd);
            }
            return fr;
        }
    }
}
