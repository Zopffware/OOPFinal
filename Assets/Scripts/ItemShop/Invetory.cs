using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invetory : MonoBehaviour {
    
    GameObject invetoryPanel;
    GameObject slotPanel;
    ItemDataBase dataBase;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public Sprite back;
    Vector2 vector = new Vector2(0, 1.525879e-05f);

     int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

     void Start()
    {
        dataBase = GetComponent<ItemDataBase>();
        slotAmount = 9;
        invetoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = invetoryPanel.transform.Find("Slot Panel").gameObject;
        for(int i = 0; i < slotAmount; i++)
        {
            
            items.Add(new Item());
            
        }
            GameControl.control.inventory.SetActive(false);

    }

    public void AddItem(int id, float test)
    {
        Vector2 vector = new Vector2(0, test);
        Item itemToAdd = dataBase.FecthItemByID(id);
        for(int i =0;i<items.Count; i++)
        {
            if (items[i].ID == -1)
            {

                items[i] = itemToAdd;
                
                //GameObject itemObj = itemObjj.GetComponentInChildren<GameObject>();
              
                Image a = slots[i].transform.Find("Item 1").GetComponent<Image>();


                a.GetComponent<Image>().sprite = itemToAdd.getSprite();
                a.transform.SetParent(slots[i].transform);
                Debug.Log("yesa");
                break;
            }
        }

    }
    public void RemoveItem( int b)
    {
        if(GameObject.Find("LeftPortrait").GetComponentInChildren<Image>().sprite.name!=null)
        {
            int lp = items[b].Lovepoints;
            string namne = GameObject.Find("LeftPortrait").GetComponentInChildren<Image>().sprite.name;

            Debug.Log(namne);
            switch (namne)
            {
                
                
                case "JavaGirl":
                    GameControl.control.JavaLovePoints += lp;
                    Debug.Log(lp);
                    break;
                case "C#Girl":
                    GameControl.control.CSLovePoints += lp;
                    Debug.Log(lp);
                    break;
                case "C++Girl":
                    GameControl.control.CPPLovePoints += lp;
                    Debug.Log(lp);
                    break;
                case "PythonGirl":
                    GameControl.control.PYLovePoints += lp;
                    Debug.Log(lp);
                    break;
                case "JSGirl":
                    GameControl.control.JSHTMLLovePoints += lp;
                    Debug.Log(lp);
                    break;
                default:
                    Debug.Log("nope");
                    break;

            }
            items[b] = new Item();

            Image a = slots[b].transform.Find("Item 1").GetComponent<Image>();


            a.GetComponent<Image>().sprite = back;
            
            Debug.Log("yesa");
        }
        
            

        
    }
}
