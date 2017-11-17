using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using UnityEngine.Networking;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class ThirdPersonCharacter : NetworkBehaviour
    {
        [SerializeField] float m_MovingTurnSpeed = 180;
        [SerializeField] float m_StationaryTurnSpeed = 360;
        [SerializeField] float m_JumpPower = 12f;
        [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
        [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField] float m_MoveSpeedMultiplier = 1f;
        [SerializeField] float m_AnimSpeedMultiplier = 1f;
        [SerializeField] float m_GroundCheckDistance = 0.1f;
        [SerializeField]
        public bool is_attack = false;
        [SerializeField] GameObject hit;

        public GameObject[] cooldown;

        public AudioSource m_audio;
        public AudioClip m_audioCliip;
        public AudioClip swordHit;

        public PlayerStatus playerStatus;



        private float normalAttackCooldown = 1f;
        private float skill1Cooldown = 2f;
        private float skill2Cooldown = 1.5f;
        private float skill3Cooldown = 3f;
        private float skill4Cooldown = 3.2f;

        private float[] skillCoodownArr = {2f,1.5f,3f,3.2f};
        


        private float normalTimer;
        private float skill1Timer;
        private float skill2Timer;
        private float skill3Timer;
        private float skill4Timer;

        public GameObject Marker;

        public Collider attackHitBox;
        private ParticleSystem ps;
        public GameObject objPs;


        Rigidbody m_Rigidbody;
		Animator m_Animator;
		bool m_IsGrounded;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		bool m_Crouching;

        private VisualJoyStick jStick;


        void Start()
		{
            playerStatus = gameObject.GetComponent<PlayerStatus>();
            m_audio = GetComponent<AudioSource>();
            //ps = objPs.GetComponent<ParticleSystem>();
            m_Animator = GetComponent<Animator>();
            
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;

			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;

            if(SkillManager.Instance != null)
            {
                cooldown[0] = SkillManager.Instance.CoolDown1;
                cooldown[1] = SkillManager.Instance.CoolDown2;
                cooldown[2] = SkillManager.Instance.CoolDown3;
                cooldown[3] = SkillManager.Instance.CoolDown4;

                // Set Player SKill
                SkillManager.Instance.NormalAttack.GetComponent<Button>().onClick.AddListener(this.NormalAttack);
                SkillManager.Instance.Skill1.GetComponent<Button>().onClick.AddListener(this.Skill1);
                SkillManager.Instance.Skill2.GetComponent<Button>().onClick.AddListener(this.Skill2);
                SkillManager.Instance.Skill3.GetComponent<Button>().onClick.AddListener(this.Skill3);
                SkillManager.Instance.Skill4.GetComponent<Button>().onClick.AddListener(this.Skill4);
            }
 

        }
        void Update()
        {
            

        }
        
        private void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Slash") && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Skill1") &&
                !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Skill2") && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Skill3") &&
                !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Skill4")
                )
            {
                //Debug.Log("IDLE or Finished");
                this.is_attack = false;
                //this.hit.SetActive(false);
            }
            else
            {
                this.is_attack = true;
                //Debug.Log("Skill Launch");
            }


            this.ManageCoolDown();


            // manage cooldown
            this.CooldownManage();
        }

        public void LeftStep()
        {
            AudioClip cp = null;
            float maxvol = 1.0f;
            maxvol = Random.Range(0.2f, 0.3f);
            cp = m_audioCliip;
            m_audio.PlayOneShot(cp, maxvol);

        }
        public void RightStep()
        {
            AudioClip cp = null;
            float maxvol = 1.0f;
            cp = m_audioCliip;
            maxvol = Random.Range(0.3f, 0.4f);
            m_audio.PlayOneShot(m_audioCliip, maxvol);
        }

        public void WalkLeftStep()
        {
            
            AudioClip cp = null;
            float maxvol = 0.01f;
            cp = m_audioCliip;
            
            m_audio.PlayOneShot(m_audioCliip, maxvol);

        }
        public void WalkRighttStep()
        {
            AudioClip cp = null;
            float maxvol = 0.01f;
            cp = m_audioCliip;
            
            m_audio.PlayOneShot(m_audioCliip, maxvol);

        }


        public void Move(Vector3 move, bool crouch, bool jump)
		{

            if (this.is_attack == false)
            {
                // convert the world relative moveInput vector into a local-relative
                // turn amount and forward amount required to head in the desired
                // direction.
                if (move.magnitude > 1f) move.Normalize();
                move = transform.InverseTransformDirection(move);
                CheckGroundStatus();
                move = Vector3.ProjectOnPlane(move, m_GroundNormal);
                m_TurnAmount = Mathf.Atan2(move.x, move.z);
                m_ForwardAmount = move.z;

                ApplyExtraTurnRotation();

                // control and velocity handling is different when grounded and airborne:
                if (m_IsGrounded)
                {
                    HandleGroundedMovement(crouch, jump);

                }
                else
                {
                    HandleAirborneMovement();
                }

                ScaleCapsuleForCrouching(crouch);
                PreventStandingInLowHeadroom();

                // send input and other state parameters to the animator
                UpdateAnimator(move);
            }
		}


		void ScaleCapsuleForCrouching(bool crouch)
		{
			if (m_IsGrounded && crouch)
			{
				if (m_Crouching) return;
				m_Capsule.height = m_Capsule.height / 2f;
				m_Capsule.center = m_Capsule.center / 2f;
				m_Crouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
					return;
				}
				m_Capsule.height = m_CapsuleHeight;
				m_Capsule.center = m_CapsuleCenter;
				m_Crouching = false;
			}
		}

		void PreventStandingInLowHeadroom()
		{
			// prevent standing up in crouch-only zones
			if (!m_Crouching)
			{
				Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
				float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
				if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					m_Crouching = true;
				}
			}
		}


		void UpdateAnimator(Vector3 move)
		{
            
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool("OnGround", m_IsGrounded);
			if (!m_IsGrounded)
			{
				m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			}

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			if (m_IsGrounded)
			{
				m_Animator.SetFloat("JumpLeg", jumpLeg);
			}

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (m_IsGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
                if (m_audio.isPlaying == false)
                {
                    m_audio.volume = Random.Range(0.8f, 1f);
                    m_audio.pitch = Random.Range(0.8f, 1.1f);
                    m_audio.Play();
                }
            }
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}


		void HandleGroundedMovement(bool crouch, bool jump)
		{
            // check whether conditions are right to allow a jump:
            
            if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
                // jump!
                
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
				m_IsGrounded = false;
				m_Animator.applyRootMotion = false;
				m_GroundCheckDistance = 0.1f;
			}
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
           
		}


		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (m_IsGrounded && Time.deltaTime > 0)
			{
                //Move motionZ
                Vector3 moveForward = transform.forward * m_Animator.GetFloat("motionZ") * Time.deltaTime;

				Vector3 v = ((m_Animator.deltaPosition + moveForward) * m_MoveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = v;
			}
		}


		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				m_Animator.applyRootMotion = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}
		}

        // for skill
        public void NormalAttack()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            //playerAnim.ResetTrigger("PunchTrigger");
            if(this.normalTimer == 0 && this.is_attack == false)
            {
                this.is_attack = true;
                
                GetComponent<NetworkAnimator>().SetTrigger("PunchTrigger");
                this.normalTimer = this.normalAttackCooldown;
            }


        }
        // for skill
        public void Skill1()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            //playerAnim.ResetTrigger("PunchTrigger");
            if (this.skill1Timer == 0 && this.is_attack == false)
            {
                setSkillCoolDown(0);
               
                this.is_attack = true;
                
                GetComponent<NetworkAnimator>().SetTrigger("Skill1");
                this.skill1Timer = this.skill1Cooldown;

            }


        }

        public void Skill2()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            //playerAnim.ResetTrigger("PunchTrigger");
            if (this.skill2Timer == 0 && this.is_attack == false)
            {
                setSkillCoolDown(1);
                this.is_attack = true;
                GetComponent<NetworkAnimator>().SetTrigger("Skill2");
                this.skill2Timer = this.skill2Cooldown;

            }


        }
        public void Skill3()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            //playerAnim.ResetTrigger("PunchTrigger");
            if (this.skill3Timer == 0 && this.is_attack == false)
            {

                setSkillCoolDown(2);
                this.is_attack = true;
                //this.hit.SetActive(true);
                GetComponent<NetworkAnimator>().SetTrigger("Skill3");
                this.skill3Timer = this.skill3Cooldown;
                
            }


        }
        public void Skill4()
        {
            if (!isLocalPlayer)
            {
                return;
            }
            //playerAnim.ResetTrigger("PunchTrigger");
            if (this.skill4Timer == 0 && this.is_attack == false)
            {

                setSkillCoolDown(3);
                this.is_attack = true;
                GetComponent<NetworkAnimator>().SetTrigger("Skill4");
                this.skill4Timer = this.skill4Cooldown;

            }


        }

        private bool AnimatorIsPlaying()
        {
            return m_Animator.GetCurrentAnimatorStateInfo(0).length >
                   m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }


        private void LunchAttack(Collider col)
        {
            Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("HitBox"));
            //Debug.Log(cols.Length);
            
            foreach(Collider c in cols){
                if (c.transform.root == transform)
                {
                    continue;
                }
                //Debug.Log(c.name);
                string uniqueID = c.transform.name;
                
                float damage = Mathf.Floor(Random.Range(playerStatus.P_ATTACK, playerStatus.P_ATTACK));
                CmdTellServerMonsterWasAttack(uniqueID, damage);
                //c.transform.GetComponent<Enemy>().TakeDamage(damate);
                //c.SendMessageUpwards("TakeDamage", damate);
                
            }
        }


        private void CooldownManage()
        {
            if(this.normalTimer > 0)
            {
                this.normalTimer -= Time.deltaTime;
            }
            if(this.normalTimer < 0)
            {
                this.normalTimer = 0;
            }

            if (this.skill1Timer > 0)
            {
                this.skill1Timer -= Time.deltaTime;
            }
            if (this.skill1Timer < 0)
            {
                
                this.skill1Timer = 0;
            }
            if (this.skill2Timer > 0)
            {
                this.skill2Timer -= Time.deltaTime;
            }
            if (this.skill2Timer < 0)
            {

                this.skill2Timer = 0;
            }
            if (this.skill3Timer > 0)
            {
                this.skill3Timer -= Time.deltaTime;
            }
            if (this.skill3Timer < 0)
            {

                this.skill3Timer = 0;
            }
            if (this.skill4Timer > 0)
            {
                this.skill4Timer -= Time.deltaTime;
            }
            if (this.skill4Timer < 0)
            {

                this.skill4Timer = 0;
            }
        }

        private void PlayEffect()
        {
            this.objPs.transform.Rotate(new Vector3(0, 0, 0));
            this.objPs.GetComponent<ParticleSystem>().Play();
        }

        private void setSkillCoolDown(int position)
        {
            if (this.cooldown[position] != null)
            {
                Image cooldownImage = this.cooldown[position].GetComponent<Image>();
                cooldownImage.fillAmount = 1;
            }

        }

        private void ManageCoolDown()
        {
            int i = 0;
            foreach(GameObject go in this.cooldown)
            {
                if(go != null)
                {
                    Image cooldownImage = go.GetComponent<Image>();
                    float skill1Cooldown = this.skillCoodownArr[i];
                    if (cooldownImage.fillAmount > 0)
                    {
                        cooldownImage.fillAmount -= 1 / skill1Cooldown * Time.deltaTime;
                        if (cooldownImage.fillAmount <= 0)
                        {
                            cooldownImage.fillAmount = 0;
                        }
                    }
                }
               
                i++;
                
            }

            


        }

        [Command]
        void CmdTellServerMonsterWasAttack(string uniqueID,float damage)
        {
            GameObject go = GameObject.Find(uniqueID);
            go.GetComponent<Enemy>().TakeDamage(damage);
        }

        public void EnableMarker()
        {
            PlayEffect();
            this.Marker.SetActive(true);
            float maxvol = Mathf.Floor(Random.Range(1f, 2.5f));
            m_audio.PlayOneShot(swordHit, maxvol);
            this.LunchAttack(attackHitBox);
        }
        public void DisableMarker()
        {
            this.Marker.SetActive(false);
        }
    }
}
