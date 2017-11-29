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
    private float minDistance = 2.5f;
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
            if(currentDistance > minDistance && currentDistance < (minDistance + 0.5))
            {
                myTransform.LookAt(targetScript.targetTransform);
                //myTransform.position = Vector3.MoveTowards(myTransform.position, targetScript.targetTransform.position, 0f);
                
            }
            if (currentDistance <= minDistance && Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
                agent.isStopped = true;
                agent.ResetPath();

                Debug.Log("Target MOnster: " + targetScript.targetTransform.transform.name);
                myTransform.LookAt(targetScript.targetTransform);


                //RpcMonsterPlayAnimation();
                //MonsterAttack();
                if(targetScript.targetTransform.transform.GetComponent<PlayerHealthManager>().isDead == false)
                {
                    MonsterAttack(); // for host player
                    RpcLunchAttack();
                }

               
                //myTransform.position = Vector3.MoveTowards(myTransform.position, targetScript.targetTransform.position, 0.2f);
                //agent.destination = targetScript.targetTransform.position;
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

        float damage = Mathf.Floor(Random.Range(2000, 5000));
        return damage;
    }

    public void EnableMarker()
    {
        this.AttackMarker.SetActive(true);
        //Collider col = AttackMarker.GetComponent<Collider>();
        if (targetScript.targetTransform != null && targetScript.targetTransform.transform.GetComponent<PlayerHealthManager>().isDead == false)
        {
            targetScript.targetTransform.transform.GetComponent<PlayerHealthManager>().TakeDamage(this.Attack());
            
        }
    }
    public void DiableMarker()
    {
        this.AttackMarker.SetActive(false);
    }

   

}
