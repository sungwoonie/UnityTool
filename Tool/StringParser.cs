using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StringParser
{
    public static string ParseString(Dictionary<string, string> data, string key, string defaultValue = "")
        => data.TryGetValue(key, out var v) ? v : defaultValue;

    public static int ParseInt(Dictionary<string, string> data, string key, int defaultValue = 0)
        => data.TryGetValue(key, out var v) && int.TryParse(v, out var result) ? result : defaultValue;

    public static float ParseFloat(Dictionary<string, string> data, string key, float defaultValue = 0f)
        => data.TryGetValue(key, out var v) && float.TryParse(v, out var result) ? result : defaultValue;

    public static double ParseDouble(Dictionary<string, string> data, string key, double defaultValue = 0d)
        => data.TryGetValue(key, out var v) && double.TryParse(v, out var result) ? result : defaultValue;

    public static TEnum ParseEnum<TEnum>(Dictionary<string, string> data, string key, TEnum defaultValue) where TEnum : struct
        => data.TryGetValue(key, out var v) && Enum.TryParse(v, out TEnum result) ? result : defaultValue;

    public static List<double> ParseDoubleList(string str, char delimiter = ';')
        => str.Split(delimiter).Select(s => double.TryParse(s, out var v) ? v : 0d).ToList();

    public static List<float> ParseFloatList(string str, char delimiter = ';')
        => str.Split(delimiter).Select(s => float.TryParse(s, out var v) ? v : 0f).ToList();

    public static List<int> ParseIntList(string str, char delimiter = ';')
        => str.Split(delimiter).Select(s => int.TryParse(s, out var v) ? v : 0).ToList();

    private static Dictionary<int, T> ParseDictionary<T>(string str, Func<string, T> parseFunc, char delimiter = ';')
    {
        var list = str.Split(delimiter);
        var dict = new Dictionary<int, T>();
        for (int i = 0; i < list.Length; i++)
        {
            dict[i + 1] = parseFunc(list[i]);
        }
        return dict;
    }

    public static Dictionary<int, int> ParseIntDictionary(string str, char delimiter = ';')
    => ParseDictionary(str, s => int.TryParse(s, out var v) ? v : 0, delimiter);

    public static Dictionary<int, float> ParseFloatDictionary(string str, char delimiter = ';')
        => ParseDictionary(str, s => float.TryParse(s, out var v) ? v : 0f, delimiter);

    public static Dictionary<int, double> ParseDoubleDictionary(string str, char delimiter = ';')
        => ParseDictionary(str, s => double.TryParse(s, out var v) ? v : 0d, delimiter);
}