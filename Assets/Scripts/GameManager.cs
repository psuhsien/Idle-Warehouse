using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    
    static double curCurrency;
    double curCpS;
    private double totalCurrencyEarn;
    int serviceInd;
    float timeCount = 0.0f;
    public static Service[] serviceList;

    private Text infoTxt;
    private Text curCurrencyTxt;
    private Text curCpSTxt;

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
        InitGame();
    }

    void InitGame()
    {
        curCurrency = 0;
        curCpS = 0;
        totalCurrencyEarn = 0;

        InvokeRepeating("UpdateCurCurrency", 1.0f, 1.0f);
        InvokeRepeating("UpdateTotalCurrencyEarn", 1.0f, 1.0f);
        InvokeRepeating("ServiceCheckAndUnlock", 1.0f, 1.0f);
        //InvokeRepeating("ServiceBtnCheck", 1.0f, 1.0f);
        //InvokeRepeating("ServiceUpgradeBtnCheck", 1.0f, 1.0f);

        infoTxt = GameObject.Find("InfoTxt").GetComponent<Text>();
        curCurrencyTxt = GameObject.Find("CurCurrency").GetComponent<Text>();
        curCpSTxt = GameObject.Find("CurCpS").GetComponent<Text>();
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


	void Update () {

        //transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //GameObject.Find("Button").GetComponent<Button>().transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        

        timeCount += Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds((int)timeCount);

        infoTxt.text = "Total Game Time: " + string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}",
            timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) + "\n" +
            "Total Currency Earn: " + totalCurrencyEarn;

        UpdateCurCpS();

        curCurrencyTxt.text = "Currency\n" + curCurrency;
        curCpSTxt.text = "Currency Per Second\n" + curCpS;

        ServiceBtnCheck();
        ServiceUpgradeBtnCheck();
    }

    void UpdateCurCurrency() { curCurrency += curCpS; }
    void UpdateTotalCurrencyEarn() { totalCurrencyEarn += curCpS; }

    void UpdateCurCpS()
    {
        double newCurCpS = 0;

        for (int i = 0; i < serviceList.Length; i++)
            newCurCpS += serviceList[i].GetTotalCpS();

        curCpS = newCurCpS;
    }

    void ServiceCheckAndUnlock()
    {
        if ( curCurrency >= serviceList[serviceInd].GetBaseCost())
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
            curCurrency += value;
    }
}
