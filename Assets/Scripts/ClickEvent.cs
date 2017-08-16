using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent : MonoBehaviour {

    public void BoxClick() { GameManager.curCurrency++; }

    public void ServiceClick(int ind) { GameManager.serviceList[ind].LevelUp(); }

    public void ServiceUpgradeClick(int ind, int num) { GameManager.serviceList[ind].Upgrade(num); }

    public void SaveCLick()
    {

    }

    public void LoadClick()
    {

    }

    public void StatsClick()
    {

    }

    public void WipeClick()
    {

    }
}
