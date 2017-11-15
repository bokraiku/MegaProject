using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SkillClick : MonoBehaviour {


    public Animator playerAnim;
    
    public bool is_attack;
    Animator anim;
	// Use this for initialization
	void Start () {
        this.is_attack = false;
        //playerAnim = GetComponent<Animator>();
        //Debug.Log("Start");
	}

    void Update()
    {
        if (this.AnimatorIsPlaying(this.playerAnim) && playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
            this.is_attack = false;
        }
    }
	

    public void SkillAttackClick()
    {
        Debug.Log("Click");
        //playerAnim.ResetTrigger("PunchTrigger");
        this.is_attack = true;
        playerAnim.SetTrigger("PunchTrigger");

    }

    private bool AnimatorIsPlaying(Animator anim)
    {
        return anim.GetCurrentAnimatorStateInfo(0).length >
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
