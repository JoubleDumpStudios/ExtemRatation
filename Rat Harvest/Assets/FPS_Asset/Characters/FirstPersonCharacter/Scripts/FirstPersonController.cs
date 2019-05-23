using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;


namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        // Camera ray's Length
        [SerializeField] private float Cam_Ray_Length = 3;

        // Position where we are going to throw the raycast
        [SerializeField] private Transform Harvest_Raycast_SpawnPoint;

        // A variable to access the PlayerManager
        private PlayerManager playerManager;

        // A variable to pass the data of the soil between objects
        private GameObject plantBehaviour;

        // A variable to access the soil script and be able to change its state to forbidden the player planting crops
        private PlantPoint PlantPointScript;

        // Variable to acces the plantBehaviour script to pass the data of the soil
        private Plant_Behaviour plantBehaviourScript;

        // Variable to access to the outline script
        private OutlineManager outlineScript;

        private GameObject gameobjectCollided;

        //Variable to acces the shotgunbehaviour to fill the ammo pocket.
        [SerializeField]
        private ShotGunBehaviour shotGunBehaviourScript_;

        private Animator shotGunAnimator;
        public Animator ShotGunAnimator { get { return this.shotGunAnimator; } }

        //allow us to detect the colision of an object with the barrel to avoid the raycasting inside the objects bug
        private bool collidingWithBarrel;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);

            playerManager = GetComponent<PlayerManager>();
            shotGunAnimator = GetComponentInChildren<Animator>();
        }

        public void cameraRecoil(float ang, float cameraRecoilSpeed, float cameraRecoilTime)
        {
            m_MouseLook.cameraRecoil(ang, cameraRecoilSpeed, cameraRecoilTime);
        }
        // Update is called once per frame
        private void Update()
        {
            if(Time.timeScale != 0)
                GetPlayerInput();          
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed = 0f;


            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;


            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }

        private void GetPlayerInput()
        {
            Ray ray = new Ray(Harvest_Raycast_SpawnPoint.position, Harvest_Raycast_SpawnPoint.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Cam_Ray_Length))
            {
                Debug.DrawLine(ray.origin, hit.point);

                gameobjectCollided = hit.collider.gameObject;

                if (gameobjectCollided.GetComponent<PlantModel>() != null)
                {
                    if (gameobjectCollided.GetComponentInParent<Plant_Behaviour>().CanBeHarvested())
                        playerManager.EnableHarvestIcon();
                    else
                        playerManager.DisableHarvestIcon();

                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                        Harvest(gameobjectCollided);
                }
                else if (gameobjectCollided.GetComponent<PlantPoint>() != null)
                {
                    PlantPointScript = gameobjectCollided.GetComponent<PlantPoint>();

                    if (!PlantPointScript.HasCrop)
                        playerManager.EnablePlantIcon();
                    else
                        playerManager.DisablePlantIcon();

                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                        Plant(gameobjectCollided);
                }
                else if (gameobjectCollided.gameObject.tag == "AmmoChest")
                {
                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                    {
                        shotGunBehaviourScript_.FillBulletsPocket();
                    }
                }
                else if (gameobjectCollided.gameObject.tag == "HarvestChest")
                {
                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                    {
                        playerManager.updateScore();
                    }
                }
                else
                    DisablePlayerIcons();
            }
            else
                DisablePlayerIcons();

            IfBarrelInsideTheObject();

            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");


            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Run"))
                m_IsWalking = false;
            else
                m_IsWalking = true;
            //m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        // A method to plant a new crop in a gameobject
        private void Plant(GameObject go)
        {
            //PlantPointScript = go.GetComponent<PlantPoint>();

            if (PlantPointScript != null)
                if (!PlantPointScript.HasCrop)
                {
                    PlantPointScript.enablePlantIcon();
                    spawnPlant(go);
                    playerManager.DisablePlantIcon();
                }
                                     
        }

        // A method that spawns a plant
        private void spawnPlant(GameObject go)
        {
            ObjectPooler.instance.spawnFromPool("Plant", go.transform.position,
                        go.transform.rotation, out plantBehaviour);

            plantBehaviour.GetComponent<Plant_Behaviour>().PlantPoint = go;
            PlantPointScript.HasCrop = true;
            PlantPointScript.Plant = plantBehaviour;
            //PlantPointScript.EnablePlantEatingPoints();
        }

        // Method to harvest a crop
        private void Harvest(GameObject go)
        {
            GameObject rootPlant = go.transform.parent.gameObject;
            plantBehaviourScript = rootPlant.GetComponent<Plant_Behaviour>();

            if (plantBehaviourScript.CanBeHarvested())
            {
                int points = plantBehaviourScript.CurrentPoints;

                //int maxpoints = playerManager.BagCapacity;
                //int p = playerManager.PlayerHarvest + points;

                //if (playerManager.PlayerHarvest < playerManager.BagCapacity && p > maxpoints)               
                //    points = playerManager.BagCapacity - playerManager.PlayerHarvest;               
                //else
                //    points = 0;

                //playerManager.updateScore(points);
                playerManager.updateBag(points);

                resetPlantPointStatus(rootPlant);
            }          
        }

        // Method to reset all the statistics of the soil when it is harvested
        private void resetPlantPointStatus(GameObject rootPlant)
        {
            plantBehaviourScript.resetPlant();
            ObjectPooler.instance.killGameObject(rootPlant);
        }

        // Method to disable Plant and HarvestIcons
        private void DisablePlayerIcons()
        {
            playerManager.DisablePlantIcon();
            playerManager.DisableHarvestIcon();
        }

        private void IfBarrelInsideTheObject()
        {
            if (collidingWithBarrel)
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                {
                    shotGunBehaviourScript_.FillBulletsPocket();
                }
            }
        }


        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "AmmoChest")
            {

                collidingWithBarrel = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.tag == "AmmoChest")
            {

                collidingWithBarrel = false;
            }
        }

    }
}
