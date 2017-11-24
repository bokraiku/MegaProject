using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using System.Text.RegularExpressions;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    
    [RequireComponent(typeof (ThirdPersonCharacter))]
   
    public class ThirdPersonUserControl : NetworkBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        private GameObject CameraBase;

        private Camera CameraFollower = null;
        private float CameraHight;
        private Vector3 CameraVerocity = Vector3.zero;
        private float CameraDampTime = 0.15f;
        private float CameraSpeed = 2f;

        private string bound_pattern = "(_bound)$";
        private string warp_pattern = "(spawn_point)$";
        private GameObject SkillPanel;



        //public VisualJoyStick jStick;
        public VisualJoyStick jStick;

        private void AttachedCamera()
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                CameraFollower = camera;
                CameraHight = CameraFollower.transform.position.y;
            }
        }

        public void DetachCamera()
        {
            if (CameraFollower != null)
            {
                CameraFollower = null;
            }
        }

        void Awake()
        {
            SkillPanel = GameObject.FindWithTag("SkillPanel");
            //CameraBase = GameObject.FindWithTag("CameraBase");
            //CameraBase.SetActive(false);
        }
        private void Start()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            if (isLocalPlayer)
            {
                this.AttachedCamera();
            }


            //CameraBase.SetActive(true);

            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();

            jStick = GameObject.FindWithTag("VSJoyStick").GetComponent<VisualJoyStick>();
           

        }


        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }


            if(GameManagement.instane != null)
            {
                GameManagement.instane.DeActivateZone();
            }


            //if (!m_Jump)
            //{
            //    m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            //    //Debug.Log("Jump" + m_Jump);
            //}
        }

        void OnTriggerEnter(Collider other)
        {
            
            if(other != null)
            {
                //Debug.Log("Current Game Object  " + other.gameObject.name);
                GameObject currentGameobject = other.gameObject;
                Regex objectMatch = new Regex(this.bound_pattern);
                Regex warpMatch = new Regex(this.warp_pattern);
                if (objectMatch.IsMatch(currentGameobject.name.ToString()) &&  GameManagement.instane != null)
                {
                    if (isLocalPlayer)
                    {
                        GameManagement.instane.LocalPlayerCurrentZone = currentGameobject;
                        if (currentGameobject.name != "g_z1_bound")
                        {
                            SkillPanel.SetActive(true);
                        }
                        else
                        {
                            SkillPanel.SetActive(false);
                        }
                    }
                    
                    //Debug.Log("Current Game Object  " + currentGameobject.name);
                }

                

                if (warpMatch.IsMatch(currentGameobject.name.ToString()) && GameManagement.instane != null)
                {
                    
                    GameObject WarpObject =  GameManagement.instane.ManageWarp(currentGameobject.name);
                    Debug.Log("Current Warp :  " + WarpObject);
                    if (WarpObject != null)
                    {
                        this.transform.position = WarpObject.transform.position;
                    }
                }

            }

        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            

            if(CameraFollower != null)
            {
                Vector3 position = CameraFollower.transform.position;
                position = Vector3.SmoothDamp(position, transform.position,ref CameraVerocity,CameraDampTime,Mathf.Infinity,Time.fixedDeltaTime * CameraSpeed);
                position.y = CameraHight;
                position.z = transform.position.z - 6;
                CameraFollower.transform.position = position;
            }
            // read inputs
            //float h = CrossPlatformInputManager.GetAxis("Horizontal");
            //float v = CrossPlatformInputManager.GetAxis("Vertical");

            float h = jStick.Horizontal();
            float v = jStick.Vertical();

            //Debug.Log("Debug H :" + h);
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;

                m_Move = v*m_CamForward + h*m_Cam.right;
                //m_Move = v*Vector3.forward + h*Vector3.right;
            
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif
            //Debug.Log(m_skill.is_attack);

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;


        }
    }
}
