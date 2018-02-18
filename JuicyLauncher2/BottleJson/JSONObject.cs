using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BottleJson
{
    public sealed class JSONObject
    {
        private String pcontents = "";

        public JSONObject(String contents)
        {
            pcontents = contents;
            pcontents = new bottleJson().buildJson(pcontents);
        }

        public JSONObject()
        {
            pcontents = "{}";
            pcontents = new bottleJson().buildJson(pcontents);
        }

        public String getString(String key)
        {
            int startInd = 0;
            String tc = "{[" + pcontents + "]}";
            while (tc.Substring(startInd).Contains("\"" + key + "\""))
            {
                startInd = tc.IndexOf("\"" + key + "\":", startInd) + 1;
                if (new bottleJson().checkJson(" " + tc.Substring(startInd, tc.Length - startInd) + " ", 0) && new bottleJson().checkJson(" " + tc.Substring(0, startInd) + " ", 1))
                {
                    startInd = tc.IndexOf("\"", startInd + key.Length + 2) + 1;
                    String fr = tc.Substring(startInd, tc.IndexOf("\"", startInd) - startInd).Replace("\"", "");
                    return fr.Replace("▁", "{").Replace("▂", "[").Replace("▃", "]").Replace("▄", "}").Replace("▅", ",");
                }
            }
            return "";
        }

        public void putString(String key, String value)
        {
            if (!this.Contains(key))
            {
                pcontents = pcontents.Substring(0, pcontents.Length - 1) + ",\"" + key + "\": \"" + value + "\"}";
                pcontents = Regex.Replace(pcontents, "\\{\\s*,", "{");
                format();
                return;
            }
            int startInd = 0;
            String tc = "{[" + pcontents + "]}";
            while (tc.Substring(startInd).Contains("\"" + key + "\""))
            {
                startInd = tc.IndexOf("\"" + key + "\":", startInd) + 1;
                if (new bottleJson().checkJson(" " + tc.Substring(startInd, tc.Length - startInd) + " ", 0) && new bottleJson().checkJson(" " + tc.Substring(0, startInd) + " ", 1))
                {
                    startInd = tc.IndexOf("\"", startInd + key.Length + 2) + 1;
                    pcontents = tc.Substring(2, startInd - 2) + value + tc.Substring(tc.IndexOf("\"", startInd), tc.Length - 2 - tc.IndexOf("\"", startInd));
                }
            }
            format();
        }

        public JSONObject getJSONObject(String key)
        {
            int startInd = 0;
            while (pcontents.Substring(startInd).Contains("\"" + key + "\""))
            {
                startInd = pcontents.IndexOf("\"" + key + "\":", startInd) + 1;
                if (new bottleJson().checkJson(" {" + pcontents.Substring(startInd, pcontents.Length - startInd) + " ") && new bottleJson().checkJson(" " + pcontents.Substring(0, startInd) + "} "))
                {
                    startInd = pcontents.IndexOf("{", startInd + key.Length + 2);
                    int endInd = startInd;
                    while (pcontents.Substring(endInd + 1).Contains("}"))
                    {
                        endInd = pcontents.IndexOf("}", endInd + 1);
                        if (new bottleJson().checkJson(" {" + pcontents.Substring(endInd + 1, pcontents.Length - endInd - 1) + " ") && new bottleJson().checkJson(" " + pcontents.Substring(0, endInd + 1) + "} "))
                        {
                            break;
                        }
                    }
                    String fr = pcontents.Substring(startInd, endInd + 1 - startInd);
                    JSONObject jo = new JSONObject(fr.Replace("▁", "{").Replace("▂", "[").Replace("▃", "]").Replace("▄", "}").Replace("▅", ","));
                    return jo;
                }
            }
            return null;
        }

        public void putJSONObject(String key, JSONObject value)
        {
            if (!this.Contains(key))
            {
                pcontents = pcontents.Substring(0, pcontents.Length - 1) + ",\"" + key + "\": " + value.toString() + "}";
                pcontents = Regex.Replace(pcontents, "\\{\\s*,", "{");
                format();
                return;
            }
            int startInd = 0;
            while (pcontents.Substring(startInd).Contains("\"" + key + "\""))
            {
                startInd = pcontents.IndexOf("\"" + key + "\":", startInd) + 1;
                if (new bottleJson().checkJson(" {" + pcontents.Substring(startInd, pcontents.Length - startInd) + " ") && new bottleJson().checkJson(" " + pcontents.Substring(0, startInd) + "} "))
                {
                    startInd = pcontents.IndexOf("{", startInd + key.Length + 2);
                    int endInd = startInd;
                    while (pcontents.Substring(endInd + 1).Contains("}"))
                    {
                        endInd = pcontents.IndexOf("}", endInd + 1);
                        if (new bottleJson().checkJson(" {" + pcontents.Substring(endInd + 1, pcontents.Length - endInd - 1) + " ") && new bottleJson().checkJson(" " + pcontents.Substring(0, endInd + 1) + "} "))
                        {
                            break;
                        }
                    }
                    pcontents = pcontents.Substring(0, startInd) + value.toString() + pcontents.Substring(endInd + 1);
                }
            }
            format();
        }

        public JSONArray getJSONArray(String key)
        {
            int startInd = 0;
            while (pcontents.Substring(startInd).Contains("\"" + key + "\""))
            {
                startInd = pcontents.IndexOf("\"" + key + "\":", startInd) + 1;
                if (new bottleJson().checkJson(" {" + pcontents.Substring(startInd, pcontents.Length - startInd) + " ") && new bottleJson().checkJson(" " + pcontents.Substring(0, startInd) + "} "))
                {
                    startInd = pcontents.IndexOf("[", startInd + key.Length + 2);
                    int endInd = startInd;
                    while (pcontents.Substring(endInd + 1).Contains("]"))
                    {
                        endInd = pcontents.IndexOf("]", endInd + 1);
                        if (new bottleJson().checkJson(" {" + pcontents.Substring(endInd + 1, pcontents.Length - endInd - 1) + " ") && new bottleJson().checkJson(" " + pcontents.Substring(0, endInd + 1) + "} "))
                        {
                            break;
                        }
                    }
                    String fr = pcontents.Substring(startInd, endInd + 1 - startInd);
                    JSONArray ja = new JSONArray(fr.Replace("▁", "{").Replace("▂", "[").Replace("▃", "]").Replace("▄", "}").Replace("▅", ","));
                    return ja;
                }
            }
            return null;
        }

        public void putJSONArray(String key, JSONArray value)
        {
            if (!this.Contains(key))
            {
                pcontents = pcontents.Substring(0, pcontents.Length - 1) + ",\"" + key + "\": " + value.toString() + "}";
                pcontents = Regex.Replace(pcontents, "\\{\\s*,", "{");
                format();
                return;
            }
            int startInd = 0;
            while (pcontents.Substring(startInd).Contains("\"" + key + "\""))
            {
                startInd = pcontents.IndexOf("\"" + key + "\":", startInd) + 1;
                if (new bottleJson().checkJson(" {" + pcontents.Substring(startInd, pcontents.Length - startInd) + " ") && new bottleJson().checkJson(" " + pcontents.Substring(0, startInd) + "} "))
                {
                    startInd = pcontents.IndexOf("[", startInd + key.Length + 2);
                    int endInd = startInd;
                    while (pcontents.Substring(endInd + 1).Contains("]"))
                    {
                        endInd = pcontents.IndexOf("]", endInd + 1);
                        if (new bottleJson().checkJson(" [" + pcontents.Substring(endInd + 1, pcontents.Length - endInd - 1) + " ") && new bottleJson().checkJson(" " + pcontents.Substring(0, endInd + 1) + "] "))
                        {
                            break;
                        }
                    }
                    pcontents = pcontents.Substring(0, startInd) + value.toString() + pcontents.Substring(endInd + 1);
                }
            }
            format();
        }

        public void putObject(String key, String value)
        {
            if (!this.Contains(key))
            {
                pcontents = pcontents.Substring(0, pcontents.Length - 1) + ",\"" + key + "\": " + value + "}";
                pcontents = Regex.Replace(pcontents, "\\{\\s*,", "{");
                format();
                return;
            }
            int startInd = 0;
            String tc = "{[" + pcontents + "]}";
            while (tc.Substring(startInd).Contains("\"" + key + "\""))
            {
                startInd = tc.IndexOf("\"" + key + "\":", startInd) + 1;
                if (new bottleJson().checkJson(" " + tc.Substring(startInd, tc.Length - startInd) + " ", 0) && new bottleJson().checkJson(" " + tc.Substring(0, startInd) + " ", 1))
                {
                    startInd = startInd + key.Length + 1;
                    if (value == null)
                    {
                        pcontents = tc.Substring(2, tc.Replace("[", ",").Replace("{", ",").LastIndexOf(",", startInd) - 1) + tc.Substring(pcontents.Replace("]", ",").Replace("}", ",").IndexOf(",", startInd) + 2, tc.Length - 2 - pcontents.Replace("]", ",").Replace("}", ",").IndexOf(",", startInd) - 2);
                        pcontents = Regex.Replace(pcontents, "\\{\\s*,", "{");
                        pcontents = Regex.Replace(pcontents, ",\\s*\\}", "}");
                    }
                    else
                    {
                        pcontents = tc.Substring(2, startInd - 1) + value + tc.Substring(pcontents.Replace("]", ",").Replace("}", ",").IndexOf(",", startInd) + 2, tc.Length - 2 - (pcontents.Replace("]", ",").Replace("}", ",").IndexOf(",", startInd) + 2));
                    }
                }
            }
            format();
        }

        public String getObject(String key)
        {
            int startInd = 0;
            String tc = "{[" + pcontents + "]}";
            while (tc.Substring(startInd).Contains("\"" + key + "\""))
            {
                startInd = tc.IndexOf("\"" + key + "\":", startInd) + 1;
                if (new bottleJson().checkJson(" " + tc.Substring(startInd, tc.Length - startInd) + " ", 0) && new bottleJson().checkJson(" " + tc.Substring(0, startInd) + " ", 1))
                {
                    startInd = startInd + key.Length;
                    String fr = pcontents.Substring(startInd, pcontents.Replace("]", ",").Replace("}", ",").IndexOf(",", startInd) - startInd).Replace("\"", "");
                    return Regex.Replace(fr.Replace("▁", "{").Replace("▂", "[").Replace("▃", "]").Replace("▄", "}").Replace("▅", ","),"\\s*","");
                }
            }
            return "";
        }

        public Boolean Contains(String key)
        {
            if (Regex.IsMatch(pcontents, "\\s*{\\s*}\\s*"))
            {
                return false;
            }
            else
            {
                if (this.getKeys().Contains(key))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public String toString()
        {
            return pcontents.Replace("▁","{").Replace("▂","[").Replace("▃","]").Replace("▄","}").Replace("▅",",");
        }

        public List<String> getKeys()
        {
            List<String> keylist = new List<String>();
            int startInd = 0;
            String tc = "{[" + pcontents + "]}";
            try
            {
                keylist.Add(pcontents.Substring(pcontents.IndexOf("\"", startInd + 1) + 1, pcontents.IndexOf("\":", startInd) - (pcontents.IndexOf("\"", startInd + 1) + 1)));
                while (tc.Substring(startInd).Contains(","))
                {
                    startInd = tc.IndexOf(",", startInd) + 1;
                    if (new bottleJson().checkJson(" " + tc.Substring(startInd, tc.Length - startInd) + " ", 0) && new bottleJson().checkJson(" " + tc.Substring(0, startInd) + " ", 1))
                    {
                        startInd = tc.IndexOf("\"", startInd);
                        keylist.Add(tc.Substring(startInd + 1, tc.IndexOf("\":", startInd) - startInd - 1));
                    }
                }
            }
            catch (Exception ex) { }
            return keylist;
        }

        public void format()
        {
            pcontents = new bottleJson().buildJson(pcontents);
            pcontents = Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(pcontents, "\n* *]", "]"), "\\{\n* *", "{"), "\\[\n* *", "["), "\n* *}", "}"), "\n* *\\,\n* *", ",");
            int startInd = 0;
            while (pcontents.Substring(startInd).Contains(","))
            {
                startInd = pcontents.IndexOf(",", startInd) + 1;
                string spaces = "";
                for (int i = 0; i < (" " + pcontents.Substring(0, startInd - 1)).Split("{".ToCharArray()).Length + (" " + pcontents.Substring(0, startInd - 1)).Split("[".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("}".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("]".ToCharArray()).Length; i++)
                {
                    spaces = spaces + "    ";
                }
                pcontents = pcontents.Substring(0, startInd - 1) + ",\n" + spaces + pcontents.Substring(startInd);
            }
            startInd = 0;
            while (pcontents.Substring(startInd).Contains("{"))
            {
                startInd = pcontents.IndexOf("{", startInd) + 1;
                string spaces = "    ";
                for (int i = 0; i < (" " + pcontents.Substring(0, startInd - 1)).Split("{".ToCharArray()).Length + (" " + pcontents.Substring(0, startInd - 1)).Split("[".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("}".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("]".ToCharArray()).Length; i++)
                {
                    spaces = spaces + "    ";
                }
                pcontents = pcontents.Substring(0, startInd - 1) + "{\n" + spaces + pcontents.Substring(startInd);
            }
            startInd = 0;
            while (pcontents.Substring(startInd).Contains("["))
            {
                startInd = pcontents.IndexOf("[", startInd) + 1;
                string spaces = "    ";
                for (int i = 0; i < (" " + pcontents.Substring(0, startInd - 1)).Split("{".ToCharArray()).Length + (" " + pcontents.Substring(0, startInd - 1)).Split("[".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("}".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("]".ToCharArray()).Length; i++)
                {
                    spaces = spaces + "    ";
                }
                pcontents = pcontents.Substring(0, startInd - 1) + "[\n" + spaces + pcontents.Substring(startInd);
            }
            startInd = pcontents.Length;
            while (pcontents.Substring(0, startInd).Contains("}"))
            {
                startInd = pcontents.LastIndexOf("}", startInd) + 1;
                string spaces = "";
                for (int i = 1; i < (" " + pcontents.Substring(0, startInd - 1)).Split("{".ToCharArray()).Length + (" " + pcontents.Substring(0, startInd - 1)).Split("[".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("}".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("]".ToCharArray()).Length; i++)
                {
                    spaces = spaces + "    ";
                }
                pcontents = pcontents.Substring(0, startInd - 1) + "\n" + spaces + "}" + pcontents.Substring(startInd);
                startInd = startInd - 2;
            }
            startInd = pcontents.Length;
            while (pcontents.Substring(0, startInd).Contains("]"))
            {
                startInd = pcontents.LastIndexOf("]", startInd) + 1;
                string spaces = "";
                for (int i = 1; i < (" " + pcontents.Substring(0, startInd - 1)).Split("{".ToCharArray()).Length + (" " + pcontents.Substring(0, startInd - 1)).Split("[".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("}".ToCharArray()).Length - (" " + pcontents.Substring(0, startInd - 1)).Split("]".ToCharArray()).Length; i++)
                {
                    spaces = spaces + "    ";
                }
                pcontents = pcontents.Substring(0, startInd - 1) + "\n" + spaces + "]" + pcontents.Substring(startInd);
                startInd = startInd - 2;
            }
        }
    }
}
