using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(save);
    }

    void save()
    {
        GameControl.control.Save();
    }

}