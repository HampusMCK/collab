using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public GameObjects Item = null;
    public TMP_Text amount;
    public Image img;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Item != null)
            if (Item.amount > 0)
            {
                amount.enabled = true;
                img.enabled = true;

                amount.text = Item.amount.ToString();
                img.sprite = Item.sprite;
            }

        if (Item == null)
        {
            amount.enabled = false;
            img.enabled = false;

            amount.text = null;
            img.sprite = null;
        }
    }
}
