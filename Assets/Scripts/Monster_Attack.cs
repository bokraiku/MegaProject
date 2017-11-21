using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;
public class Monster_Attack : NetworkBehaviour
{
    [SerializeField]
    private GameObject AttackMarker;

    private float currentDistance;
    private float minDistance = 2.3f;
    private float nextAttack;
    private float attackRate = 3f;
    private Transform myTransform;
    private Enemy targetScript;
    private Animator anim;
    private NavMeshAgent agent;
    private bool is_attack = false;

    // Use this for initialization
    void Start () {
        DiableMarker();
        myTransform = transform;
        targetScript = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        if (isServer)
        {
            StartCoroutine(IAttack());
        }
        //StartCoroutine(IAttack());
    }

    void CheckIfTargetInRanger()
    {
        if (targetScript.targetTransform != null)
        {
            currentDistance = Vector3.Distance(targetScript.targetTransform.position , myTransform.position);

            Debug.Log("Distance : " + (currentDistance < minDistance) + "|Value : " + currentDistance);
            Debug.Log("Time :" + (Time.time > nextAttack));
            if (currentDistance < minDistance && Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
                agent.isStopped = true;
                agent.ResetPath();
                //anim.SetBool("IsWalk", false);
                //anim.SetTrigger("IsAttack");

                //GetComponent<NetworkAnimator>().SetTrigger("IsAttack");
                Debug.Log("Target MOnster: " + targetScript.targetTransform.transform.name);


                //MonsterAttack(); // for host player
                //RpcMonsterPlayAnimation();
                //MonsterAttack();
                RpcLunchAttack();
            }

        }
    }


    void MonsterAttack()
    {
        anim.SetTrigger("IsAttack");
    }

    IEnumerator CheckAttack()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.2f);
            CheckIfTargetInRanger();
        }
    }
    IEnumerator IAttack()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.2f);
            CheckIfTargetInRanger();
        }
    }
    [ClientRpc]
    private void RpcLunchAttack()
    {
        MonsterAttack();

    }

    public float Attack()
    {

        float damage = Mathf.Floor(Random.Range(10, 50));
        return damage;
    }

    public void EnableMarker()
    {
        this.AttackMarker.SetActive(true);
        //Collider col = AttackMarker.GetComponent<Collider>();
        if (targetScript.targetTransform != null)
        {
            targetScript.targetTransform.transform.GetComponent<PlayerHealthManager>().TakeDamage(this.Attack());
            
        }
    }
    public void DiableMarker()
    {
        this.AttackMarker.SetActive(false);
    }

   

}
