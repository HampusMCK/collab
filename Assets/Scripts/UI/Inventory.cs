using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    PlayerController player;
    public GameObject ItemSlotPrefab;
    public List<UIItemSlot> slots;
    public int ID;
    public Chest owner;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        for (int i = 0; i < 49; i++)
        {
            GameObject g = Instantiate(ItemSlotPrefab);
            g.name = "Slot" + (i + 1);
            g.transform.SetParent(transform);
            UIItemSlot itemSlot = g.GetComponent<UIItemSlot>();
            if (i == 3)
                itemSlot.Item = player.Inventory[0];
            slots.Add(itemSlot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (ID == 0)
        //     for (int i = 0; i < slots.Count; i++)
        //     {
        //         if (player.Inventory.Count > i)
        //             slots[i].Item = player.Inventory[i];
        //         else
        //             slots[i].Item = null;
        //     }
        // else
        //     for (int i = 0; i < slots.Count; i++)
        //     {
        //         if (owner.Inventory.Count > i)
        //             slots[i].Item = owner.Inventory[i];
        //         else
        //             slots[i].Item = null;
        //     }
    }
}