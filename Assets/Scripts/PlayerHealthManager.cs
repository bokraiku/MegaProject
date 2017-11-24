using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using SocketIO;

public class PlayerHealthManager : NetworkBehaviour {

    public Image PlayerHealth;
    public GameObject damageSpawn;

    
    private float MaxHealth;

    [SyncVar(hook = "OnHealthChanged")]
    private float HealthAmount = 8000;

    [SyncVar(hook = "OnDamageHit")]
    private float DamageHit;

    public delegate void DieDelegate();
    public event DieDelegate EventDie;

    public delegate void RespawnDelegate();
    public event RespawnDelegate EventRespawn;
    private bool shouldDie = false;
    public bool isDead = false;



    public AudioClip[] m_audio;
    public AudioSource m_source;
    public Animator m_anim;
    //public bool isDead = false;

    public UIManager uiManager;
    private Text HealthText;

    private PlayerStatus playerStatus;


    public override void OnStartLocalPlayer()
    {
        m_source = GetComponent<AudioSource>();
        m_anim = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
        PlayerHealth = GameObject.FindWithTag("HealthBar").GetComponent<Image>();

        HealthText = GameObject.FindWithTag("HealthBar").gameObject.transform.Find("HP").GetComponent<Text>();
        //HealthAmount = playerStatus.HEALTH;
        SetHealthText();
    }

	
	// Update is called once per frame
	void Update () {
        //CheckCondition();
        //StartCoroutine(test());
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(5f);
        RpcRespawn();
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            // Set the spawn point to origin as a default value
            //wVector3 spawnPoint = Vector3.zero;

            // If there is a spawn point array and the array is not empty, pick one at random
            // Set the player’s position to the chosen spawn point
            transform.position = GameObject.Find("Spawn Points").transform.GetChild(0).transform.position;
        }
    }

    void CheckCondition()
    {
        if (HealthAmount <= 0 && !shouldDie && !isDead)
        {
            shouldDie = true;
        }

        if (HealthAmount <= 0 && shouldDie)
        {
            if (EventDie != null)
            {
                EventDie();
            }

            shouldDie = false;
        }

        if (HealthAmount > 0 && isDead)
        {
            if (EventRespawn != null)
            {
                EventRespawn();
            }

            isDead = false;
        }
    }

    [Client]
    public void CmdShowTextDamage(float Damage)
    {
        FloatingController.CreateFloatingText(Damage.ToString(), damageSpawn.transform);
    }

    public void TakeDamage(float Damage)
    {
        DamageHit = Damage;
        HealthAmount -= DamageHit;
        Debug.Log("Health Amount : " + HealthAmount);
        if (HealthAmount <= 0)
        {
            m_anim.SetTrigger("IsDeath");
            RpcRespawn();
        }
        //CmdShowTextDamage(Damage);
        //FloatingController.CreateFloatingText(Damage.ToString(), damageSpawn.transform);
        //if (isLocalPlayer)
        //{
        //    if (HealthAmount > 0)
        //    {
        //        FloatingController.CreateFloatingText(Damage.ToString(), damageSpawn.transform);
        //        //m_source.PlayOneShot(m_audio[0], 0.5f);
        //        //m_anim.SetTrigger("Impact");

        //        float fill = (1 / MaxHealth) * HealthAmount;
        //        PlayerHealth.fillAmount = fill;
        //        Debug.Log("Take Damage : " + Damage);
        //    }


        //    if (HealthAmount <= 0 && this.isDead == false)
        //    {
        //        this.isDead = true;
        //        Death();
        //    }
        //}

    }



    public void SetHealth(int health)
    {
        MaxHealth = health;
        HealthAmount = health;
        SetHealthText();
        /*
        if (isLocalPlayer)
        {
           
            //float fill = (1 / MaxHealth) * HealthAmount;
            //PlayerHealth.fillAmount = fill;
            //HealthText.text = HealthAmount.ToString();
            SetHealthText();
        }
        */

    }
    void SetHealthText() {
        if (isLocalPlayer)
        {
            float fill = (1 / MaxHealth) * HealthAmount;
            PlayerHealth.fillAmount = fill;
            HealthText.text = HealthAmount.ToString();
            Debug.Log("Max health : " + MaxHealth + "|Health Amount : " + HealthAmount);

        }
    }
    void OnHealthChanged(float hlth)
    {
        
        HealthAmount = hlth;
        SetHealthText();
        
       
    }



    void OnDamageHit(float damage)
    {
        DamageHit = damage;
        FloatingController.CreateFloatingText(DamageHit.ToString(), damageSpawn.transform);
        DamageHit = 0;
    }

    private void Death()
    {
        m_anim.SetTrigger("IsDeath");
        StartCoroutine("LoadNewState");
       
    }
    private IEnumerator LoadNewState()
    {
        yield return new WaitForSeconds(5f);
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (!uiManager.LoadingPanel.activeSelf)
        {
            uiManager.LoadingPanel.SetActive(true);
        }
        uiManager.loadingBar.gameObject.SetActive(true);
        StartCoroutine(uiManager.LoadLevelWithRealProcess(uiManager.lobby,uiManager.adv_1));

    }
}
