using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Service : MonoBehaviour {
    

    String title;
    int baseCost;
    int baseCpS;
    int currentLevel;
    int currentCpS;
    int currentCost;
    const double fixedVal = 1.08f;
    Text titleTxt;
    //Image img;
    Text infoTxt;
    Text btnTxt;
    Button btn;

    public Service(String title, int baseCost, int baseCpS, int i)
    {
        this.title = title;
        this.baseCost = baseCost;
        this.baseCpS = baseCpS;
        currentLevel = 0;
        currentCost = baseCost;

        titleTxt = GameObject.Find("Service" + i + "Name").GetComponent<Text>();
        infoTxt = GameObject.Find("Service" + i + "Info").GetComponent<Text>();
        btnTxt = GameObject.Find("Service" + i + "BtnTxt").GetComponent<Text>();
        btn = GameObject.Find("Service" + i + "Btn").GetComponent<Button>();

        titleTxt.text = "???";
        infoTxt.text = "Level:???\n" + "CpS:???";
        btnTxt.text = "Level Up +???\n" + "??? Currency";
        
    }

    public int GetBaseCost() { return baseCost; }
    public int GetCurrentLevel() { return currentLevel; }
    public float GetcurrentCost() { return currentCost; }
    public Button GetBtn() { return btn; }

    public int CalCurrentCost() { return (int) (baseCost * Math.Pow(fixedVal, currentLevel)); }

    public void Unlock()
    {
        titleTxt.text = title;
        infoTxt.text = "Level: 0\n" + "CpS: " + baseCpS;
        btnTxt.text = "Level Up + 1\n" + baseCost + "Currency";
    }

    public void LevelUp()
    {
        GameManager.curCurrency -= currentCost;
        GameManager.curCpS += baseCpS;
        currentLevel++;
        currentCost = CalCurrentCost();
        currentCpS = currentLevel * baseCpS;

        infoTxt.text = "Level: " + currentLevel + "\n" + "CpS:" + currentCpS;
        btnTxt.text = "Level Up + 1\n" + currentCost + "Currency";
    }
}
