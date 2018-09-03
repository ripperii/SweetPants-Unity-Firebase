using System;
using UnityEngine;

public class TimeStamp : MonoBehaviour
{
    public static DateTime Convert(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }
    public static double Convert(DateTime dt)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return (dt - dtDateTime).TotalMilliseconds;
    }
    public static double Convert(string dt)
    {
        return double.Parse(dt);
    }
}
