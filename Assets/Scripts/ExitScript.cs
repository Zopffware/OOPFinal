using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ExitScript : MonoBehaviour {
    public Button Exit;
    // Use this for initialization
    void Start () {
        Button btn = Exit.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    void TaskOnClick()
    {
        Application.Quit();
    }


}
