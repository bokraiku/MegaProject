using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour {

    private GameObject PlayerObj;
    public GameObject NormalAttack;
    public GameObject Skill1;
    public GameObject Skill2;
    public GameObject Skill3;
    public GameObject Skill4;
    public static SkillManager Instance;

    public GameObject CoolDown1;
    public GameObject CoolDown2;
    public GameObject CoolDown3;
    public GameObject CoolDown4;


    // Use this for initialization
    void Start () {
        if (!Instance)
        {
            Instance = this;
        }
        //PlayerObj = GameObject.FindWithTag("Player");
        //Debug.Log("Player Object : " + PlayerObj);
        //NormalAttack.GetComponent<Button>().onClick.AddListener(PlayerObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().NormalAttack);
        //Skill1.GetComponent<Button>().onClick.AddListener(PlayerObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().Skill1);
        //Skill2.GetComponent<Button>().onClick.AddListener(PlayerObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().Skill2);
        //Skill3.GetComponent<Button>().onClick.AddListener(PlayerObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().Skill3);
        //Skill4.GetComponent<Button>().onClick.AddListener(PlayerObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>().Skill4);
    }
	

}
