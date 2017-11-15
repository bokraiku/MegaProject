using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    public Animator anim;
    private Text damageText;
	// Use this for initialization
	void OnEnable() {
        AnimatorClipInfo[] animInfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, animInfo[0].clip.length);

        damageText = anim.GetComponent<Text>();
	}
	

    public void setText(String text)
    {
        damageText.text = text;
    }

}
