using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public GameObjects Item = new();
    public TMP_Text amount;
    public Image img;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(HasItem + ", " + gameObject.name);
        if (HasItem)
        {
            if (Item.amount <= 0)
            {
                Item = null;
                return;
            }
            // Debug.Log(HasItem + ", " + gameObject.name);

            amount.enabled = true;
            img.enabled = true;

            amount.text = Item.amount.ToString();
            img.sprite = Item.sprite;
        }
        else
        {
            amount.enabled = false;
            img.enabled = false;

            amount.text = null;
            img.sprite = null;
        }
    }

    public void InsetStack(UIItemSlot stack)
    {
        Item = stack.Item;
    }

    public void EmptyStack()
    {
        Item = null;
    }

    public void add(int amount, UIItemSlot stack)
    {
        Item.amount += amount;
        stack.Item.amount -= amount;
    }

    public void TakeAll(UIItemSlot stack)
    {
        Item = stack.Item;
        stack.EmptyStack();
    }

    public bool HasItem
    {
        get
        {
            if (Item == null || Item.amount <= 0)
                return false;
            else
                return true;
        }
    }
}