using System;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo {

    float timeCount;
    double totalCurrencyEarn;

    static Text infoTxt;

    public GameInfo()
    {
        timeCount = 0.0f;
        totalCurrencyEarn = 0;

        infoTxt = GameObject.Find("InfoTxt").GetComponent<Text>();
    }

    public  void UpdateGameInfoUI()
    {
        timeCount += Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds((int)timeCount);

        infoTxt.text = "Total Game Time: " + string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}",
            timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) + "\n" +
            "Total Currency Earn: " + Simplify.LargeNumConvert(totalCurrencyEarn);
    }

    public  void UpdateTotalCurrencyEarn(double value)
    {
        totalCurrencyEarn += value;
    }

    public string GenProgress()
    {
        string progressStr = "";

        progressStr += timeCount + "," + totalCurrencyEarn;

        return progressStr;
    }

    public void LoadProgress(String[] split)
    {
        timeCount = Convert.ToSingle(split[0]);
        totalCurrencyEarn = Convert.ToDouble(split[1]);
    }
}
