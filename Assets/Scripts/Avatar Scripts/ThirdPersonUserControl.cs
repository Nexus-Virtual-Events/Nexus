using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.ThirdPerson;
using System.IO;
using UMA.CharacterSystem;
using System;
using Normal.Realtime;


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

    public bool sit = false;
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
    private string prevFocusState;


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


        if (_realtimeView.isOwnedLocally)
        {
            ActionRouter.SetLocalAvatar(transform.gameObject);
            avatar.ClearSlots();
            avatarRecipe = PlayerPrefs.GetString("playerRecipe");
            avatar.LoadFromRecipeString(avatarRecipe);

            //SET RECIPE UPON ENTERING THE GAME
            RecipeSync _recipeSync;
            _recipeSync = GameObject.FindObjectOfType<RecipeSync>();
            _recipeSync.SetRecipe(PlayerPrefs.GetString("playerRecipe"));
        }
        else {
            // Set as remote avatar
            transform.gameObject.layer = LayerMask.NameToLayer("RemoteAvatar");
        }
    }


    public void ReactToInteractionChange(GameObject sourceCharacter, string newInteraction)
    {
        Debug.Log("Interaction type: " + newInteraction);

        float DISTANCE_FOR_HANDSHAKE = 0.8f;
        if (!_realtimeView.isOwnedLocally) { return; }

        Vector3 otherPosition = sourceCharacter.transform.position;
        Vector3 diff = (otherPosition - transform.position);

        Vector3 centerTarget = (diff/2) + transform.position;

        Vector3 centerToTargetVect = (transform.position - centerTarget);
        centerToTargetVect.Normalize();
        Vector3 moveToTarget = centerTarget + centerToTargetVect * (DISTANCE_FOR_HANDSHAKE/2);

        canMove = false;
        autoPilot = true;
        autoTarget = moveToTarget;
    }

    int maxId = -1;

    public void ReactToPodiumChange(int newPodium)
    {

        if (!_realtimeView.isOwnedLocally) { return; }

        if (newPodium == -1)
        {
            Debug.Log("Podium reset");
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (maxId != -1)
                    if (player.GetComponent<ThirdPersonUserControl>().getID() == maxId)
                    {
                        player.GetComponent<AudioSource>().spatialBlend = 1;
                    }
            }
            return;
        }

        Debug.Log("podium changed to "+ newPodium.ToString());

        float maxDistance = 1000;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            float dist = (player.transform.position - GameObject.Find("Podium").transform.position).magnitude;
                
            if (dist < maxDistance)
            {
                maxDistance = dist;
                maxId = player.GetComponent<ThirdPersonUserControl>().getID();
            }
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(maxId != -1)
                if(player.GetComponent<ThirdPersonUserControl>().getID() == maxId)
                    {
                        player.GetComponent<AudioSource>().spatialBlend = 0;
                    }
        }

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
            GameObject.Find("Realtime").GetComponent<AdminPanel>().FocusCamera();
        }
        else if(prevFocusState == "1" && eventManager.GetEvents()[2] == '0')
        {
            Camera.main.transform.position = cameraStay.position;
            Camera.main.transform.rotation = cameraStay.rotation;
            camScript.AssignPlayer(transform);
            canMove = true;
            GameObject.Find("Realtime").GetComponent<AdminPanel>().UnfocusCamera();
        }

        prevFocusState = eventManager.GetEvents()[2].ToString();
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
            return;
        
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

    private string parseMoveToString(Vector3 move, bool[] toggleAnimations)
    {
        string animationString = move.x.ToString() + " " + move.y.ToString() + " " + move.z.ToString() + " ";
        foreach (bool animation in toggleAnimations)
        {
            animationString += Convert.ToInt16(animation) + " ";
        }
        return animationString;
    }

   private int startedShakingHandsAt = 0;
    private bool IsShakingHands () {
        if (Time.frameCount > startedShakingHandsAt + 8) {
            return false;
        }
        else {
            return true;
        }
    }

    private void StartShakingHands () {
        startedShakingHandsAt = Time.frameCount;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // If this CubePlayer prefab is not owned by this client, bail.
        if (!_realtimeView.isOwnedLocally)
        {

            m_Character.ForeignMove(GetComponent<UpdateMove>().GetCharacterMove());
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


                if ((h != 0f || v != 0f) && sit)
                {
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

                bool[] toggleInformation = new bool[6];
                toggleInformation[0] = crouch;
                toggleInformation[1] = m_Jump;
                toggleInformation[2] = clap;
                toggleInformation[3] = wave;
                toggleInformation[4] = sit;
                toggleInformation[5] = IsShakingHands();


                GetComponent<UpdateMove>().UpdateCharacterMove(parseMoveToString(m_Move, toggleInformation));

                m_Jump = false;
            }
            else
            {
                if (autoPilot)
                {

                    m_Character.Move(autoTarget - transform.position, false, false, false, false, false);
                    bool[] toggleInformation = new bool[6];
                    toggleInformation[0] = false;
                    toggleInformation[1] = false;
                    toggleInformation[2] = false;
                    toggleInformation[3] = false;
                    toggleInformation[4] = false;
                    toggleInformation[5] = IsShakingHands();


                    if(Vector3.Distance(transform.position, autoTarget) < 0.1)
                    {
                        //handshake finishing action
                        StartShakingHands();
                        toggleInformation[5] = IsShakingHands();
                        Debug.Log("toggleInformation" + toggleInformation.ToString());
                        //m_Character.Move(new Vector3(0, 0, 0), false, false, false, false, false);
                        m_Character.StartShakeHand();
                        GetComponent<UpdateMove>().UpdateCharacterMove(parseMoveToString(new Vector3(0, 0, 0), toggleInformation));
                        canMove = true;
                        autoPilot = false;
                        Debug.Log("target reached");
                    }
                    else {
                        GetComponent<UpdateMove>().UpdateCharacterMove(parseMoveToString(autoTarget - transform.position, toggleInformation));
                    }
                }
            }
        }
    }
}

