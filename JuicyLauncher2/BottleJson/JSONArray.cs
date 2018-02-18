using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BottleJson
{
    public sealed class JSONArray {

    String pcontents = "";
    private String[] ArrList;

    public JSONArray(String contents) {
        pcontents = new bottleJson().buildJson(contents);
        ArrList = SplitArray(contents);
    }

    public JSONArray()
    {
        pcontents = "[]";
        pcontents = new bottleJson().buildJson(pcontents);
        ArrList = new string[99999];
    }

    private void ReGen() {
        pcontents = "";
        foreach (String str in ArrList) {
            if (str == null) {
                break;
            }
            pcontents = pcontents + str + ",";
        }
        pcontents = "[" + pcontents.Substring(0, pcontents.Length - 1) + "]";
    }

    public JSONObject getJSONObject(int index) {
        JSONObject jo = new JSONObject(ArrList[index].Replace("▁", "{").Replace("▂", "[").Replace("▃", "]").Replace("▄", "}").Replace("▅", ","));
        return jo;
    }

    public void putJSONObject(int index, JSONObject value) {
        ArrList[index] = value.toString();
        ReGen();
    }

    public String getString(int index) {
        return ArrList[index].Replace("\"", "").Replace("▁", "{").Replace("▂", "[").Replace("▃", "]").Replace("▄", "}").Replace("▅", ",");
    }

    public void putString(int index, String value) {
        ArrList[index] = "\"" + value + "\"";
        ReGen();
    }

    public JSONArray getJSONArray(int index) {
        JSONArray ja = new JSONArray(ArrList[index].Replace("▁", "{").Replace("▂", "[").Replace("▃", "]").Replace("▄", "}").Replace("▅", ","));
        return ja;
    }

    public void putJSONArray(int index, JSONArray value) {
        ArrList[index] = value.toString();
        ReGen();
    }

    private String[] SplitArray(String NativeJson) {
        String[] ForRet = new String[99999];
        int startInd = 0;
        int lateInd = 0;
        int counter = 0;
        while (NativeJson.Substring(startInd + 1).Contains(",")) {
            startInd = NativeJson.IndexOf(",", startInd + 1);
            if (new bottleJson().checkJson(" " + NativeJson.Substring(startInd, NativeJson.Length - 1-startInd) + " ") && new bottleJson().checkJson(" " + NativeJson.Substring(1, startInd-1) + " ")) {
                ForRet[counter] = NativeJson.Substring(lateInd + 1, startInd-lateInd-1);
                counter++;
                lateInd = startInd;
            }
        }
        ForRet[counter] = NativeJson.Substring(lateInd + 1, NativeJson.Length - 1 - lateInd - 1);
        return ForRet;
    }

    public String getObject(int index) {
        return ArrList[index].Replace("▁", "{").Replace("▂", "[").Replace("▃", "]").Replace("▄", "}").Replace("▅", ",");
    }

    public int length() {
        for (int i = 0; i < ArrList.Length; i++) {
            if (ArrList[i] == null) {
                return i;
            }
        }
        return 0;
    }

    public void putObject(int index, String value) {
        ArrList[index] = value;
        ReGen();
    }

    public Boolean Contains(String key) {
        for (int i = 0; i < this.length(); i++) {
            String aitm = ArrList[i];
            int startInd = 0;
            try {
                while (aitm.Substring(startInd).Contains(key)) {
                    startInd = aitm.IndexOf(key, startInd + 1);
                    if (new bottleJson().checkJson(" " + aitm.Substring(startInd, aitm.Length - 1-startInd) + " ") && new bottleJson().checkJson(" " + aitm.Substring(1, startInd-1) + " ") && startInd <= key.Length) {
                        return true;
                    }
                }
            } catch (Exception ex) {

            }
        }
        return false;
    }

    public void deleteItem(int index) {
        ArrList[index] = null;
        for(int i=index;i<ArrList.Length;i++){
            if(ArrList[i+1]==null){
                ArrList[i]=null;
                break;
            }
            ArrList[i]=ArrList[i+1];
        }
        ReGen();
    }

    public String toString() {
        return pcontents.Replace("▁", "{").Replace("▂", "[").Replace("▃", "]").Replace("▄", "}").Replace("▅", ",");
    }

    public JSONObject[] toJSONObjects() {
        JSONObject[] jo = new JSONObject[this.length()];
        for (int i = 0; i < this.length(); i++) {
            jo[i] = new JSONObject(ArrList[i]);
        }
        return jo;
    }
}

}
