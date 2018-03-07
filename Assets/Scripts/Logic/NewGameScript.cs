using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewGameScript : MonoBehaviour {
    public Button NewGame;
    public GameObject infoPrefab, menuPrefab;
	// Use this for initialization
	void Start () {
        Button btn = NewGame.GetComponent<Button>();
        
        btn.onClick.AddListener(GameStart);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GameStart () {
        Destroy(menuPrefab);
        Vector3 pos = new Vector3(0, 0, 0);
        Instantiate(infoPrefab, pos, Quaternion.identity);
    }
}
