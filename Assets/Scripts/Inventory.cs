using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public GameObject CharacterCanvas;
    public GameObject UIManager;

    GameObject InventoryPanel;
    GameObject SlotPanel;
    ItemDatabase databases;
    public GameObject InventorySlot;
    public GameObject InventoryItem;


    public GameObject[] EquipmentSlot;

    private int slotAmount = 25;
    private int slotEqAmount = 8;

    private static Inventory inventoryInstance;

    public List<ItemEquipment> itemsEquipment = new List<ItemEquipment>();

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

     void Awake()
    {
        
        //if (inventoryInstance == null)
        //{
            
        //    inventoryInstance = this;
        //    //DontDestroyOnLoad(inventoryInstance);
        //}
        //else if(inventoryInstance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
       
    }

    void Start()
    {
        CharacterCanvas.SetActive(true);
        //UIManager.SetActive(true);
        databases = GetComponent<ItemDatabase>();
        InventoryPanel = GameObject.Find("InventoryPanel");
        SlotPanel = InventoryPanel.transform.Find("SlotPanel").gameObject;
        for(int i = 0;i < slotAmount; i++)
        {
            items.Add(new Item());
            slots.Add(Instantiate(InventorySlot));
            slots[i].transform.SetParent(SlotPanel.transform,false);
            slots[i].GetComponent<Slot>().id = i;
        }
        for (int i = 0; i < slotEqAmount; i++)
        {
            itemsEquipment.Add(new ItemEquipment());
            //slots.Add(Instantiate(InventorySlot));
            //slots[i].transform.SetParent(SlotPanel.transform, false);
            //slots[i].GetComponent<Slot>().id = i;
        }


        
    }

    public void ClearItemInSlot()
    {
        for (int i = 0; i < slotAmount; i++)
        {

            if (items[i].ID != -1) { 
                GameObject.Destroy(slots[i].transform.GetChild(0).gameObject);
                items[i].ID = -1;
            }
        }

        for (int i = 0; i < slotEqAmount; i++)
        {
            if (itemsEquipment[i].ID != -1)
            {

                GameObject.Destroy(EquipmentSlot[i].transform.GetChild(0).gameObject);
                
                itemsEquipment[i].ID = -1;
            }
           
            //slots.Add(Instantiate(InventorySlot));
            //slots[i].transform.SetParent(SlotPanel.transform, false);
            //slots[i].GetComponent<Slot>().id = i;
        }
    }
    public void AddEquipment(int id)
    {
        ItemEquipment itemEq = databases.FetchEquipmentByID(id);
        //itemsEquipment[0] = itemEq;
        if(itemsEquipment[itemEq.ItemEquipmentType - 1].ID == -1)
        {

            Debug.Log("Add to slot : " + itemEq.Title);
            itemsEquipment[itemEq.ItemEquipmentType - 1] = itemEq;
            GameObject itemObject = Instantiate(InventoryItem);
            itemObject.GetComponent<ItemData>().itemEq = itemEq;
            itemObject.transform.SetParent(EquipmentSlot[itemEq.ItemEquipmentType - 1].transform, false);

            itemObject.GetComponent<Image>().sprite = itemEq.SpriteImage;
            itemObject.name = itemEq.Title;
        }
        
                
           

    }

    public void AddItem(int id)
    {
        Item ItemToAdd = databases.FetchItemByID(id);
        if (ItemToAdd.StackAble == 1 && this.CheckItemInventory(ItemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i].ID == id)
                {
                    
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    Debug.Log("Already :"  + data.amount);
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else 
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = ItemToAdd;
                    GameObject itemObject = Instantiate(InventoryItem);
                    itemObject.GetComponent<ItemData>().item = ItemToAdd;
                    itemObject.GetComponent<ItemData>().amount = 1;
                    itemObject.GetComponent<ItemData>().slot = i;
                    itemObject.transform.SetParent(slots[i].transform, false);
                    itemObject.GetComponent<Image>().sprite = ItemToAdd.SpriteImage;
                    itemObject.name = ItemToAdd.Title;
                    //itemObject.transform.position = Vector2.zero;
                    break;
                }
            }
        }

    }
    public void AddItem(int id,int inv)
    {
        Item ItemToAdd = databases.FetchItemByID(id, inv);
        if (ItemToAdd.StackAble == 1 && this.CheckItemInventory(ItemToAdd))
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {

                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    Debug.Log("Already :" + data.amount);
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = ItemToAdd;
                    GameObject itemObject = Instantiate(InventoryItem);
                    itemObject.GetComponent<ItemData>().item = ItemToAdd;
                    itemObject.GetComponent<ItemData>().amount = 1;
                    itemObject.GetComponent<ItemData>().slot = i;
                    itemObject.transform.SetParent(slots[i].transform, false);
                    itemObject.GetComponent<Image>().sprite = ItemToAdd.SpriteImage;
                    itemObject.name = ItemToAdd.Title;
                    //itemObject.transform.position = Vector2.zero;
                    break;
                }
            }
        }

    }

    bool CheckItemInventory(Item item)
    {
        for(int i = 0;i < items.Count; i++)
        {
            if(items[i].ID == item.ID)
            {
                return true;
            }
        }
        return false;
    }

    public void Test()
    {
        Debug.Log("KKKKKKK!!!");
    }
}
