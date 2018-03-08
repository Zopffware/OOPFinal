using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class currentMoney : MonoBehaviour {
    public Text countText;

	// Use this for initialization
	void Start () {
		 int cm = GameControl.control.money;
        
        countText.text="ByteCoin: "+cm.ToString ();

	}
	
	// Update is called once per frame
	void Update () {
        int cm = GameControl.control.money;
        string myString = cm.ToString();
        GameObject.Find("currentMoney").GetComponentInChildren<Text>().text=myString;
	}
}
