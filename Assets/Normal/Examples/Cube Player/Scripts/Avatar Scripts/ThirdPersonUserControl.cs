using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;
using System.IO;
using UMA.CharacterSystem;
using System;

namespace Normal.Realtime.Examples
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {

        public RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        private float offsetx = 0f;
        private float offsety = 1.25f;
        private float offsetz = -2f;

        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        GameObject m_MainCamera;
        EventManager eventManager;
        public Transform playerCamera;
        private ThirdPersonOrbitCamBasic camScript;

        ModifyInteraction interactionModifier;

        public DynamicCharacterAvatar avatar;
        public string avatarRecipe;
        public string foreignAvatarRecipe;

        public bool sit;
        public Vector3 positionBeforeSitting;

        private bool isCameraParented = false;

        private int currentNumPlayers;
        private int prevNumPlayers;

        //Camera focus stuff
        private bool canMove = true;
        private bool autoPilot = false;
        private Vector3 autoTarget;

        private GameObject FocusCameraPosition;
        private Transform cameraStay;

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        public bool isLocallyOwned()
        {
            return _realtimeView.isOwnedLocally;
        }

        public int getID()
        {
            return _realtimeView.ownerID;
        }

        private void Start()
        {
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
            eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
            eventManager.OnEventsChange.AddListener(SwitchFocus);

            interactionModifier = GetComponent<ModifyInteraction>();
            interactionModifier.OnInteractionsChange.AddListener(reactToInteractionChange);


            if (_realtimeView.isOwnedLocally)
            {
               ActionRouter.SetLocalAvatar(transform.gameObject);
               sit = false;
            }
         
        }


        private void reactToInteractionChange()
        {

            if (!_realtimeView.isOwnedLocally)
                return;


            string[] parameters = stringToArray(interactionModifier.interaction);


            if (parameters[2] == "0")
                return;

            if (parameters[0] != getID().ToString() && parameters[1] != getID().ToString())
            {
                Debug.Log("irrelevant");
                return;
            }

            Vector3 otherPosition = ActionRouter.GetCurrentCharacter().transform.position;
            Vector3 target = ((otherPosition - transform.position)/2) + transform.position;

            Debug.Log("target: " + target.ToString());

            canMove = false;
            autoPilot = true;
            autoTarget = target;
            
        }

        private string[] stringToArray(string s)
        {
            string[] parameters;

            //"(float forwardamount) (float turnamount) (int crouching) (int onGround)"
            parameters = s.Split(' ');
            return parameters;
        }

        private void SwitchFocus()
        {
            if (!_realtimeView.isOwnedLocally)
                return;

            FocusCameraPosition = GameObject.Find("FocusCamPos");

            if (eventManager.GetEvents()[2] == '1')
            {
                cameraStay = Camera.main.transform;
                camScript.AssignPlayer(FocusCameraPosition.transform);
                canMove = false;
            }
            else
            {
                Camera.main.transform.position = cameraStay.position;
                Camera.main.transform.rotation = cameraStay.rotation;
                camScript.AssignPlayer(transform);
                canMove = true;
            }

        }


        private void Update()
        {

            // render new avatars by checking if the number of players has changed
            currentNumPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
            if (currentNumPlayers != prevNumPlayers)
            {
                if (avatarRecipe != null && avatarRecipe.Length > 100)
                {
                    avatar.ClearSlots();
                    avatar.LoadFromRecipeString(avatarRecipe);
                    prevNumPlayers = currentNumPlayers;
                }
            }

            // If this CubePlayer prefab is not owned by this client, bail.
            if (!_realtimeView.isOwnedLocally)
            {
                //do nothing
            }
            else
            {
                // Move the camera
                if (!isCameraParented)
                {
                    m_MainCamera = Camera.main.gameObject;
                    m_MainCamera.transform.parent = transform;
                    Vector3 offset = new Vector3(offsetx, offsety, offsetz);
                    m_MainCamera.transform.position = transform.position + offset;
                    m_MainCamera.transform.LookAt(transform);

                    playerCamera = m_MainCamera.transform;

                    camScript = playerCamera.GetComponent<ThirdPersonOrbitCamBasic>();
                    camScript.AssignPlayer(transform);

                    isCameraParented = true;
                }

                // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
                _realtimeTransform.RequestOwnership();
                if (canMove)
                {
                    if (!m_Jump)
                    {
                        m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                    }

                }
            }
        }

        private string parseMoveToString(Vector3 move, bool[] toggleAnimations)
        {
            string animationString = move.x.ToString() + " " + move.y.ToString() + " " + move.z.ToString() + " ";
            foreach (bool animation in toggleAnimations) {
                animationString += Convert.ToInt16(animation) + " ";
            }
            return animationString;
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // If this CubePlayer prefab is not owned by this client, bail.
            if (!_realtimeView.isOwnedLocally)
            {

                m_Character.ForeignMove(GetComponent<UpdateMove>().characterMove);
            }
            else
            {

                // Move the camera
                if (!isCameraParented)
                {
                    m_MainCamera = Camera.main.gameObject;
                    m_MainCamera.transform.parent = transform;
                    Vector3 offset = new Vector3(offsetx, offsety, offsetz);
                    m_MainCamera.transform.position = transform.position + offset + new Vector3(0, -1, 0);
                    m_MainCamera.transform.LookAt(transform);

                    playerCamera = m_MainCamera.transform;

                    camScript = playerCamera.GetComponent<ThirdPersonOrbitCamBasic>();
                    camScript.AssignPlayer(transform);

                    isCameraParented = true;
                }

                // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
                _realtimeTransform.RequestOwnership();

                if (canMove)
                {
                    // read inputs
                    float h = CrossPlatformInputManager.GetAxis("Horizontal");
                    float v = CrossPlatformInputManager.GetAxis("Vertical");
                    bool crouch = Input.GetKey(KeyCode.C);
                    bool clap = Input.GetKey(KeyCode.Alpha1);
                    bool wave = Input.GetKey(KeyCode.Alpha2);

                    if (sit)
                    {
                        GetComponent<CapsuleCollider>().enabled = false;
                        GetComponent<Rigidbody>().useGravity = false;
                    }
                    

                    if ((h != 0f || v != 0f) && sit){
                        Debug.Log("Stand up");
                        sit = false;
                        transform.position = positionBeforeSitting;
                        GetComponent<CapsuleCollider>().enabled = true;
                        GetComponent<Rigidbody>().useGravity = true;

                    }


                    // calculate move direction to pass to character
                    if (m_Cam != null)
                    {
                        // calculate camera relative direction to move:
                        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                        m_Move = v * m_CamForward + h * m_Cam.right;
                    }
                    else
                    {
                        // we use world-relative directions in the case of no main camera
                        m_Move = v * Vector3.forward + h * Vector3.right;
                    }
#if !MOBILE_INPUT
                    // walk speed multiplier
                    if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

                    // pass all parameters to the character control script
                    m_Character.Move(m_Move, crouch, m_Jump, clap, wave, sit);

                    bool[] toggleInformation = new bool[5];
                    toggleInformation[0] = crouch;
                    toggleInformation[1] = m_Jump;
                    toggleInformation[2] = clap;
                    toggleInformation[3] = wave;
                    toggleInformation[4] = sit;

                    GetComponent<UpdateMove>().characterMove = parseMoveToString(m_Move, toggleInformation);

                    m_Jump = false;
                }
                else
                {
                    if (autoPilot)
                    {
                        Debug.Log("autoPilot " + getID().ToString());

                        m_Character.Move(autoTarget - transform.position, false, false, false, false, false);
                        bool[] toggleInformation = new bool[5];
                        toggleInformation[0] = false;
                        toggleInformation[1] = false;
                        toggleInformation[2] = false;
                        toggleInformation[3] = false;
                        toggleInformation[4] = false;

                        GetComponent<UpdateMove>().characterMove = parseMoveToString(m_Move, toggleInformation);

                        if(Vector3.Distance(transform.position, autoTarget) < 0.1)
                        {
                            canMove = true;
                            autoPilot = false;
                            Debug.Log("target reached");
                        }
                    }
                }
            }
        }
    }
}

