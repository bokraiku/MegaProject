using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler {

    public Item item;
    public ItemEquipment itemEq;
    public int amount;
    public int slot;

    public Inventory inv;
    private Vector2 offset;

    private ToolTip tooltip;

    void Start()
    {
        inv = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        tooltip = inv.GetComponent<ToolTip>();
        
    }

    /*
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);

            this.transform.SetParent(this.transform.root);
            this.transform.position = eventData.position - offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        this.transform.SetParent(inv.slots[slot].transform);
        this.transform.position = inv.slots[slot].transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    */

    public void OnPointerEnter(PointerEventData eventData)
    {
        //tooltip.Activate(item);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        //tooltip.Deactivate();   
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tooltip.Deactivate();
        tooltip.Activate(item, itemEq);
    }
}
