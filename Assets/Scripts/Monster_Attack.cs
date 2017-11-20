using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;
public class Monster_Attack : NetworkBehaviour
{


    private float currentDistance;
    private float minDistance = 2f;
    private float nextAttack;
    private float attackRate = 3f;
    private Transform myTransform;
    private Enemy targetScript;
    private Animator anim;
    private NavMeshAgent agent;

    // Use this for initialization
    void Start () {
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
            if (currentDistance < minDistance && Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
                agent.isStopped = true;
                agent.ResetPath();
                //anim.SetBool("IsWalk", false);
                //anim.SetTrigger("IsAttack");

                //GetComponent<NetworkAnimator>().SetTrigger("IsAttack");
                Debug.Log("Target MOnster: " + targetScript.targetTransform.transform.name);
                
                targetScript.targetTransform.transform.GetComponent<PlayerHealthManager>().TakeDamage(this.Attack());
                //MonsterAttack();
                RpcMonsterPlayAnimation();
            }
        }
    }
    void MonsterAttack()
    {
        transform.LookAt(targetScript.targetTransform.transform);
        anim.SetTrigger("IsAttack");

    }

    [ClientRpc]
    void RpcMonsterPlayAnimation()
    {
        MonsterAttack();
    }
    IEnumerator IAttack()
    {
        for (;;)
        {
            yield return new WaitForSeconds(0.2f);
            CheckIfTargetInRanger();
        }
    }
    public float Attack()
    {

        float damage = Mathf.Floor(Random.Range(10, 50));
        return damage;
    }
}
