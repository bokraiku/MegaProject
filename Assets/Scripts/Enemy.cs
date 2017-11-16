using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class Enemy : NetworkBehaviour {

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

    private LayerMask raycastLayer;
    private float radius = 20f;
    private float speed = 3f;

    private Transform myTransform;
    private Transform targetTransform;
    private NavMeshAgent agent;

    public Transform Player;
    private Vector3  originalPosition;

    private bool isReturn = false;
    private void Awake()
    {
        originalPosition = new Vector3(transform.position.x,transform.position.y, transform.position.z);
    }
    void Start () {
        
        FloatingController.Initialize();
        //damageSpawn = GameObject.Find("DamageSpawn");
        anim = GetComponent<Animator>();
        anim.SetBool("IsWalk", false);
        m_audio = GetComponent<AudioSource>();
        amountHp = this.maxHp;

        agent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        raycastLayer = 1<<LayerMask.NameToLayer("Player");

        


    }

    private void FixedUpdate()
    {
        
        SearchTarget();
        MoveToTarget();

        ReturnToOri();
        //CheckOri();
    }
    void ReturnToOri()
    {
        if (targetTransform != null)
        {
            float distance = Vector3.Distance(transform.position, targetTransform.position);

            if (distance >= 18f)
            {
                isReturn = true;
                targetTransform = null;
                //Debug.Log("Distnace : " + distance);
                agent.isStopped = true;
                agent.ResetPath();
                //Debug.Log("ori : " + originalPosition.position);
                float step = speed * Time.deltaTime;
                //transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
                //agent.SetDestination(originalPosition);
                agent.destination = originalPosition;

            }
        }
    }
    void CheckOri()
    {
        if (isReturn)
        {
            float distance = Vector3.Distance(transform.position, originalPosition);
            Debug.Log("TO origin : " + distance);
            if (distance <= 0.5f)
            {
                isReturn = false;
                agent.isStopped = true;
                agent.ResetPath();
                anim.SetBool("IsWalk", false);

            }
        }

    }
    void SearchTarget()
    {
        if (!isServer)
        {
            return;
        }
        if(targetTransform == null)
        {
            Collider[] hitCollider = Physics.OverlapSphere(myTransform.position, radius, raycastLayer);
            if(hitCollider.Length > 0)
            {
                Debug.Log("Target : " + hitCollider[0].gameObject.name);
                int randomRange = Random.Range(0, hitCollider.Length - 1);
                targetTransform = hitCollider[randomRange].transform;
            }
            else
            {
                targetTransform = null;
            }
            
        }

        


    }

    void MoveToTarget()
    {
        if(targetTransform != null && isServer)
        {
            anim.SetBool("IsWalk", true);
            //Debug.Log("Monster Moving ");
            SetNavDestination(targetTransform);
        }
        //else
        //{
        //    Debug.Log("No Target");
        //    agent.isStopped = true;
        //    agent.ResetPath();
        //}
    }
    void SetNavDestination(Transform dest)
    {
        agent.SetDestination(dest.position);
    }

    // Update is called once per frame
    void Update () {
        
        
        //this.ManageAttack();
        ////FloatingController.CreateFloatingText("1244", transform);
        //Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //if (Player != null && Vector3.Distance(Player.position,this.transform.position) < 10 && noTarget == false)
        //{
        //    Vector3 direction = Player.position - this.transform.position;
        //    direction.y = 0;
        //    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        //    if(direction.magnitude > 2f)
        //    {
  
        //        anim.SetBool("IsWalk", true);
        //        this.transform.Translate(0, 0, 0.05f);
        //    }
        //    else
        //    {
        //        if (attackTime == 0)
        //        {
        //            GetComponent<NetworkAnimator>().SetTrigger("IsAttack");
        //            attackTime = attackCooldown;
        //        }
               
        //        anim.SetBool("IsWalk", false);
        //    }
        //}
        //else
        //{
        //    //anim.SetBool("IsAttack", false);
        //    anim.SetBool("IsWalk", false);
        //}
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
        GetComponent<NetworkAnimator>().SetTrigger("Hit");

        amountHp -= damage;
        float fill = (1 / maxHp) * amountHp;
        HealthBar.fillAmount = fill;
        Debug.Log(fill);
        FloatingController.CreateFloatingText(damage.ToString(), damageSpawn.transform);
        if(amountHp <= 0)
        {
            GetComponent<NetworkAnimator>().SetTrigger("IsDeath");
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

        //m_audio.PlayOneShot(m_clip[1], maxvol);

    }
    public void WalkRighttStep()
    {
        AudioClip cp = null;
        float maxvol = 0.01f;
        cp = m_clip[1];

        //m_audio.PlayOneShot(m_clip[1], maxvol);

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
