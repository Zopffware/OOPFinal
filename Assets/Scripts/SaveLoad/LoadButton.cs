using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour {
    public Button Load;
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(LoadGame);
    }

    void LoadGame()
    {
        GameControl.control.Load();
    }
}
