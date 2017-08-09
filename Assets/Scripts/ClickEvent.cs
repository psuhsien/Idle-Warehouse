using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent : MonoBehaviour {

    public void BoxClick() { GameManager.curCurrency++; }

    public void ServiceClick(int ind) { GameManager.serviceList[ind].LevelUp(); }
}
