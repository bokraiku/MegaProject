using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PlayerHealthManager : NetworkBehaviour {

    public Image PlayerHealth;
    public GameObject damageSpawn;

    [SyncVar]
    private float MaxHealth;

    [SyncVar(hook = "OnHealthChanged")]
    private float HealthAmount;

    [SyncVar(hook = "OnDamageHit")]
    private float DamageHit;

    public PlayerStatus playerStatus;

    public AudioClip[] m_audio;
    public AudioSource m_source;
    public Animator m_anim;
    public bool isDead = false;

    public UIManager uiManager;

    

	// Use this for initialization
	void Start () {
        m_source = GetComponent<AudioSource>();
        m_anim = GetComponent<Animator>();

        PlayerHealth = GameObject.FindWithTag("HealthBar").GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Health : "  +this.MaxHealth);
	}
    public void SetHealth(int health)
    {
        if (isLocalPlayer)
        {
            this.MaxHealth = health;
            this.HealthAmount = this.MaxHealth;
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
        Debug.Log("Take Damage : " + DamageHit);
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
    void SetHealthText() {
        if (isLocalPlayer)
        {

            float fill = (1 / MaxHealth) * HealthAmount;
            PlayerHealth.fillAmount = fill;
            
        }
    }
    void OnHealthChanged(float hlth)
    {
        
        HealthAmount = hlth;
        SetHealthText();
        FloatingController.CreateFloatingText(DamageHit.ToString(), damageSpawn.transform);
        DamageHit = 0;
    }
    void OnDamageHit(float damage)
    {
        DamageHit = damage;
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
