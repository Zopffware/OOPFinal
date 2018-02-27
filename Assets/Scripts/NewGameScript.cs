using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameScript : MonoBehaviour {
    public Button NewGame;
	// Use this for initialization
	void Start () {
        Button btn = NewGame.GetComponent<Button>();
        btn.onClick.AddListener(GameStart);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GameStart () {
        SceneManager.LoadScene("IntroScene", LoadSceneMode.Single);
        
    }
}
