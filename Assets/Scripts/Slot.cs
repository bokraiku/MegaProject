using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour, IDropHandler {

    public int id;
    public Inventory inven;

    void Start()
    {
        inven = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData itemDrop = eventData.pointerDrag.GetComponent<ItemData>();

        
        if (inven.items[id].ID == -1)
        {
            inven.items[itemDrop.slot] = new Item();
            inven.items[id] = itemDrop.item;
            itemDrop.slot = id;
        }
        else if(itemDrop.slot != id)
        {
            Transform item = this.transform.GetChild(0);
            item.GetComponent<ItemData>().slot = itemDrop.slot;
            item.transform.SetParent(inven.slots[itemDrop.slot].transform);
            item.transform.position = inven.slots[itemDrop.slot].transform.position;

            // Move Item to new position
            itemDrop.slot = id;
            itemDrop.transform.SetParent(this.transform);
            itemDrop.transform.position = this.transform.position;

            inven.items[itemDrop.slot] = item.GetComponent<ItemData>().item;
            inven.items[id] = itemDrop.item;
        }
    }


}
