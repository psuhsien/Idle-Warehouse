using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;


	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        //Set resolution to 1600 x 900 and window
        Screen.SetResolution(1600, 900, false);
	}
	
	void Update () {
		
	}
}
