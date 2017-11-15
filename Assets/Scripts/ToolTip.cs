using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class ToolTip : MonoBehaviour {

    private Item item;
    private ItemEquipment itemEq;
    private string data;
    private GameObject tooltip;
    private GameObject EquipmentTooltip;
    public Transform tooltipSpawn;

    public SocketController socketController; 

    

    public GameObject EquipButton;
    private void Start()
    {
        tooltip = GameObject.Find("Tooltip");
        EquipmentTooltip = GameObject.Find("EquipmentTooltip");
        socketController = GameObject.Find("Inventory").GetComponent<SocketController>();
        tooltip.SetActive(false);
        EquipmentTooltip.SetActive(false);
    }
    private void Update()
    {
        if (tooltip != null && tooltip.activeSelf)
        {
            tooltip.transform.position = tooltipSpawn.transform.position; //Input.mousePosition;
        }

    }
    public void Activate(Item item,ItemEquipment itemEq)
    {
        this.item = item;
        this.itemEq = itemEq;
        ContructDataString();

        if(itemEq == null && item.ItemType != 1)
        {
            tooltip.SetActive(true);
        }else if (item != null && item.ItemType == 1)
        {
            EquipmentTooltip.SetActive(true);
        }
        else
        {
            EquipmentTooltip.SetActive(true);
        }
        
    }
    public void Deactivate()
    {
        
        EquipmentTooltip.SetActive(false);
        tooltip.SetActive(false);
    }

    public void ManageEquipment()
    {
        if(item != null)
        {
            Debug.Log("Inventory" + item.ID);
            socketController.Equipment(1000001, item.ID, item.Inv_id,1);
        }
        if(itemEq != null)
        {
            //Debug.Log(socketController);
            socketController.UnEquipment(1000001, itemEq.ID,itemEq.Inv_id,0);
            Debug.Log("Inventory" + itemEq.ID);

        }
        EquipmentTooltip.SetActive(false);
    }

    public void ContructDataString()
    {
        if(item != null)
        {
            if(item.ItemType == 1)
            {
                Debug.Log("Item Inventory");
                GameObject ItemName = EquipmentTooltip.transform.Find("itemName").gameObject;
                GameObject itemStatus = EquipmentTooltip.transform.Find("itemStatus").gameObject;
                GameObject itemOption = EquipmentTooltip.transform.Find("itemOption").gameObject;
                //GameObject ManageObject = EquipmentTooltip.transform.Find("itemManage").gameObject.transform.Find("EquipButton").gameObject;


                
                if(item.IsEquip == 0)
                {
                    EquipButton.transform.GetChild(0).GetComponent<Text>().text = "สวมใส่";
                }
                else
                {
                    EquipButton.transform.GetChild(0).GetComponent<Text>().text = "ถอดอุปกรณ์";
                }

                ItemName.transform.Find("Name").GetComponent<Text>().text = item.Title;
                ItemName.transform.Find("Enchanted").GetComponent<Text>().text = (item.Enchanted > 0) ? "+" + item.Enchanted.ToString() : "";

                ItemName.transform.Find("Slot").transform.Find("Item").GetComponent<Image>().sprite = item.SpriteImage;

                if (item.Status != null)
                {
                    Text Status = itemStatus.transform.Find("Status").GetComponent<Text>();
                    Status.text = "";

                    foreach (string key in item.Status.Keys)
                    {
                        if((int)item.Status[key] != 0)
                        {
                            Status.text += key.ToUpper() + ":" + item.Status[key] + "\n";
                        }
                        
                    }

                }
                if (item.Option != null)
                {
                    Text option = itemOption.transform.Find("Option").GetComponent<Text>(); ;
                    option.text = "";
                    foreach (string key in item.Option.Keys)
                    {
                        if ((int)item.Option[key] != 0)
                        {
                            option.text += key.ToUpper() + ":" + item.Option[key] + "\n";
                        }
                            
                    }
                }
               
            }
            else
            {
                data = "<color=#92d7fc><b>" + item.Title + "</b></color>\n\n\n";
                data += "<color=#FFFFFF><b>" + item.Description + "</b></color>\n\n";
                tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
            }

        }
        else
        {
            EquipButton.transform.GetChild(0).GetComponent<Text>().text = "ถอดอุปกรณ์";

            GameObject ItemName = EquipmentTooltip.transform.Find("itemName").gameObject;
            GameObject itemStatus = EquipmentTooltip.transform.Find("itemStatus").gameObject;
            GameObject itemOption = EquipmentTooltip.transform.Find("itemOption").gameObject;

            ItemName.transform.Find("Name").GetComponent<Text>().text = itemEq.Title;
            ItemName.transform.Find("Enchanted").GetComponent<Text>().text = (itemEq.Enchanted > 0)?"+"+itemEq.Enchanted.ToString():"";

            ItemName.transform.Find("Slot").transform.Find("Item").GetComponent<Image>().sprite = itemEq.SpriteImage;

            if (itemEq.Status != null)
            {
                Text Status = itemStatus.transform.Find("Status").GetComponent<Text>();
                Status.text = "";

                foreach(string key in itemEq.Status.Keys)
                {
                   
                    if ((int)itemEq.Status[key] != 0)
                    {
                        Status.text += key.ToUpper() + ":" + itemEq.Status[key] + "\n";
                    }
                }
                /*
                Dictionary<string, int> items = itemEq.Status.ToJson();
                foreach (Dictionary<string,int> tt in items)
                {
                    //Debug.Log(items.ke);
                    Status.text += items.Keys + ":" + items.Values + "\n";
                }
                */
            }
            if(itemEq.Option != null)
            {
                Text option = itemOption.transform.Find("Option").GetComponent<Text>(); ;
                option.text = "";
                foreach (string key in itemEq.Option.Keys)
                {
                    
                    if ((int)itemEq.Option[key] != 0)
                    {
                        option.text += key.ToUpper() + ":" + itemEq.Option[key] + "\n";
                    }
                }
            }
            data = "<color=#92d7fc><b>" + itemEq.Title + "</b></color>\n\n\n";
            data += "<color=#FFFFFF><b>" + itemEq.Description + "</b></color>\n\n";
            //tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
        }

    
    }
}
