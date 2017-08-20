using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Service {


    String title;
    double baseCost;
    double baseCpS;
    int curLevel;
    double totalCpS;
    double unitCpS;
    double curCost;
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

    GameObject toolTip;

    public Service(String title, int baseCost, int baseCpS, int serviceInd)
    {
        this.title = title;
        this.baseCost = baseCost;
        this.baseCpS = baseCpS;
        curLevel = 0;
        totalCpS = 0;
        unitCpS = baseCpS;
        curCost = baseCost;
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
            UpgradeBtnAddEventTrigger(upgradeBtn[i], buttonInd);
            upgradeTxt[i] = GameObject.Find("Service" + serviceInd + "Upgrade" + (i + 1) + "Txt").GetComponent<Text>();
            upgradeCost[i] = baseCost * Math.Pow(10, (i + 1));
        }

        LockUI();

    }

    public double GetBaseCost() { return baseCost; }
    public double GetCurLevel() { return curLevel; }
    public double GetTotalCpS() { return totalCpS; }
    public double GetcurCost() { return curCost; }
    public Button GetBtn() { return btn; }
    public Button[] GetUpgradeBtn() { return upgradeBtn; }
    public Text[] GetUpgradeTxt() { return upgradeTxt; }
    public double[] GetUpgradeCost() { return upgradeCost; }

    public double CalCurrentCost() { return Math.Floor(baseCost * Math.Pow(fixedVal, curLevel)); }

    public void Unlock()
    {
        titleTxt.text = title;
        UpdateUI();
    }

    public void LockUI() {
        titleTxt.text = "???";
        infoTxt.text = "Level:???\n" + "CpS:???";
        btnTxt.text = "Level Up +???\n" + "??? Currency";
    }

    public void LevelUp()
    {
        GameManager.CurCurrencyOP('-', curCost);
        curLevel++;
        curCost = CalCurrentCost();

        UpdateCurrentCpS();
        UpdateUI();
    }

    public void Upgrade(int buttonInd)
    {
        upgradeCost[buttonInd] = -1;
        upgradeTxt[buttonInd].text = "/";
        multiplier *= 2;

        UpdateCurrentCpS();
        UpdateUnitCpS();
        UpdateUI();

        GameManager.Destroy(toolTip);
    }
    
    public void UpgradeBtnAddEventTrigger(Button upgradeBtn, int buttonInd)
    {
        EventTrigger btnTrigger = upgradeBtn.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { ToolTipPointerEnter(buttonInd); });
        btnTrigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { ToolTipPointerExit(buttonInd); });
        btnTrigger.triggers.Add(entry);
    }

    public void ToolTipPointerEnter(int buttonInd)
    {
        if (upgradeCost[buttonInd] == -1 || upgradeCost[buttonInd] == -2)
            return;

        toolTip = (GameObject) GameManager.Instantiate(Resources.Load("ToolTip"), 
            GameObject.Find("Canvas").GetComponent<Canvas>().transform);
        toolTip.transform.position = new Vector3(upgradeBtn[buttonInd].transform.position.x, 
            upgradeBtn[buttonInd].transform.position.y + 20, 0);

        Text ToolTipTxt = toolTip.transform.Find("ToolTipTxt").gameObject.GetComponent<Text>();

        ToolTipTxt.text = title + " produce twice\n" + "Cost: " + Simplify.LargeNumConvert(upgradeCost[buttonInd]);
        toolTip.GetComponent<RectTransform>().sizeDelta = new Vector2(ToolTipTxt.preferredWidth, ToolTipTxt.preferredHeight);
    }

    public void ToolTipPointerExit(int buttonInd)
    {
        if (upgradeCost[buttonInd] == -1 || upgradeCost[buttonInd] == -2)
            return;

        GameManager.Destroy(toolTip);
    }

    public void UpdateCurrentCpS() { totalCpS = curLevel * baseCpS * multiplier; }
    public void UpdateUnitCpS() { unitCpS = baseCpS * multiplier; }

    public void UpdateUI()
    {
        infoTxt.text = "Level: " + curLevel + "\n" + "Total CpS:" + Simplify.LargeNumConvert(totalCpS);
        btnTxt.text = "Level Up + " + Simplify.LargeNumConvert(unitCpS) + " CpS\n" + Simplify.LargeNumConvert(curCost) + " Currency";
    }

    public string GenProgress()
    {
        string progressStr = "";

        progressStr += curLevel + "," + curCost;

        for (int i = 0; i < upgradeCost.Length; i++)
            progressStr += "," + upgradeCost[i];

        progressStr += "," + multiplier;

        return progressStr;
    }

    public void LoadProgress(string[] split)
    {
        curLevel = Convert.ToInt32(split[0]);
        curCost = Convert.ToDouble(split[1]);

        for (int i = 0; i < upgradeCost.Length; i++)
            upgradeCost[i] = Convert.ToDouble(split[i + 2]);

        multiplier = Convert.ToInt32(split[12]);

        UpdateCurrentCpS();
        UpdateUnitCpS();

        LockUI();
    }
}