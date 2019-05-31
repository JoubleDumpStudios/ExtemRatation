using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;


namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
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
        private bool barrelCollidingWithAmmoChest;
        private bool barrelCollidingWithHarvestChest;
        private bool barrelCollidingWithPlant;
        private bool barrelCollidingWithPlantPoint;


        private GameObject nastyCollidedGameObject;

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
            AkSoundEngine.PostEvent("Land", gameObject);
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
                    playerManager.EnableFillingAmmoIcon();

                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                    {
                        shotGunBehaviourScript_.FillBulletsPocket();
                        Debug.Log("Fill ammo now!");
                    }
                }
                else if (gameobjectCollided.gameObject.tag == "HarvestChest")
                {
                    playerManager.EnableFillingHarvestIcon();
                    if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                        playerManager.updateScore(); 
                }
                else
                    DisablePlayerIcons();
            }
            else
                DisablePlayerIcons();

            IfBarrelInsideTheObject(nastyCollidedGameObject);

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
            AkSoundEngine.PostEvent("Jump", gameObject);
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
           
            AkSoundEngine.PostEvent("Footstep", gameObject);
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
            plantBehaviour.GetComponent<Plant_Behaviour>().StartRound = PlantPointScript.RoundStarted;
            //PlantPointScript.EnablePlantEatingPoints();
        }

        // Method to harvest a crop
        private void Harvest(GameObject go)
        {
            GameObject rootPlant = go.transform.parent.gameObject;
            plantBehaviourScript = rootPlant.GetComponent<Plant_Behaviour>();

            if (plantBehaviourScript.CanBeHarvested())
            {
                AkSoundEngine.PostEvent("HUD_Harvest", gameObject);
                int points = plantBehaviourScript.CurrentPoints;
                playerManager.updateBag(points);
                resetPlantPointStatus(rootPlant);
                playerManager.DisableHarvestIcon();
                barrelCollidingWithPlant = false;
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
            if(!barrelCollidingWithPlantPoint)
                playerManager.DisablePlantIcon();

            if (!barrelCollidingWithPlant)
                playerManager.DisableHarvestIcon();

            if(!barrelCollidingWithAmmoChest)
                playerManager.DisableFillingAmmoIcon();

            if(!barrelCollidingWithHarvestChest)
                playerManager.DisableFillingHarvestIcon();
        }

        private void IfBarrelInsideTheObject(GameObject collidedGameObject)
        {
            if (barrelCollidingWithAmmoChest)
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                    shotGunBehaviourScript_.FillBulletsPocket();
            }
            else if (barrelCollidingWithHarvestChest)
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                    playerManager.updateScore();
            }
            else if (barrelCollidingWithPlant)
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                    Harvest(collidedGameObject);
            }
            else if (barrelCollidingWithPlantPoint)
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                    Plant(collidedGameObject);
            }

        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "AmmoChest")
            {
                playerManager.EnableFillingAmmoIcon();
                barrelCollidingWithAmmoChest = true;
            }
            else if (collision.gameObject.tag == "HarvestChest")
            {
                playerManager.EnableFillingHarvestIcon();
                barrelCollidingWithHarvestChest = true;
            }
            else if (collision.gameObject.GetComponent<PlantModel>() != null)
            {
                nastyCollidedGameObject = collision.gameObject;
                playerManager.EnableHarvestIcon();
                barrelCollidingWithPlant = true;
            }
            else if (collision.gameObject.GetComponent<PlantPoint>() != null)
            {
                nastyCollidedGameObject = collision.gameObject;
                playerManager.EnablePlantIcon();
                barrelCollidingWithPlantPoint = true;
                PlantPointScript = collision.gameObject.GetComponent<PlantPoint>();
            }
            else if (collision.gameObject.GetComponent<Rat_Movement_Logic>() != null)
            {
                nastyCollidedGameObject = collision.gameObject;

                shotGunBehaviourScript_.BarrelOnRat = true;
                shotGunBehaviourScript_.RatOnBarrel = nastyCollidedGameObject;

            }
            else        
                DisablePlayerIcons();    
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.tag == "AmmoChest")
            {
                playerManager.DisableFillingAmmoIcon();
                barrelCollidingWithAmmoChest = false;
            }
            else if (collision.gameObject.tag == "HarvestChest")
            {
                playerManager.DisableFillingHarvestIcon();
                barrelCollidingWithHarvestChest = false;
            }
            else if (collision.gameObject.GetComponent<PlantModel>() != null)
            {
                playerManager.DisableHarvestIcon();
                barrelCollidingWithPlant = false;
            }
            else if (collision.gameObject.GetComponent<PlantPoint>() != null)
            {
                playerManager.DisablePlantIcon();
                barrelCollidingWithPlantPoint = false;
            }
            else if (collision.gameObject.GetComponent<Rat_Movement_Logic>() != null)
            {
                shotGunBehaviourScript_.BarrelOnRat = false;
                shotGunBehaviourScript_.RatOnBarrel = null;

            }

        }

    }
}
