using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

    public float minDistance = 1.0f;
    public float maxDistance = 7.0f;

    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjust;
    public float distance;
    public Transform PlayerTarget;

    private float x = 0;
    private float y = 0;
    private Quaternion oriRotation;
    // Use this for initialization
    void Awake () {
        //dollyDir = transform.localPosition.normalized;
        //distance = transform.localPosition.magnitude;
    }

    void Start()
    {
        oriRotation = transform.rotation;
        var angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;
    }
    void LateUpdate()
    {
        //y = PlayerTarget.eulerAngles.y;
        //Quaternion rotation = Quaternion.Euler(x, y, 0);

        //Vector3 position = PlayerTarget.position - (rotation * Vector3.forward * distance + new Vector3(0, -2, 0));
        //transform.position = position;
        //transform.position = PlayerTarget.position;
        //transform.Translate(0, 5,- 10);
        //transform.rotation = oriRotation;
        //transform.LookAt(PlayerTarget);
    }
	
	// Update is called once per frame
	void Update () {
        //Vector3 disredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        //RaycastHit hits;

        //if(Physics.Linecast(transform.parent.position, disredCameraPos,out hits))
        //{
        //    Debug.Log(hits.transform.gameObject.name);
        //    distance = Mathf.Clamp((hits.distance * 0.87f), minDistance, maxDistance);
        //}
        //else
        //{
        //    distance = maxDistance;
        //}

        //transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
        
	}
}
