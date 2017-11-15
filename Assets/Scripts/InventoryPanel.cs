using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPanel : MonoBehaviour,IPointerClickHandler
{
    public ToolTip tooltip;
    public Inventory inv;

    void Start()
    {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        Debug.Log("Inventory : " + inv);
        tooltip = inv.GetComponent<ToolTip>();
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerEnter.name);
        tooltip.Deactivate();
    }
}
