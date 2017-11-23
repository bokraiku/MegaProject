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

    [SyncVar (hook = "OnHealthChange")]
    private float maxHp = 5000f;
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


    public Transform targetTransform;
    private NavMeshAgent agent;

    public Transform Player;
    private Vector3  originalPosition;

    private float currentDistance;
    private float minDistance = 2f;
    private float nextAttack;
    private float attackRate = 3f;
    private float maxDistanceToLeave = 18f;
    private float moveSpeed = 0f;

    int layerMask;
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
        raycastLayer = 1 << LayerMask.GetMask("Player");
        //raycastLayer = ~raycastLayer;
        //layerMask = 1 << 6;
        Debug.Log("Layer Start : " + raycastLayer.value);

        if (isServer)
        {
            StartCoroutine(DoCheck());
            
        }
       

    }

    private void FixedUpdate()
    {
        
        //SearchTarget();
        //MoveToTarget();

        //ReturnToOri();
        //CheckOri();
    }
    void ReturnToOri()
    {

        if (targetTransform != null)
        {
            float distance = Vector3.Distance(targetTransform.position,transform.position);

            if (distance >= maxDistanceToLeave)
            {
                //anim.SetBool("IsWalk", true);
                isReturn = true;
                targetTransform = null;
                //Debug.Log("Distnace : " + distance);
                agent.isStopped = true;
                agent.ResetPath();
                //Debug.Log("ori : " + originalPosition.position);
                //float step = speed * Time.deltaTime;
                //transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
                //agent.SetDestination(originalPosition);
                agent.destination = originalPosition;
                Debug.Log("REturn to origin 1");

            }
        }

        if (targetTransform == null && Vector3.Distance(transform.position, originalPosition) > 0.5f)
        {
            //anim.SetBool("IsWalk", true);
            isReturn = true;
            targetTransform = null;
            //Debug.Log("Distnace : " + distance);
            agent.isStopped = true;
            agent.ResetPath();
            //Debug.Log("ori : " + originalPosition.position);
            //float step = speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
            //agent.SetDestination(originalPosition);
            agent.destination = originalPosition;
            //Debug.Log("REturn to origin 2");
        }
    }
    void CheckOri()
    {


        float distance = Vector3.Distance(originalPosition,transform.position);

        if(distance <= 0.5f)
        {
           // Debug.Log("Arived to origin");
            //anim.SetBool("IsWalk", false);
            isReturn = false;
        }

        //if (!agent.pathPending && isReturn)
        //{
        //    if (agent.remainingDistance <= agent.stoppingDistance)
        //    {
                
        //        if (!agent.hasPath || agent.velocity.sqrMagnitude <= 2.5f)
        //        {
        //                Debug.Log("Arived to origin");
        //                anim.SetBool("IsWalk", false);
        //                isReturn = false;
        //        }
        //    }
        //}

    }
    void SearchTarget()
    {

        if (targetTransform == null)
        {
            Collider[] hitCollider = Physics.OverlapSphere(myTransform.position, radius, LayerMask.GetMask("Player"));
            if (hitCollider.Length > 0)
            {

                foreach (Collider c in hitCollider)
                {


                    if (c.transform.name == "HitPoint")
                    {
                        continue;
                    } else {

                        targetTransform = c.transform;
                        Debug.Log("Layer : " + c.name);
                        break;

                    }

                }
            }
            else
            {
                targetTransform = null;
            }

            if (targetTransform == null)
            {
                Debug.Log(transform.name + " target null");
            }
           
            //if(hitCollider.Length > 0)
            //{

            //    int randomRange = Random.Range(0, hitCollider.Length);
            //    Debug.Log("Target : " + hitCollider[randomRange].gameObject.name);
            //    targetTransform = hitCollider[randomRange].transform;
            //}
            //else
            //{
            //    targetTransform = null;
            //}

        }

        


    }

    void MoveToTarget()
    {
        if(targetTransform != null && isServer)
        {
            
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
    void PlayAnimationMove()
    {
        moveSpeed = agent.velocity.magnitude;
        anim.SetFloat("Forward", moveSpeed);
    }

    IEnumerator DoCheck()
    {
        for (;;)
        {
            SearchTarget();
            MoveToTarget();
            ReturnToOri();
            CheckOri();
            PlayAnimationMove();
            yield return new WaitForSeconds(0.2f);
        }
    }


    void OnHealthChange(float health)
    {
        amountHp = health;
        float fill = (1 / maxHp) * amountHp;
        HealthBar.fillAmount = fill;
    }

    [ClientRpc]
    void RpcMonsterShowDamage(float damage)
    {
        
        float fill = (1 / maxHp) * amountHp;
        HealthBar.fillAmount = fill;
        FloatingController.CreateFloatingText(damage.ToString(), damageSpawn.transform);
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
        //if(other.tag == "Player")
        //{
           
        //    //other.GetComponent<>s
        //    PlayerHealthManager PlayerObject = other.gameObject.GetComponent<PlayerHealthManager>();
        //    if (!PlayerObject.isDead)
        //    {
        //        PlayerObject.TakeDamage(Attack());
        //    }
        //    else
        //    {
        //        noTarget = true;
        //    }
            
        //}
    }

    public void TakeDamage(float damage)
    {
        //m_audio.PlayOneShot(m_clip[0], 0.5f);
        //GetComponent<NetworkAnimator>().SetTrigger("Hit");

        amountHp -= damage;
       
        //Debug.Log(fill);
        //FloatingController.CreateFloatingText(damage.ToString(), damageSpawn.transform);
        RpcMonsterShowDamage(damage);
        if (amountHp <= 0)
        {
            //GetComponent<NetworkAnimator>().SetTrigger("IsDeath");
            ////anim.SetTrigger("IsDeath");
            //gameObject.layer = 1;
            //gameObject.GetComponent<Collider>().isTrigger = true;
            //gameObject.GetComponent<Enemy>().enabled = false;
            //GameObject.Destroy(gameObject, 2f);
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

    //public void EnableMarker()
    //{
    //    this.AttackMarker.SetActive(true);
    //}
    //public void DiableMarker()
    //{
    //    this.AttackMarker.SetActive(false);
    //}
}
