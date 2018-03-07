using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

    public static GameControl control;

    public int JSLovePoints;
    public int HTMLLovePoints;
    public int JavaLovePoints;
    public int CSLovePoints;
    public int CPPLovePoints;
    public int PYLovePoints;
    public int money;
    public int day;
    public int hour;

	void Awake () {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this) {
            Destroy(gameObject);
        }
	}

    private void OnGUI()
    {
        /*GUI.Label(new Rect(10, 10, 400, 90), "Love Points: " + JSLovePoints);
        GUI.Label(new Rect(10, 40, 400, 90), "Day: " + day);
        GUI.Label(new Rect(10, 70, 400, 90), "Hour: " + hour);*/
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.lovePoints = JSLovePoints;
        data.day = day;
        data.hour = hour;
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        Debug.Log("here");
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            JSLovePoints = data.lovePoints;
            day = data.day;
            hour = data.hour;
        }
    }
}

[Serializable]
class PlayerData
{
    public int lovePoints;
    public int day;
    public int hour;
}
