using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerScript : MonoBehaviour
{
    public Transform PlayerGender;
    public Transform PlayerPreferance;
    public InputField NameField;
    public string sex;
    public string preferedSex;
    public string playerName;
    // Use this for initialization
    void Start()
    {
        int menuIndex = PlayerGender.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = PlayerGender.GetComponent<Dropdown>().options;
        sex = menuOptions[menuIndex].text;
        menuOptions = PlayerPreferance.GetComponent<Dropdown>().options;
        preferedSex = menuOptions[menuIndex].text;
        playerName = NameField.text;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
   void TogglefullScreen()
   {
            Screen.fullScreen = !Screen.fullScreen;
   }
   
}
