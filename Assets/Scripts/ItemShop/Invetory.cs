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
            slots.Add(Instantiate(inventorySlot));
            slots[i].transform.SetParent(slotPanel.transform);
        }
            GameControl.control.inventory.SetActive(false);
        
    }

    public void AddItem(int id, float test)
    {
        Vector2 vector = new Vector2(0, test);
        Item itemToAdd = dataBase.FecthItemByID(id);
        for(int i =0;i< items.Count; i++)
        {
            if (items[i].ID == -1)
            {

                items[i] = itemToAdd;
                GameObject itemObj = Instantiate(inventoryItem);
                itemObj.transform.SetParent(slots[i].transform);
                itemObj.transform.position = Vector2.zero;
                itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                Debug.Log("yesa");
                break;
            }
        }

    }
    public void RemoveItem()
    {

    }
}
