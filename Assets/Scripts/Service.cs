using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Service : MonoBehaviour {


    String name;
    int baseCost;
    int baseCpS;
    int currentLevel;
    int currentCpS;
    int currentCost;
    const float fixedVal = 1.08f;
    bool display;
    Text nameTxt;
    //Image img;
    Text infoTxt;
    Text btnTxt;


    public Service(String name, int baseCost, int baseCpS, int i)
    {
        this.name = name;
        this.baseCost = baseCost;
        this.baseCost = baseCpS;
        currentLevel = 0;
        currentCost = baseCost;
        display = false;

        nameTxt = GameObject.Find("Service" + i + "Name").GetComponent<Text>();
        infoTxt = GameObject.Find("Service" + i + "Info").GetComponent<Text>();
        btnTxt = GameObject.Find("Service" + i + "BtnTxt").GetComponent<Text>();

        nameTxt.text = "???";
        infoTxt.text = "Level:???\n CpS:???";
        btnTxt.text = "Level Up +???\n" + "??? Currency";
        
    }

    public int GetBaseCost() { return baseCost; }
    public int GetCurrentLevel() { return currentLevel; }
    public float GetcurrentCost() { return currentCost; }

    public void SetDiaply(bool display) { this.display = display; }

    public float CalCurrentCost() { return baseCost * Mathf.Pow(fixedVal, currentLevel); }
}
