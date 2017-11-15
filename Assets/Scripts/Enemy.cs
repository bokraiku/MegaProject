using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour {

    // Use this for initialization
    public GameObject damageSpawn;
    public Animator anim;

    public AudioClip[] m_clip;
    public AudioSource m_audio;

    public Image HealthBar;

    private float maxHp = 50000f;
    private float amountHp;

    private bool noTarget = false;

    float m_ForwardAmount;

    public GameObject AttackMarker;
    private float attackCooldown = 3f;
    private float attackTime = 0f;


    public Transform Player;
	void Start () {
        FloatingController.Initialize();
        //damageSpawn = GameObject.Find("DamageSpawn");
        anim = GetComponent<Animator>();
        anim.SetBool("IsWalk", false);
        m_audio = GetComponent<AudioSource>();
        amountHp = this.maxHp;

        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();


    }
	
	// Update is called once per frame
	void Update () {
        this.ManageAttack();
        //FloatingController.CreateFloatingText("1244", transform);

        if (Player != null && Vector3.Distance(Player.position,this.transform.position) < 10 && noTarget == false)
        {
            Vector3 direction = Player.position - this.transform.position;
            direction.y = 0;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            if(direction.magnitude > 2f)
            {
  
                anim.SetBool("IsWalk", true);
                this.transform.Translate(0, 0, 0.05f);
            }
            else
            {
                if (attackTime == 0)
                {
                    anim.SetTrigger("IsAttack");
                    attackTime = attackCooldown;
                }
               
                anim.SetBool("IsWalk", false);
            }
        }
        else
        {
            //anim.SetBool("IsAttack", false);
            anim.SetBool("IsWalk", false);
        }
    }
    void OnMouseDown()
    {
        //Debug.Log("ok");
        
    }
    void ManageAttack()
    {
        
        if(attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
        if(attackTime < 0)
        {
            attackTime = 0;
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
           
            //other.GetComponent<>s
            PlayerHealthManager PlayerObject = other.gameObject.GetComponent<PlayerHealthManager>();
            if (!PlayerObject.isDead)
            {
                PlayerObject.TakeDamage(Attack());
            }
            else
            {
                noTarget = true;
            }
            
        }
    }

    public void TakeDamage(float damage)
    {
        m_audio.PlayOneShot(m_clip[0], 0.5f);
        anim.SetTrigger("Hit");

        amountHp -= damage;
        float fill = (1 / maxHp) * amountHp;
        HealthBar.fillAmount = fill;
        Debug.Log(fill);
        FloatingController.CreateFloatingText(damage.ToString(), damageSpawn.transform);
        if(amountHp <= 0)
        {
            anim.SetTrigger("IsDeath");
            gameObject.layer = 1;
            gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.GetComponent<Enemy>().enabled = false;
            GameObject.Destroy(gameObject, 2f);
        }
    }
    public void WalkLeftStep()
    {

        AudioClip cp = null;
        float maxvol = 0.01f;
        cp = m_clip[1];

        m_audio.PlayOneShot(m_clip[1], maxvol);

    }
    public void WalkRighttStep()
    {
        AudioClip cp = null;
        float maxvol = 0.01f;
        cp = m_clip[1];

        m_audio.PlayOneShot(m_clip[1], maxvol);

    }

    public float Attack()
    {

        float damage = Mathf.Floor(Random.Range(200, 500));
        return damage;
    }

    public void EnableMarker()
    {
        this.AttackMarker.SetActive(true);
    }
    public void DiableMarker()
    {
        this.AttackMarker.SetActive(false);
    }
}
