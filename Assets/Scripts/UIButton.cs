using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIButton : MonoBehaviour {

    public Image mainMenuImage;
    public Animator amin;
	// Use this for initialization
	void Start () {
        //mainMenuImage = transform.GetChild(0).GetComponent<Image>();
        amin = GetComponent<Animator>();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MainMenuClick()
    {
        int isOpen = (amin.GetInteger("IsOpen")==1)?0:1;
        amin.SetInteger("IsOpen", isOpen);
        //Debug.Log(isOpen);
        //float rotation_z = (mainMenuImage.transform.rotation.z == 0) ? 180 : 0;
        //Debug.Log(rotation_z);
        //mainMenuImage.transform.rotation = Quaternion.Euler(0, 0, rotation_z);
        
    }
}
