using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    
    public static GameControl control;
    public GameObject menu, store, inventory;
    public bool pause = false;
    public bool shop = false;
    public bool invt = false;

    public int JSHTMLLovePoints;
    public int JavaLovePoints;
    public int PYLovePoints;
    public int CSLovePoints;
    public int CPPLovePoints;
    public int money;
    public int day;
    public int hour;
    public int commandIndex;
    public List<ICommand> currentScript;
    
    Invetory Inventory;
    void Awake()
    {
        Inventory = FindObjectOfType<Invetory>();
        if (control == null)
        {
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
        data.JSHTMLLovePoints = JSHTMLLovePoints;
        data.PYLovePoints = PYLovePoints;
        data.JavaLovePoints = JavaLovePoints;
        data.CSLovePoints = CSLovePoints;
        data.CPPLovePoints = CPPLovePoints;
        data.money = money;
        data.day = day;
        data.hour = hour;
        data.currentScript = currentScript;

        Debug.Log(Inventory.items);
        data.items = Inventory.items;
        



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

            
            JSHTMLLovePoints = data.JSHTMLLovePoints;
            PYLovePoints = data.PYLovePoints;
            JavaLovePoints = data.JavaLovePoints;
            CSLovePoints = data.CSLovePoints;
            CPPLovePoints = data.CPPLovePoints;
            money = data.money;
            day = data.day;
            hour = data.hour;
            currentScript = data.currentScript;

            Debug.Log(data.items);
                Inventory.items = data.items;
            
        }
    }

    public void changeAmount(int minusMon)
    {
        for(int i = 0; i < Inventory.slots.Count; i++)
        {
            if (Inventory.items[i].ID == -1)
            {
                if (money >= minusMon)
                {
                    int v = 0;
                    money = money - minusMon;
                    if(minusMon == 750)
                    {
                         v = 0;
                    }else if(minusMon ==850)
                    {
                        v = 1;
                    }else if (minusMon == 925)
                    {
                        v = 2;
                    }else if (minusMon == 1000)
                    {
                        v = 3;
                    }else if (minusMon == 800)
                    {
                        v = 4;
                    }
                    Inventory.AddItem(v, 1.525879e-05f);
                    break;
                }

            }
        }

    }
    public void itemUse(int a)
    {
        Inventory.RemoveItem(a);
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (pause == true)
            {
                Time.timeScale = 0.0f;
                pause = false;
                menu.SetActive(false);
            }
            else
            {
                Time.timeScale = 1.0f;
                pause = true;
                menu.SetActive(true);
            }
        }
        if (Input.GetKeyDown("s"))
        {
            if (shop == true)
            {
                shop = false;
                store.SetActive(false);
            }
            else
            { 
                shop = true;
                store.SetActive(true);
            }
        }
        if (Input.GetKeyDown("i"))
        {
            if (invt == true)
            {
                invt = false;
                inventory.SetActive(false);
            }
            else
            {
                invt = true;
                inventory.SetActive(true);
            }
        }
        if (Input.GetKeyDown("space"))
        {
            ScriptParser.advanceScript();
        }
    }
}

[Serializable]
class PlayerData
{
    
    public int JSHTMLLovePoints;
    public int JavaLovePoints;
    public int PYLovePoints;
    public int CSLovePoints;
    public int CPPLovePoints;
    public int money;
    public int day;
    public int hour;
    public List<ICommand> currentScript;
    public List<GameObject> slots = new List<GameObject>( 9);
    public List<Item> items = new List<Item>(9);
}

//GameControl.control.money += x;



