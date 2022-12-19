using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CSV
{
    public static Dictionary<string, MyJson.INode> allCSV = new Dictionary<string, MyJson.INode>();
    private static int tpos = 0;
    private static int tendpos = 0;
    private static string tstr = "";

    public static MyJson.INode LoadCSV(string name, string json)
    {
        if (!allCSV.ContainsKey(name))
        {
            MyJson.INode csvData = Parse(json);
            allCSV.Add(name, csvData);
            return csvData;
        }
        else
        {
            return allCSV[name];
        }
    }

    public static MyJson.INode LoadCSVFromResources(string name)
    {
        if (!allCSV.ContainsKey(name))
        {
            string json = Resources.Load<TextAsset>("CSV/" + name).text;
            MyJson.INode csvData = Parse(json);
            allCSV.Add(name, csvData);
            return csvData;
        }
        else
        {
            return allCSV[name];
        }
    }
    protected static MyJson.INode Parse(string str)
    {
        tpos = 0;
        tstr = str;
        tendpos = str.Length;
        List<List<MyJson.INode>> array = new List<List<MyJson.INode>>();
        while (tpos < tendpos)
        {
            List<MyJson.INode> nextrow = NextRow();
            array.Add(nextrow);
        }
        return BindColumns(array);
    }
    protected static MyJson.INode BindColumns(List<List<MyJson.INode>> rows)
    {
        List<MyJson.INode> keys = rows[0];
        rows.RemoveAt(0);
        MyJson.JObject json = new MyJson.JObject();
        for (int i = 0; i < rows.Count; i++)
        {
            MyJson.JObject row_obj = new MyJson.JObject();
            for (int j = 1; j < rows[i].Count; j++)
            {
                row_obj.SetDictValue(keys[j].ToString(), rows[i][j]);
            }
            json.SetDictValue(rows[i][0].ToString(), row_obj);
        }
        return json;
    }
    protected static List<MyJson.INode> NextRow()
    {
        List<MyJson.INode> array = new List<MyJson.INode>();
        MyJson.INode str = NextStr();
        array.Add(str);
        str = NextLabel();
        while (str != null)
        {
            array.Add(str);
            str = NextLabel();
        }
        if (!NextIs("\n") && !NextIs("\r\n"))
        {
            return null;
        }
        return array;
    }
    protected static MyJson.INode NextLabel()
    {
        if (NextIs(","))
        {
            var tmp = NextStr();
            return tmp;
        }
        else
        {
            return null;
        }
    }
    protected static MyJson.INode NextStr()
    {
        int begin = tpos;
        if (NextIs("["))
        {
            int posa = tstr.IndexOf("]", begin);
            posa = posa > 0 ? ++posa : tendpos;
            tpos = posa;
            string array_str = tstr.Substring(begin, tpos - begin);
            return MyJson.Parse(array_str);
        }
        else if (NextIs("\"["))
        {
            int posa = tstr.IndexOf("]\"", begin);
            posa = posa > 0 ? posa += 1 : tendpos;
            tpos = posa;
            string array_str = tstr.Substring(begin + 1, tpos - (begin + 1));
            string new_str = array_str;
            while (new_str.IndexOf("\"\"") > 0 )
            {
                new_str = new_str.Replace("\"\"", "\"");
            }
            tpos++;
            return MyJson.Parse(new_str);
        }
        else
        {
            int posa = tstr.IndexOf(",", begin);
            if (posa < 0)
            {
                posa = tendpos;
            }
            int posb = tstr.IndexOf("\n", begin);
            if (posb < 0)
            {
                posb = tendpos;
            }
            int posc = tstr.IndexOf("\r\n", begin);
            if (posc < 0)
            {
                posc = tendpos;
            }
            tpos = Mathf.Min(Mathf.Min(posa, posb), posc);
            string value_str = tstr.Substring(begin, tpos - begin);
            MyJson.JObject tempNode = new MyJson.JObject();
            tempNode.SetDictValue("temp", value_str);
            return tempNode["temp"];
        }
    }
    protected static bool NextIs(string s)
    {
        if (tpos < tendpos)
        {
            int len = s.Length;
            if (tstr.Substring(tpos,len) == s)
            {
                tpos += len;
                return true;
            }
        }
        return false;
    }
}
