using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Monster_Attack : NetworkBehaviour
{


    private float currentDistance;
    private float minDistance = 2f;
    private float nextAttack;
    private float attackRate = 3f;
    private Transform myTransform;
    private Enemy targetScript;

    // Use this for initialization
    void Start () {
        myTransform = transform;
        targetScript = GetComponent<Enemy>();

        if (isServer)
        {
            StartCoroutine(IAttack());
        }
	}

    void CheckIfTargetInRanger()
    {
        if (targetScript.targetTransform != null)
        {
            currentDistance = Vector3.Distance(targetScript.targetTransform.position , myTransform.position);
            if (currentDistance < minDistance && Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;
                GetComponent<NetworkAnimator>().SetTrigger("IsAttack");
                //Debug.Log("Target : " + targetScript.transform.name);
                targetScript.transform.GetComponent<PlayerHealthManager>().TakeDamage(this.Attack());
            }
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
    public float Attack()
    {

        float damage = Mathf.Floor(Random.Range(200, 500));
        return damage;
    }
}
