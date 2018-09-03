using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Levels : MonoBehaviour
{
    //
    public static Dictionary<int, List<int>> list;
    //public static Dictionary<int, int> SummedLevels;

    public static void GetLevels()
    {
        Dictionary<string, string> temp = new Dictionary<string, string>();
        string json = JsonHelper.FixJson(RemoteConfig.LevelsJson);

        Debug.Log(json);

        temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        
        int sum = 0;
        list = new Dictionary<int, List<int>>();

        foreach(var l in temp.OrderBy(i => int.Parse(i.Key)))
        {
            list.Add(int.Parse(l.Key), new List<int>(2) { int.Parse(l.Value), sum });

            Debug.Log("Level: " + l.Key + " XP: " + int.Parse(l.Value) + " Summed XP: " + sum);

            sum += int.Parse(l.Value);
        }
    }
    public static string FindPlayerLevelString(int xp)
    {
        string lv = "1";
        if (xp > list.LastOrDefault().Value[1])
        {
            xp = list.LastOrDefault().Value[1];
        }

        lv = list.Where(x => (x.Key + 1) > list.Count || list[x.Key + 1][1] >= xp).FirstOrDefault().Key.ToString();

        /*
        for (int i = 0; i<list.Count; i++)
        {
            if( (i+1) > list.Count || list[i+1][1] >= xp)
            {
                lv = i.ToString();
                break;
            }
        }
        */
        return lv;
    }
    public static KeyValuePair<int, List<int>> FindPlayerLevel(int xp)
    {
        KeyValuePair<int, List<int>> ret = list.Where(x => x.Value[1] == 0).FirstOrDefault();

        if (xp > list[list.Count - 1][1])
        {
            xp = list[list.Count - 1][1];
        }
        
        ret = list.Where(x => (x.Key + 1) > list.Count || list[x.Key + 1][1] >= xp).FirstOrDefault();

        /*
        for (int i = 0; i < list.Count; i++)
        {
            if ((i + 1) > list.Count || list[i + 1][1] >= xp)
            {
                ret = new KeyValuePair<int, List<int>>(i,list[i]);
                break;
            }
        }
        */
        return ret;
    }
    public static KeyValuePair<int, List<int>> GetNextLevel(int currentLevel)
    {
        return list.Where(x => x.Key == currentLevel+1).FirstOrDefault();
    }
}



