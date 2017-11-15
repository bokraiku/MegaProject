using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour {


    public Animator anim;

    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();
        StartCoroutine(PlayIdleAnim());
    }


    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("1"))
        {
            anim.Play("WAIT01", -1, 0f);
        }
    }
        

    private  IEnumerator PlayIdleAnim()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            float rand = Mathf.Round(Random.Range(0f, 4f));
            string str_anim = "WAIT0" + rand.ToString();
            anim.Play(str_anim);
        }

    }
}
