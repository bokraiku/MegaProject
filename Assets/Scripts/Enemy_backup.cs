using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_backup : MonoBehaviour {

    // Use this for initialization
    public GameObject damageSpawn;
    public Animator anim;
	void Start () {
        FloatingController.Initialize();
        damageSpawn = GameObject.Find("DamageSpawn");
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        //FloatingController.CreateFloatingText("1244", transform);
       
    }
    void OnMouseDown()
    {
        //Debug.Log("ok");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
    }

    public void TakeDamage(float damage)
    {
        anim.SetTrigger("Hit");
        FloatingController.CreateFloatingText(damage.ToString(), damageSpawn.transform);
    }
}
