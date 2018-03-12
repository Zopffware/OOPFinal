using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class ItemDataBase : MonoBehaviour {
    private List<Item> dataBase = new List<Item>();
    private JsonData itemData;

    private void Start()
    {
       
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();

        
        Debug.Log(FecthItemByID(0).Description);
    }
    
    public Item FecthItemByID(int id)
    {
        for(int i = 0;i < dataBase.Count; i++)
        
            if (dataBase[i].ID == id) 
                return dataBase[i];

        return null;
        
    }

    void ConstructItemDatabase()
    {
        for(int i =0; i<itemData.Count; i++)
        {
            dataBase.Add(new Item((int)itemData[i]["id"],itemData[i]["title"].ToString(),(int)itemData[i]["value"],(int)itemData[i]["lovepoints"],itemData[i]["decription"].ToString(),itemData[i]["slug"].ToString()));
        }
    }
}
[Serializable]
public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int Lovepoints { get; set; }
    public string Description { get; set; }
    public string slug { get; set; }
   
    public Sprite getSprite()
    {
        return Resources.Load<Sprite>( "Sprites"+ slug);
    }


    public Item(int id , string title, int value,int lovepoints, string description,string slug)
    {
        this.ID = id;
        this.Title = title;
        this.Value = value;
        this.Lovepoints = lovepoints;
        this.Description = description;
    }
    public Item()
    {
        this.ID = -1;
    }
}
