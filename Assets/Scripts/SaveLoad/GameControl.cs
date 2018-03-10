using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameControl : MonoBehaviour
{

    public static GameControl control;
    public GameObject menu, store, inventory;
    public bool pause = false;
    public bool shop = false;
    public bool invt = false;

    public int JSLovePoints;
    public int HTMLLovePoints;
    public int JavaLovePoints;
    public int PYLovePoints;
    public int CSLovePoints;
    public int CPPLovePoints;
    public int money;
    public int day;
    public int hour;
    public int commandIndex;

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

    public void changeAmount(int minusMon)
    {
        if (money >= minusMon)
        {
            money = money - minusMon;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (pause == true)
            {
                Time.timeScale = 0.0f;
                pause = false;
                menu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1.0f;
                pause = true;
                menu.SetActive(false);
            }
        }
        if (Input.GetKeyDown("s"))
        {
            if (shop == true)
            {
                shop = false;
                store.SetActive(true);
            }
            else
            { 
                shop = true;
                store.SetActive(false);
            }
        }
        if (Input.GetKeyDown("i"))
        {
            if (invt == true)
            {
                invt = false;
                inventory.SetActive(true);
            }
            else
            {
                invt = true;
                inventory.SetActive(false);
            }
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



