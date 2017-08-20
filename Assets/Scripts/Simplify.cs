using System;
using System.IO;

public class Simplify {

    static string[] listLargeNumName;

    public static void InitLargeNumName()
    {
        listLargeNumName = new String[102];

        string path = "Assets/Resources/ListOfLargeNumberNames.txt";

        StreamReader reader = new StreamReader(path);

        for (int i = 0; i < 102; i++)
        {
            string cur = reader.ReadLine();

            listLargeNumName[i] = cur;
        }
    }

    public static String LargeNumConvert(double value)
    {
        if (value < 1000000)
            return value.ToString();

        string result = "";

        double log10Result = Math.Log10(value);

        int ind = (int)log10Result / 3 - 2;

        result = (value / Math.Pow(10, (ind + 2) * 3)).ToString("N3") + " " + listLargeNumName[ind];

        return result;
    }
}