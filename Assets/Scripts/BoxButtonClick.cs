using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxButtonClick : MonoBehaviour {

	public void BoxClick()
    {
        Debug.Log("Click");
        GameManager.curCurrency++;
    }
}
