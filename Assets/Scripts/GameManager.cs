using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class GameManager : MonoBehaviour {

    static double curCurrency;
    static double curCpS;
    int serviceInd;

    static GameInfo gameInfo;
    public static Service[] serviceList;

    Text curCurrencyTxt;
    Text curCpSTxt;

    public static GameManager instance = null;
    
    private void Start()
    {
        InitList();
    }

    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        //Set resolution to 1600 x 900 and window
        Screen.SetResolution(1600, 900, false);
	}
	
    void InitList()
    {
        InitService();
        InitGameInfo();
        Simplify.InitLargeNumName();
        InitGame();
    }

    void InitGame()
    {
        curCurrency = 0;
        curCpS = 0;

        InvokeRepeating("UpdateCurCurrency", 1.0f, 1.0f);
        InvokeRepeating("UpdateTotalCurrencyEarn", 1.0f, 1.0f);
        InvokeRepeating("ServiceCheckAndUnlock", 1.0f, 1.0f);

        curCurrencyTxt = GameObject.Find("CurCurrencyTxt").GetComponent<Text>();
        curCpSTxt = GameObject.Find("CurCpSTxt").GetComponent<Text>();
    }

    void InitService()
    {
        int[] baseCostArr = new int[] { 8, 64, 512, 4096, 32768, 262144, 2097152 };
        int[] baseCpSArr = new int[] { 1, 8, 64, 512, 4096, 32768, 262144 };
        String[] serviceNameArr = new String[] { "Labor",  "Cart", "Handheld Pellet Truck",
            "Order Picker", "Fork", "Clamp", "Turret Truck"};

        int len = baseCostArr.Length;

        serviceList = new Service[len];

        for (int i = 0; i < len; i++)
            serviceList[i] = new Service(serviceNameArr[i], baseCostArr[i], baseCpSArr[i], i);
    }

    void InitGameInfo()
    {
        gameInfo = new GameInfo();
    }

	void Update () {
        gameInfo.UpdateGameInfoUI();

        UpdateCurCpS();

        UpdateProgressInfoUI();

        ServiceBtnCheck();
        ServiceUpgradeBtnCheck();
    }

    void UpdateProgressInfoUI()
    {
        curCurrencyTxt.text = "Currency\n" + Simplify.LargeNumConvert(curCurrency);
        curCpSTxt.text = "Currency Per Second\n" + Simplify.LargeNumConvert(curCpS);
    }

    void UpdateCurCurrency() { curCurrency += curCpS; }
    void UpdateTotalCurrencyEarn() { gameInfo.UpdateTotalCurrencyEarn(curCpS); }

    void UpdateCurCpS()
    {
        double newCurCpS = 0;

        for (int i = 0; i < serviceList.Length; i++)
            newCurCpS += serviceList[i].GetTotalCpS();

        curCpS = newCurCpS;
    }

    void ServiceCheckAndUnlock()
    {
        if (curCurrency >= serviceList[serviceInd].GetBaseCost())
        {
            serviceList[serviceInd].Unlock();
            serviceInd++;
        }

        if (serviceInd >= 7)
            CancelInvoke("ServiceCheckAndUnlock");
    }
    
    void ServiceBtnCheck()
    {
        for (int i = 0; i < serviceList.Length; i++)
        {
            Button curBtn = serviceList[i].GetBtn();

            if (serviceList[i].GetcurCost() > curCurrency)
                curBtn.interactable = false;
            else
                curBtn.interactable = true;
        }
    }

    void ServiceUpgradeBtnCheck()
    {
        for (int i = 0; i < serviceList.Length; i++)
        {
            Button[] curBtn = serviceList[i].GetUpgradeBtn();
            double[] curCost = serviceList[i].GetUpgradeCost();

            for (int j = 0; j < curCost.Length; j++)
            {
                if (curCost[j] > 0)
                {
                    if (curCost[j] > curCurrency)
                        curBtn[j].interactable = false;
                    else
                        curBtn[j].interactable = true;
                }
                else if (curCost[j] == -1)
                {
                    curCost[j] = -2;
                    curBtn[j].interactable = false;
                }
            }

        }
    }

    public static void CurCurrencyOP(char sym, double value)
    {
        if (sym == '-')
            curCurrency -= value;
        else if (sym == '+')
        {
            curCurrency += value;
            gameInfo.UpdateTotalCurrencyEarn(value);
        }
    }

    public static string GenProgress()
    {
        string progressStr = "";

        progressStr += curCurrency + "," + curCpS + ";";

        for (int i = 0; i < serviceList.Length - 1; i++)
            progressStr += serviceList[i].GenProgress() + ":";

        progressStr += serviceList[serviceList.Length - 1].GenProgress() + ";";

        progressStr += gameInfo.GenProgress();

        return progressStr;
    }

    public static void LoadProgress(string progressStr)
    {
        string[] strSplit = progressStr.Split(';');

        string[] split3 = strSplit[2].Split(',');
        gameInfo.LoadProgress(split3);

        string[] split2 = strSplit[1].Split(':');

        for (int i = 0; i < serviceList.Length; i++)
            serviceList[i].LoadProgress(split2[i].Split(','));


        string[] split1 = strSplit[0].Split(',');

        curCurrency = Convert.ToDouble(split1[0]);
        curCpS = Convert.ToDouble(split1[1]);
    }

    public static void WipeProgress()
    {
        LoadProgress("0,0;" +
            "0,8,80,800,8000,80000,800000,8000000,80000000,800000000,8000000000,80000000000,1:" +
            "0,64,640,6400,64000,640000,6400000,64000000,640000000,6400000000,64000000000,640000000000,1:" +
            "0,512,5120,51200,512000,5120000,51200000,512000000,5120000000,51200000000,512000000000,5120000000000,1:" +
            "0,4096,40960,409600,4096000,40960000,409600000,4096000000,40960000000,409600000000,4096000000000," +
            "40960000000000,1:0,32768,327680,3276800,32768000,327680000,3276800000,32768000000,327680000000," +
            "3276800000000,32768000000000,327680000000000,1:0,262144,2621440,26214400,262144000,2621440000," +
            "26214400000,262144000000,2621440000000,26214400000000,262144000000000,2.62144E+15,1:0,2097152,20971520," +
            "209715200,2097152000,20971520000,209715200000,2097152000000,20971520000000,209715200000000," +
            "2.097152E+15,2.097152E+16,1;1.173811,0");
    }
}
