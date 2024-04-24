using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<GameObjects> Inventory;
    public Inventory inv;
    bool open;
    public WorldSC world;

    private void Start()
    {
        world = GameObject.Find("World").GetComponent<WorldSC>();
    }

    private void Update()
    {
        if (open)
            Open();
    }

    private void UpdateInventory()
    {
        for (int x = 0; x < Inventory.Count; x++)
        {
            if (Inventory[x].amount > 0 && Inventory[x].amount < 99)
                for (int i = x + 1; i < Inventory.Count; i++)
                {
                    if (Inventory[x].ID == Inventory[i].ID && Inventory[i].amount > 0)
                    {
                        if (Inventory[x].amount + Inventory[i].amount > 99)
                        {
                            Inventory[i].amount -= 99 - Inventory[x].amount;
                            Inventory[x].amount = 99;
                            break;
                        }

                        Inventory[x].amount += Inventory[i].amount;
                        Inventory[i].amount = 0;
                    }
                }
        }

        Inventory.RemoveAll(item => item.amount <= 0);
    }

    private void Open()
    {
        UpdateInventory();
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();

        Inventory.Clear();
        foreach(UIItemSlot slot in inv.slots)
        {
            if (slot.HasItem)
            {
                Inventory.Add(slot.Item);
            }
        }

        if (Input.GetAxisRaw("Inventory") > 0)
        {
            Close(player);
        }
    }

    public void OpenChest(PlayerController player)
    {
        open = true;
        player.inInventory = true;
        inv = player.ChestInventory;
        for (int i = 0; i < Inventory.Count; i++)
        {
            inv.slots[i].Item = Inventory[i];
        }
    }

    public void Close(PlayerController player)
    {
        open = false;
        foreach(UIItemSlot slot in inv.slots)
        {
            if (slot.HasItem)
            {
                slot.Item = null;
            }
        }
        inv = null;
        player.ChestUI.SetActive(false);
    }
}