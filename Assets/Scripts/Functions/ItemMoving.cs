using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMoving : MonoBehaviour
{
    public UIItemSlot cursorSlot;
    public UIItemSlot pastClick;
    public GraphicRaycaster raycaster;
    public PointerEventData ped;
    public EventSystem es;

    private void Update()
    {
        cursorSlot.transform.position = Input.mousePosition;
        cursorSlot.gameObject.GetComponent<Image>().enabled = cursorSlot.HasItem;

        if (Input.GetMouseButtonUp(0))
            HandleClick(CheckForSlot(), 0);
        if (Input.GetMouseButtonUp(1))
            HandleClick(CheckForSlot(), 1);
    }

    public void HandleClick(UIItemSlot clickedSlot, int mbClicked)
    {
        if (mbClicked == 0)
        {
            if (clickedSlot == null)
                return;

            else if (!cursorSlot.HasItem && !clickedSlot.HasItem)
                return;

            else if (!cursorSlot.HasItem && clickedSlot.HasItem)
                cursorSlot.TakeAll(clickedSlot);

            else if (cursorSlot.HasItem && !clickedSlot.HasItem)
                clickedSlot.TakeAll(cursorSlot);

            else if (cursorSlot.HasItem && clickedSlot.HasItem)
            {
                if (cursorSlot.Item.ID == clickedSlot.Item.ID)
                {
                    if (cursorSlot.Item.amount + clickedSlot.Item.amount <= 99)
                        clickedSlot.add(cursorSlot.Item.amount, cursorSlot);
                    else
                    {
                        clickedSlot.add(99 - clickedSlot.Item.amount, cursorSlot);
                    }
                }
                else
                    cursorSlot.SwitchStack(clickedSlot);
            }
        }
        else if (mbClicked == 1)
        {
            if (clickedSlot == null)
                return;

            else if (!cursorSlot.HasItem && !clickedSlot.HasItem)
                return;

            else if (!cursorSlot.HasItem && clickedSlot.HasItem)
                cursorSlot.TakeHalf(clickedSlot);

            else if (cursorSlot.HasItem && !clickedSlot.HasItem)
                clickedSlot.TakeHalf(cursorSlot);

            else if (cursorSlot.HasItem && clickedSlot.HasItem)
            {
                if (cursorSlot.Item.ID == clickedSlot.Item.ID)
                {
                    if (clickedSlot.Item.amount + (cursorSlot.Item.amount / 2) <= 99)
                        clickedSlot.add(cursorSlot.Item.amount / 2, cursorSlot);
                    else
                    {
                        int amt = 99 - clickedSlot.Item.amount;
                        clickedSlot.add(amt, cursorSlot);
                    }
                }
                else
                    cursorSlot.SwitchStack(clickedSlot);
            }
        }
        if (clickedSlot.HasItem)
        {
            if (clickedSlot.Item.amount < 99)
                pastClick = clickedSlot;
        }
        else
            pastClick = clickedSlot;
    }

    private UIItemSlot CheckForSlot()
    {
        ped = new PointerEventData(es);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(ped, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.tag == "UIItemSlot")
                return result.gameObject.GetComponent<UIItemSlot>();
        }

        return null;
    }

    public void ReturnStack()
    {
        if (pastClick == null || !cursorSlot.HasItem)
            return;

        if (pastClick.HasItem && pastClick.Item.ID == cursorSlot.Item.ID)
            pastClick.add(cursorSlot.Item.amount, cursorSlot);
        else
            pastClick.TakeAll(cursorSlot);
    }
}