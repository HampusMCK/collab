using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    PlayerController player;
    WorldSC world;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        world = GameObject.Find("World").GetComponent<WorldSC>();
    }

    public void craft(int ID)
    {
        GameObjects ItemToCraft = world.ItemsInGame[ID];
        if (!ItemToCraft.Craftable)
            return;
        int recepieLength = ItemToCraft.recepie.Count;
        int obtainedMaterials = 0;
        bool canCraft = false;
        foreach (Recepie recepie in ItemToCraft.recepie)
        {
            foreach (GameObjects g in player.Inventory)
            {
                if (g.ID == recepie.ID)
                {
                    if (g.amount >= recepie.amount)
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

        foreach (Recepie recepie in ItemToCraft.recepie)
        {
            foreach (GameObjects g in player.Inventory)
            {
                if (g.ID == recepie.ID)
                {
                    g.amount -= recepie.amount;
                }
            }
        }

        GameObjects craftedItem = new GameObjects()
        {
            ID = ItemToCraft.ID,
            Name = ItemToCraft.Name,
            amount = ItemToCraft.amount,
            prefab = ItemToCraft.prefab,
            sprite = ItemToCraft.sprite,
            recepie = ItemToCraft.recepie,
            Craftable = ItemToCraft.Craftable,
            buildingID = ItemToCraft.buildingID,
            isPlaceable = ItemToCraft.isPlaceable,
            StackAmount = ItemToCraft.StackAmount,
            amountWhenCrafted = ItemToCraft.amountWhenCrafted
        };
        craftedItem.amount = ItemToCraft.amountWhenCrafted;

        player.AddCraftedItem(craftedItem);
    }
}