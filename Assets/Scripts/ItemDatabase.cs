using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Net;
using UnityEngine.UI;
using UnityEngine.Networking;
using SocketIO;


public class ItemDatabase : MonoBehaviour
{
    
    private List<Item> database = new List<Item>();
    private List<ItemEquipment> eqDatabase = new List<ItemEquipment>();
    private JsonData itemData;

    private JsonData equipment;
    private JsonData attr;

    private Inventory inv;

    public GameObject MsgBox;


    public Text MSG;

    private SocketIOComponent socket;
    private PlayerStatus playerStatus;
    


    // Use this for initialization
    void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        //socket.Connect();
        inv = GameObject.Find("Inventory").GetComponent<Inventory>();
        //itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/item.json"));
        //MsgBox = GameObject.Find("MessagePanel");
        //MSG = MsgBox.transform.GetChild(0).GetComponent<Text>();
        if (!socket.IsConnected)
        {
            socket.Connect();
        }

        //MSG.text = "Loading Data ....";

        socket.On("received_item", GETITEM);
       

        StartCoroutine("WaitForData");


    }
    IEnumerator WaitForData()
    {
        
        //data = rq.GET();
        yield return new WaitForSeconds(1f);
        socket.Emit("getitem");
       
  
    }
    public void GETITEM(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received : " + e.data.GetField("items").ToString() + "|"+ e.data.GetField("equipment").ToString());

        itemData    = JsonMapper.ToObject(e.data.GetField("items").ToString());
        equipment   = JsonMapper.ToObject(e.data.GetField("equipment").ToString());
       
       
        ///MsgBox.SetActive(false);
        inv.ClearItemInSlot();
        ConstructInventoryManager();
        ConstructEquipementManager();
    }



    void ConstructInventoryManager()
    {
        for(int i = 0;i < itemData.Count; i++)
        {
            try
            {
                if ((int)itemData[i]["item_is_equip"] == 0) {
                    database.Add(new Item((int)itemData[i]["item_id"], itemData[i]["item_name"].ToString(), (int)itemData[i]["item_enchanted"],
                   itemData[i]["item_sprite"].ToString(), (int)itemData[i]["item_min_level"], itemData[i]["item_description"].ToString(), (int)itemData[i]["item_is_equip"],
                   (int)itemData[i]["item_type"], (int)itemData[i]["item_equipment_type"], (int)itemData[i]["item_stackable"], (int)itemData[i]["inv_id"],(JsonData)itemData[i]["status"], (JsonData)itemData[i]["option"]));
                    inv.AddItem((int)itemData[i]["item_id"], (int)itemData[i]["inv_id"]);
                }

              
                
            }
            catch(System.Exception e)
            {
                Debug.Log(e.Message + "|" + e.Source);
            }
            
        }
    }
    void ConstructEquipementManager()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            
            try
            {
                if ((int)itemData[i]["item_is_equip"] == 1)
                {
                    Debug.Log("Add Item ID : " + (int)itemData[i]["item_id"]);
                    eqDatabase.Add(new ItemEquipment((int)itemData[i]["item_id"], itemData[i]["item_name"].ToString(), (int)itemData[i]["item_enchanted"],
                   itemData[i]["item_sprite"].ToString(), (int)itemData[i]["item_min_level"], itemData[i]["item_description"].ToString(), (int)itemData[i]["item_is_equip"],
                   (int)itemData[i]["item_type"], (int)itemData[i]["item_equipment_type"], (int)itemData[i]["item_stackable"], (int)itemData[i]["inv_id"], (JsonData)itemData[i]["status"], (JsonData)itemData[i]["option"]));
                    inv.AddEquipment((int)itemData[i]["item_id"]);
                   
                }

            }
            catch (System.Exception e)
            {

                Debug.Log(e.Message + "|" + e.Source);
            }

        }
    }

    public Item FetchItemByID(int id)
    {
        for(int i =0;i < database.Count; i++)
        {
            if(database[i].ID == id)
            {
                return database[i];
            }
        }
        return null;
    }
    public Item FetchItemByID(int id,int inv)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == id && database[i].Inv_id == inv)
            {
                return database[i];
            }
        }
        return null;
    }
    public ItemEquipment FetchEquipmentByID(int id)
    {
        for (int i = 0; i < eqDatabase.Count; i++)
        {
            if (eqDatabase[i].ID == id)
            {
                return eqDatabase[i];
            }
        }
        return null;
    }

}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int Enchanted { get; set; }
    public string Description { get; set; }
    public int StackAble { get; set; }
    public Sprite SpriteImage { get; set; }
    public int MinLevel { get; set; }
    public int IsEquip { get; set; }
    public int ItemType { get; set; }
    public JsonData Status { get; set; }
    public JsonData Option { get; set; }
    public int ItemEquipmentType { get; set; }
    public int Inv_id { get; set; }



    public Item(int id,string title,int enchanted, string item_spire, int item_min_level, string description,int is_equip,int item_type, int item_equipment_type,int stackable,int inv_id, JsonData status, JsonData option)
    {
        this.ID = id;
        this.Inv_id = inv_id;
        this.Title = title;
        this.Enchanted = enchanted;
        this.Description = description;
        this.MinLevel = item_min_level;
        this.IsEquip = is_equip;
        this.ItemType = item_type;
        this.Status = status;
        this.Option = option;
        this.ItemEquipmentType = item_equipment_type;
        this.StackAble = stackable;
        this.SpriteImage = Resources.Load<Sprite>("Sprites/items/" + item_spire);
    }
    public Item()
    {
        this.ID = -1;
    }


}


public class ItemEquipment
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public int Enchanted { get; set; }
    public string Description { get; set; }
    public int StackAble { get; set; }
    public Sprite SpriteImage { get; set; }
    public int MinLevel { get; set; }
    public int IsEquip { get; set; }
    public int ItemType { get; set; }
    public JsonData Status { get; set; }
    public JsonData Option { get; set; }
    public int ItemEquipmentType { get; set; }
    public int Inv_id { get; set; }

    public ItemEquipment(int id, string title, int enchanted, string item_spire, int item_min_level, string description, int is_equip, int item_type, int item_equipment_type, int stackable, int inv_id, JsonData status, JsonData option)
    {
        this.ID = id;
        this.Inv_id = inv_id;
        this.Title = title;
        this.Enchanted = enchanted;
        this.Description = description;
        this.MinLevel = item_min_level;
        this.IsEquip = is_equip;
        this.ItemType = item_type;
        this.Status = status;
        this.Option = option;
        this.ItemEquipmentType = item_equipment_type;
        this.StackAble = stackable;
        this.SpriteImage = Resources.Load<Sprite>("Sprites/items/" + item_spire);
    }
    public ItemEquipment()
    {
        this.ID = -1;
    }
    public  void ClearData()
    {
        this.ClearData();
    }
}




