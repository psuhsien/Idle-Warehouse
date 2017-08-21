using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventTriggerList : MonoBehaviour {

    InputField saveLoadField;
    GameObject settingBackground;
    GameObject wipeTxtBackground;

    // run when the game start
    public void Start()
    {
        saveLoadField = GameObject.Find("SaveLoadField").GetComponent<InputField>();
        settingBackground = GameObject.Find("SettingBackground");
        wipeTxtBackground = GameObject.Find("WipeTxtBackground");

        settingBackground.SetActive(false);
    }

    public void BoxClick() { GameManager.CurCurrencyOP('+', 1); }

    public void ServiceClick(int ind) { GameManager.serviceList[ind].LevelUp(); }

    public void ServiceUpgradeClick(int ind, int num) { GameManager.serviceList[ind].Upgrade(num); }

    // Save, Load, or Wipe button click
    // Save = 1, Load = 2, and Wipe = 3
    public void SaveLoadWipeClick(int OPCode)
    {
        settingBackground.SetActive(true);

        if (OPCode == 1)
        {
            wipeTxtBackground.SetActive(false);

            saveLoadField.gameObject.SetActive(true);
            saveLoadField.readOnly = true;
            saveLoadField.text = GameManager.GenProgress();
            saveLoadField.Select();
        }
        else if (OPCode == 2)
        {
            wipeTxtBackground.SetActive(false);

            saveLoadField.gameObject.SetActive(true);
            saveLoadField.readOnly = false;
            saveLoadField.text = "";
        }
        else if (OPCode == 3)
        {
            wipeTxtBackground.SetActive(true);

            saveLoadField.gameObject.SetActive(false);
        }
    }

    public void ConfirmClick()
    {
        // saveLoadField active = Save or Load button
        // otherwise Wipe button
        if (saveLoadField.IsActive())
        {
            // if saveLoadField is editable = Load button
            if (!saveLoadField.readOnly)
            {
                GameManager.LoadProgress(saveLoadField.text);
            }
        }
        else
        {
            GameManager.WipeProgress();
        }

        settingBackground.SetActive(false);
    }

    // Turn off settingBackground object
    public void CancelClick()
    {
        settingBackground.SetActive(false);
    }
}
