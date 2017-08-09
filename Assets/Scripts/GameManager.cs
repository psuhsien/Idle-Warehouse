using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    
    public static int curCurrency;
    public static int curCpS;
    int serviceInd;
    float timeCount = 0.0f;
    public static Service[] serviceList;

    private Text timeCountTxt;
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
        InitGame();
        InitService();                  
    }

    void InitGame()
    {
        curCurrency = 0;
        curCpS = 0;

        InvokeRepeating("UpdateCurCurrency", 1.0f, 1.0f);
        InvokeRepeating("ServiceCheckAndUnlock", 1.0f, 1.0f);
        InvokeRepeating("ServiceBtnCheck", 1.0f, 1.0f);

        timeCountTxt = GameObject.Find("TimeCount").GetComponent<Text>();
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

        timeCount += Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds((int)timeCount);

        timeCountTxt.text = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", 
            timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        curCurrencyTxt.text = "Currency\n" + curCurrency;
        curCpSTxt.text = "Currency Per Second\n" + curCpS;

        ServiceBtnCheck();
    }

    void UpdateCurCurrency() { curCurrency += curCpS; }
    
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

            if (serviceList[i].GetcurrentCost() > curCurrency)
                curBtn.interactable = false;
            else
                curBtn.interactable = true;
        }
    }
}
