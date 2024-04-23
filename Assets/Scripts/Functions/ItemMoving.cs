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

        if (Input.GetMouseButtonUp(0))
            HandleClick(CheckForSlot());
    }

    public void HandleClick(UIItemSlot clickedSlot)
    {
        pastClick = clickedSlot;
        if (clickedSlot == null)
            return;
        
        if (!cursorSlot.HasItem && !clickedSlot.HasItem)
            return;

        if (!cursorSlot.HasItem && clickedSlot.HasItem)
            cursorSlot.TakeAll(clickedSlot);

        if (cursorSlot.HasItem && !clickedSlot.HasItem)
            clickedSlot.TakeAll(cursorSlot);

        if (cursorSlot.HasItem && clickedSlot.HasItem)
        {
            if (cursorSlot.Item.ID == clickedSlot.Item.ID)
            {
                clickedSlot.add(99 - clickedSlot.Item.amount, cursorSlot);
            }
        }
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
}