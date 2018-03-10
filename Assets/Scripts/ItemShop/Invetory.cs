using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invetory : MonoBehaviour {

    GameObject invetoryPanel;
    GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject invetoryItem;

     int slotAmount;
    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

     void Start()
    {
        slotAmount = 9;
        invetoryPanel = GameObject.Find("Inventory Panel");
        slotPanel = invetoryPanel.transform.Find("Slot Panel").gameObject;
        for(int i = 0; i < slotAmount; i++)
        {
            slots.Add(Instantiate(inventorySlot));
            slots[i].transform.SetParent(slotPanel.transform);
        }
    }
}
