using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameScript : MonoBehaviour {
    public Button NewGame;
    public GameObject infoPrefab, menuPrefab;
    // Use this for initialization
    void Start()
    {
        Button btn = NewGame.GetComponent<Button>();
        btn.onClick.AddListener(GameStart);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void GameStart () {
        Vector3 pos = new Vector3(0, 0, 0);
        Destroy(menuPrefab);
        GameControl.control.money = 1000;
        GameControl.control.JSHTMLLovePoints = 0;
        GameControl.control.CPPLovePoints = 0;
        GameControl.control.CSLovePoints = 0;
        GameControl.control.PYLovePoints = 0;
        GameControl.control.JavaLovePoints = 0;
        GameControl.control.day = 0;
        GameControl.control.hour = 0;
        GameControl.control.commandIndex = 0;
        Instantiate(infoPrefab, pos, Quaternion.identity);
        ScriptParser.readScript("\\Home\\Narrator\\Intro.txt");
        ScriptParser.advanceScript();
    }
}
