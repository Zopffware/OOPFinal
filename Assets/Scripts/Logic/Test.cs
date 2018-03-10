using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    public Button TestButton;

    // Use this for initialization
    void Start() {
        Button btn = TestButton.GetComponent<Button>();
        btn.onClick.AddListener(test);
        ScriptParser.readScript("..\\exampleScript.txt");
    }

    // Update is called once per frame
    void Update() {

    }

    void test() {
        ScriptParser.advanceScript();
    }
}
