using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour
{

    public static GameControl control;

    public int JSLovePoints;
    public int HTMLLovePoints;
    public int JavaLovePoints;
    public int PYLovePoints;
    public int CSLovePoints;
    public int CPPLovePoints;
    public int money;
    public int day;
    public int hour;
    

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 10, 100, 30), "LovePoints: " + JSLovePoints);
    //}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.JSLovePoints = JSLovePoints;
        data.HTMLLovePoints = HTMLLovePoints;
        data.PYLovePoints = PYLovePoints;
        data.JavaLovePoints = JavaLovePoints;
        data.CSLovePoints = CSLovePoints;
        data.CPPLovePoints = CPPLovePoints;
        data.money = money;
        data.day = day;
        data.hour = hour;

        bf.Serialize(file, data);
        file.Close();
    }
    public void changeAmount(int minusMon)
    {
        if (money>=minusMon) {
            money = money - minusMon;
        }
        
    }
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            JSLovePoints = data.JSLovePoints;
            HTMLLovePoints = data.HTMLLovePoints;
            PYLovePoints = data.PYLovePoints;
            JavaLovePoints = data.JavaLovePoints;
            CSLovePoints = data.CSLovePoints;
            CPPLovePoints = data.CPPLovePoints;
            money = data.money;
            day = data.day;
            hour = data.hour;
        }
    }
}

[Serializable]
class PlayerData
{
    public int JSLovePoints;
    public int HTMLLovePoints;
    public int JavaLovePoints;
    public int PYLovePoints;
    public int CSLovePoints;
    public int CPPLovePoints;
    public int money;
    public int day;
    public int hour;
}

//GameControl.control.money += x;



