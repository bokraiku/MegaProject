using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour {


    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    Vector3 FloowPOS;
    public float clamAngle = 30.0f;
    public float InputSensitivity = 100.0f;

    public GameObject CameraObj;
    public GameObject PlayerObj;

    public float camDistanceXtoPlayer;
    public float camDistanceYtoPlayer;
    public float camDistanceZtoPlayer;

    public float mouseX;
    public float mouseY;

    public float finalInputX;
    public float finalInputZ;
    public float smoootX;
    public float smoootY;
    private float rotX = 0.0f;
    private float rotY = 0.0f;

    

    private GameObject Player;
    // Use this for initialization
    void Start () {
        Vector3 rot = transform.localRotation.eulerAngles;
        
        rotX = rot.x;
        rotY = rot.y;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        //float inputX = Input.GetAxis("RightStickHorizontal");
        //float inputZ = Input.GetAxis("RightStickVertical");

        //Debug.Log("Touch : "  + Input.touchCount);
        //mouseX = Input.GetAxis("Mouse X");
        //mouseY = Input.GetAxis("Mouse Y");

        //finalInputX = inputX + mouseX;
        //finalInputZ = inputZ + mouseY;

        //rotY += finalInputX * InputSensitivity * Time.deltaTime;
        //rotX += finalInputZ * InputSensitivity * Time.deltaTime;

        //rotX = Mathf.Clamp(rotX, -clamAngle, clamAngle);

        //Quaternion localRotation = Quaternion.Euler(rotX,rotY,0.0f);
        //transform.rotation = localRotation;
    }
    private void OnMouseDrag()
    {
        
    }

    private void LateUpdate()
    {
       
        CameraUpdated();
    }

    void CameraUpdated()
    {
        Vector3 CamPos = new Vector3(CameraFollowObj.transform.position.x, CameraFollowObj.transform.position.y, 0);
        
            //float step = CameraMoveSpeed * Time.deltaTime;
            transform.position = new Vector3(CameraFollowObj.transform.position.x, CameraFollowObj.transform.position.y, CameraFollowObj.transform.position.z - 2);

        //transform.position = CameraFollowObj.transform.position;
    }


}
