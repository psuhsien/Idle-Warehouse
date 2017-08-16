using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Service {
    

    String title;
    double baseCost;
    double baseCpS;
    int currentLevel;
    double currentCpS;
    double currentCost;
    const double fixedVal = 1.08;
    Text titleTxt;
    //Image img;
    Text infoTxt;
    Text btnTxt;
    Button btn;
    Button[] upgradeBtn;
    Text[] upgradeTxt;
    double[] upgradeCost;
    int multiplier;

    public Service(String title, int baseCost, int baseCpS, int serviceInd)
    {
        this.title = title;
        this.baseCost = baseCost;
        this.baseCpS = baseCpS;
        currentLevel = 0;
        currentCost = baseCost;
        upgradeBtn = new Button[10];
        upgradeTxt = new Text[10];
        upgradeCost = new double[10];
        multiplier = 1;

        titleTxt = GameObject.Find("Service" + serviceInd + "Name").GetComponent<Text>();
        infoTxt = GameObject.Find("Service" + serviceInd + "Info").GetComponent<Text>();
        btnTxt = GameObject.Find("Service" + serviceInd + "BtnTxt").GetComponent<Text>();
        btn = GameObject.Find("Service" + serviceInd + "Btn").GetComponent<Button>();
        
        for (int i = 0; i < 10; i++)
        {
            int buttonInd = i;

            upgradeBtn[i] = GameObject.Find("Service" + serviceInd + "Upgrade" + (i + 1) + "Btn").GetComponent<Button>();
            upgradeBtn[i].onClick.AddListener(() => Upgrade(buttonInd));
            upgradeTxt[i] = GameObject.Find("Service" + serviceInd + "Upgrade" + (i + 1) + "Txt").GetComponent<Text>();
            upgradeCost[i] = baseCost * Math.Pow(10, (i + 1));
        }
        
        titleTxt.text = "???";
        infoTxt.text = "Level:???\n" + "CpS:???";
        btnTxt.text = "Level Up +???\n" + "??? Currency";
        
    }

    public double GetBaseCost() { return baseCost; }
    public double GetCurrentLevel() { return currentLevel; }
    public double GetcurrentCost() { return currentCost; }
    public Button GetBtn() { return btn; }
    public Button[] GetUpgradeBtn() { return upgradeBtn; }
    public Text[] GetUpgradeTxt() { return upgradeTxt; }
    public double[] GetUpgradeCost() { return upgradeCost; }

    public double CalCurrentCost() { return Math.Floor(baseCost * Math.Pow(fixedVal, currentLevel)); }

    public void Unlock()
    {
        titleTxt.text = title;
        infoTxt.text = "Level: 0\n" + "CpS: " + baseCpS;
        btnTxt.text = "Level Up + 1\n" + baseCost + " Currency";
    }

    public void LevelUp()
    {
        GameManager.curCurrency -= currentCost;
        GameManager.curCpS += baseCpS;
        currentLevel++;
        currentCost = CalCurrentCost();
        currentCpS = currentLevel * baseCpS * multiplier;

        infoTxt.text = "Level: " + currentLevel + "\n" + "CpS:" + currentCpS;
        btnTxt.text = "Level Up + 1\n" + currentCost + " Currency";
    }

    public void Upgrade(int buttonInd)
    {
        upgradeCost[buttonInd] = -1;
        upgradeTxt[buttonInd].text = "/";
        multiplier *= 2;
    }
}
