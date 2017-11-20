using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Net;
using UnityEngine.UI;
using UnityEngine.Networking;
using SocketIO;

public class PlayerStatus : NetworkBehaviour {
    private JsonData attr;
    private PlayerHealthManager playerHealth;

    private SocketIOComponent socket;
    /// <summary>
    ///  Status 
    /// </summary>
    public int STR { get; set; }
    public int VIT { get; set; }
    public int DEX { get; set; }
    public int INT { get; set; }
    public int HP { get; set; }
    public int AGI { get; set; }

    public int P_ATTACK { get; set; }
    public int M_ATTACK { get; set; }
    public int P_DEFENSE { get; set; }
    public int M_DEFENSE { get; set; }
    public int HEALTH { get; set; }

    public string ATTACK_TYPE  { get; set; }

    private void Start()
    {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        playerHealth  = gameObject.GetComponent<PlayerHealthManager>();

        socket.On("player_attr_received", GET_ATTR);
        StartCoroutine("WaitForData");
    }

    IEnumerator WaitForData()
    {

        //data = rq.GET();
        yield return new WaitForSeconds(1f);
        socket.Emit("get_player_attr");

    }

    public void INIT_ATTR()
    {
        this.P_ATTACK = 0;
        this.M_ATTACK = 0;
        this.P_DEFENSE = 0;
        this.M_DEFENSE = 0;
        this.HEALTH = 0;

        this.STR = 0;
        this.VIT = 0;
        this.DEX = 0;
        this.INT = 0;
        this.HP = 0;
        this.AGI = 0;
    }

    public void setAttr(int p_attack,int m_attack,int p_defense,int m_defense,int health)
    {
        this.P_ATTACK = p_attack;
        this.M_ATTACK = m_attack;
        this.P_DEFENSE = p_defense;
        this.M_DEFENSE = m_defense;
        this.HEALTH = health;
    }

    public void setStatus(int str,int vit,int dex,int _int,int hp,int agi)
    {
        this.STR = str;
        this.VIT = vit;
        this.DEX = dex;
        this.INT = _int;
        this.HP = hp;
        this.AGI = agi;
        this.Calculate_attr();
    }

    private void Calculate_attr()
    {
        this.P_ATTACK += this.STR * 3;
        this.M_ATTACK += this.INT * 4;
        this.P_DEFENSE += (this.STR * 1) + (this.VIT * 3);
        this.M_DEFENSE += this.INT * 2;
        this.HEALTH += (this.STR * 1) + (this.VIT * 5) + (this.HP * 100);
    }





    public void GET_ATTR(SocketIOEvent e)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Debug.Log("[SocketIO] Open received Attr: " + e.data.GetField("attr").ToString());

        attr = JsonMapper.ToObject(e.data.GetField("attr").ToString());

        if (attr.Count > 0)
        {
            this.setAttr((int)attr[0]["p_attack"], (int)attr[0]["m_attack"], (int)attr[0]["p_defense"], (int)attr[0]["m_defense"], (int)attr[0]["health"]);

            this.setStatus((int)attr[0]["str"], (int)attr[0]["vit"], (int)attr[0]["dex"], (int)attr[0]["_int"], (int)attr[0]["hp"], (int)attr[0]["agi"]);

            playerHealth.SetHealth(this.HEALTH);
            Debug.Log("Attack is : " + this.P_ATTACK);
            Debug.Log("Health is : " + this.HEALTH);
        }
    }


}
