using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    PlayerController player;
    public GameObject ItemSlotPrefab;
    public List<UIItemSlot> slots;
    public Inventory main;
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
            slots.Add(itemSlot);
        }

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }
}