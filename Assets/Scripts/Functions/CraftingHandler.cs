using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    public void craft(List<GameObjects> inv, GameObjects ItemToCraft)
    {
        int recepieLength = ItemToCraft.recepie.Count;
        int obtainedMaterials = 0;
        bool canCraft = false;
        foreach (Recepie ID in ItemToCraft.recepie)
        {
            foreach (GameObjects g in inv)
            {
                if (g.ID == ID.ID)
                {
                    if (g.amount >= ID.amount)
                    {
                        if (obtainedMaterials < recepieLength)
                            obtainedMaterials++;
                    }
                }
            }
        }
        if (obtainedMaterials == recepieLength)
        {
            canCraft = true;
        }
        if (!canCraft)
            return;

        foreach (Recepie ID in ItemToCraft.recepie)
        {
            foreach (GameObjects g in inv)
            {
                if (g.ID == ID.ID)
                {
                    g.amount -= ID.amount;
                }
            }
        }

        GameObjects craftedItem = new GameObjects()
        {
            amount = ItemToCraft.amount,
            amountWhenCrafted = ItemToCraft.amountWhenCrafted,
            Name = ItemToCraft.Name,
            ID = ItemToCraft.ID,
            StackAmount = ItemToCraft.StackAmount,
            sprite = ItemToCraft.sprite,
            recepie = ItemToCraft.recepie,
            Craftable = ItemToCraft.Craftable
        };
        craftedItem.amount = ItemToCraft.amountWhenCrafted;

        inv.Add(craftedItem);
    }

    public void initiateCraft(int ID)
    {
        PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
        WorldSC world = GameObject.Find("World").GetComponent<WorldSC>();
        List<GameObjects> inv = player.Inventory;
        GameObjects itc = world.ItemsInGame[ID];
        if (itc.Craftable)
            craft(inv, itc);
    }
}