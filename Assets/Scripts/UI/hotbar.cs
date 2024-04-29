using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class hotbar : MonoBehaviour
{
    public Sprite ItemSlot;
    public Sprite HighlightedItemSlot;
    public List<Image> slots;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHighlighted(int highlight)
    {
        foreach (Image i in slots)
        {
            i.sprite = ItemSlot;
        }
        slots[highlight].sprite = HighlightedItemSlot;
    }
}