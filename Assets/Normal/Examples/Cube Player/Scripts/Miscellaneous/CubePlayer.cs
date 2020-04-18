using UnityEngine;
using Normal.Realtime;
using TMPro;

namespace Normal.Realtime.Examples
{
    public class CubePlayer : MonoBehaviour
    {

        public RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        /*
        Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.
        Converted to C# 27-02-13 - no credit wanted.
        Simple flycam I made, since I couldn't find any others made public.
        Made simple to use (drag and drop, done) for regular keyboard layout
        wasd : basic movement
        shift : Makes camera accelerate
        space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/


        float mainSpeed = 10.0f; //regular speed
        float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
        float maxShift = 100.0f; //Maximum speed when holdin gshift
        float camSens = 0.25f; //How sensitive it with mouse

        private float offsetx = 0f;
        private float offsety = 1.25f;
        private float offsetz = -2f;

        private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        private float totalRun = 1.0f;

        public GameObject podiumCameraPosition;
        GameObject m_MainCamera;
        EventManager eventManager;

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        private void Start()
        {
        }

        void Ping()
        {
            //transform.Rotate(new Vector3(20f, 0f, 0f) * Time.deltaTime);
        }


        private bool isCameraParented = false;

        private void Update()
        {
            // If this CubePlayer prefab is not owned by this client, bail.
            if (!_realtimeView.isOwnedLocally)
                return;

            // Move the camera
            if (!isCameraParented)
            {
                m_MainCamera = Camera.main.gameObject;
                m_MainCamera.transform.parent = transform;
                Vector3 offset = new Vector3(offsetx, offsety, offsetz);
                m_MainCamera.transform.position = transform.position + offset + new Vector3(0, -1, 0);
                m_MainCamera.transform.LookAt(transform); 

                playerCamera = m_MainCamera.transform;

                camScript = playerCamera.GetComponent<ThirdPersonOrbitCamBasic> ();
                camScript.AssignPlayer(transform);


                SimpleMove_Awake();
                SimpleMove_Start();

                isCameraParented = true;
            }

            // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
            _realtimeTransform.RequestOwnership();


            SimpleMove_Update();
        }

        public bool isLocallyOwned ()
        {
            return _realtimeView.isOwnedLocally;
        }

    
        public Transform playerCamera;                        // Reference to the camera that focus the player.

        public float turnSmoothing = 0.06f;                   // Speed of turn when moving to match camera facing.
        public float sprintFOV = 100f;                        // the FOV to use on the camera when player is sprinting.
        public string sprintButton = "Sprint";                // Default sprint button input name.

        //protected Animator anim;                       // Reference to the Animator component.
        protected int speedFloat;                      // Speed parameter on the Animator.
        //protected BasicBehaviour behaviourManager;     // Reference to the basic behaviour manager.
        protected int behaviourCode;                   // The code that identifies a behaviour.
        protected bool canSprint;                      // Boolean to store if the behaviour allows the player to sprint.

        private ThirdPersonOrbitCamBasic camScript;                // Reference to the third person camera script.
        private bool changedFOV;                              // Boolean to store when the sprint action has changed de camera FOV.

        public float walkSpeed = 0.15f;                 // Default walk speed.
        public float runSpeed = 1.0f;                   // Default run speed.
        public float sprintSpeed = 2.0f;                // Default sprint speed.
        public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.
        public string jumpButton = "Jump";              // Default jump button.
        public float jumpHeight = 1.5f;                 // Default jump height.
        public float jumpIntertialForce = 10f;          // Default horizontal inertial force when jumping.

        private int hFloat;                                   // Animator variable related to Horizontal Axis.
        private int vFloat;                                   // Animator variable related to Vertical Axis.

        private float speed, speedSeeker;               // Moving speed.
        private int jumpBool;                           // Animator variable related to jumping.
        private int groundedBool;                       // Animator variable related to whether or not the player is on ground.
        private bool jump;                              // Boolean to determine whether or not the player started a jump.
        private bool isColliding;                       // Boolean to determine if the player has collided with an obstacle.
        private Vector3 colExtents;                           // Collider extents for ground test. 
        private int behaviourLocked;                          // Reference to temporary locked behaviour that forbids override.
        private Vector3 lastDirection;                        // Last direction the player was moving.

        private Animator anim;                          // Reference to the Animator component.
        private float h;                                      // Horizontal Axis.
        private float v;                                      // Vertical Axis.
        private Rigidbody rBody;                              // Reference to the player's rigidbody.

        private bool sprint;                                  // Boolean to determine whether or not the player activated the sprint mode.

        enum BehaviorTypes {Move, Fly};
        private BehaviorTypes currentBehavior = BehaviorTypes.Move;


        void SimpleMove_Awake()
        {
            anim = GetComponent<Animator> ();
            rBody = GetComponent<Rigidbody> ();

            hFloat = Animator.StringToHash("H");
            vFloat = Animator.StringToHash("V");

            // Set up the references.
            speedFloat = Animator.StringToHash("Speed");
            canSprint = true;

            // Set the behaviour code based on the inheriting class.
            behaviourCode = this.GetType().GetHashCode();

            // Grounded verification variables.
            groundedBool = Animator.StringToHash("Grounded");
            colExtents = GetComponent<Collider>().bounds.extents;
            
            if (playerCamera) {
                camScript = playerCamera.GetComponent<ThirdPersonOrbitCamBasic> ();
            }
        }

        // Start is always called after any Awake functions.
        void SimpleMove_Start()
        {

            // Set up the references.
            jumpBool = Animator.StringToHash("Jump");
            groundedBool = Animator.StringToHash("Grounded");
            anim.SetBool(groundedBool, true);

            // Subscribe and register this behaviour as the default behaviour.
            //behaviourManager.SubscribeBehaviour(this);
            //behaviourManager.RegisterDefaultBehaviour(this.behaviourCode);
            speedSeeker = runSpeed;
        }

        // Update is used to set features regardless the active behaviour.
        void SimpleMove_Update()
        {
            // Store the input axes.
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            // Get jump input.
            if (!jump && Input.GetButtonDown(jumpButton) && currentBehavior == BehaviorTypes.Move)
            {
                jump = true;
            }

        
            //Debug.Log("V: " + v +", H: " + h);
            // Set the input axes on the Animator Controller.
            anim.SetFloat(hFloat, h, 0.1f, Time.deltaTime);
            anim.SetFloat(vFloat, v, 0.1f, Time.deltaTime);

            // Toggle sprint by input.
            sprint = Input.GetButton (sprintButton);

            // Set the correct camera FOV for sprint mode.
            if(IsSprinting())
            {
                changedFOV = true;
                camScript.SetFOV(sprintFOV);
            }
            else if(changedFOV)
            {
                camScript.ResetFOV();
                changedFOV = false;
            }
            // Set the grounded test on the Animator Controller.
            
            anim.SetBool(groundedBool, IsGrounded());

            
        }

        void FixedUpdate() {
            if (!playerCamera) { return; }
            LocalFixedUpdate();
        }

        // LocalFixedUpdate overrides the virtual function of the base class.
        public void LocalFixedUpdate()
        {
            // Call the basic movement manager.
            MovementManagement(h, v);

            // Call the jump manager.
            JumpManagement();
        }

        // Execute the idle and walk/run jump movements.
        void JumpManagement()
        {
            // Start a new jump.
            if (jump && !anim.GetBool(jumpBool) && IsGrounded())
            {
                // Set jump related parameters.
                LockTempBehaviour(this.behaviourCode);
                anim.SetBool(jumpBool, true);
                // Is a locomotion jump?
                if (anim.GetFloat(speedFloat) > 0.1)
                {
                    // Temporarily change player friction to pass through obstacles.
                    GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
                    GetComponent<CapsuleCollider>().material.staticFriction = 0f;
                    // Remove vertical velocity to avoid "super jumps" on slope ends.
                    RemoveVerticalVelocity();
                    // Set jump vertical impulse velocity.
                    float velocity = 2f * Mathf.Abs(Physics.gravity.y) * jumpHeight;
                    velocity = Mathf.Sqrt(velocity);
                    rBody.AddForce(Vector3.up * velocity, ForceMode.VelocityChange);
                }
            }
            // Is already jumping?
            else if (anim.GetBool(jumpBool))
            {
                // Keep forward movement while in the air.
                if (!IsGrounded() && !isColliding && GetTempLockStatus())
                {
                    rBody.AddForce(transform.forward * jumpIntertialForce * Physics.gravity.magnitude * sprintSpeed, ForceMode.Acceleration);
                }
                // Has landed?
                if ((rBody.velocity.y < 0) && IsGrounded())
                {
                    anim.SetBool(groundedBool, true);
                    // Change back player friction to default.
                    GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
                    GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
                    // Set jump related parameters.
                    jump = false;
                    anim.SetBool(jumpBool, false);
                    UnlockTempBehaviour(this.behaviourCode);
                }
            }
        }

        // Deal with the basic player movement
        void MovementManagement(float horizontal, float vertical)
        {
            //Debug.Log("Ver: " + vertical +", H: " + horizontal);
            // On ground, obey gravity.
            if (IsGrounded()) {
                rBody.useGravity = true;
            }

            // Avoid takeoff when reached a slope end.
            else if (!anim.GetBool(jumpBool) && rBody.velocity.y > 0)
            {
                RemoveVerticalVelocity();
            }

            // Call function that deals with player orientation.
            Rotating(horizontal, vertical);

            // Set proper speed.
            Vector2 dir = new Vector2(horizontal, vertical);
            speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
            // This is for PC only, gamepads control speed via analog stick.
            speedSeeker += Input.GetAxis("Mouse ScrollWheel");
            speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
            speed *= speedSeeker;
            if (IsSprinting())
            {
                speed = sprintSpeed;
            }

            anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
        }

        // Remove vertical rigidbody velocity.
        private void RemoveVerticalVelocity()
        {
            Vector3 horizontalVelocity = rBody.velocity;
            horizontalVelocity.y = 0;
            rBody.velocity = horizontalVelocity;
        }

        // Rotate the player to match correct orientation, according to camera and key pressed.
        Vector3 Rotating(float horizontal, float vertical)
        {
            // Get camera forward direction, without vertical component.
            Vector3 forward = playerCamera.TransformDirection(Vector3.forward);

            // Player is moving on ground, Y component of camera facing is not relevant.
            forward.y = 0.0f;
            forward = forward.normalized;

            // Calculate target direction based on camera forward and direction key.
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            Vector3 targetDirection;
            targetDirection = forward * vertical + right * horizontal;

            // Lerp current direction to calculated target direction.
            if ((IsMoving() && targetDirection != Vector3.zero))
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                Quaternion newRotation = Quaternion.Slerp(rBody.rotation, targetRotation, turnSmoothing);
                rBody.MoveRotation(newRotation);
                SetLastDirection(targetDirection);
            }
            // If idle, Ignore current camera facing and consider last moving direction.
            if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
            {
                Repositioning();
            }

            return targetDirection;
        }

        // Collision detection.
        private void OnCollisionStay(Collision collision)
        {
            if (!_realtimeView.isOwnedLocally)
                return;
            isColliding = true;
            // Slide on vertical obstacles
            if (currentBehavior == BehaviorTypes.Move && collision.GetContact(0).normal.y <= 0.1f)
            {
                float vel = anim.velocity.magnitude;
                Vector3 tangentMove = Vector3.ProjectOnPlane(transform.forward, collision.GetContact(0).normal).normalized * vel;
                rBody.AddForce(tangentMove, ForceMode.VelocityChange);
            }

        }
        private void OnCollisionExit(Collision collision)
        {
            if (!_realtimeView.isOwnedLocally)
                return;
            isColliding = false;
        }

        public bool IsGrounded()
        {
            Ray ray = new Ray(this.transform.position + Vector3.up * 2 * colExtents.x, Vector3.down);
            return Physics.SphereCast(ray, colExtents.x, colExtents.x + 0.2f);
        }

        // Atempt to lock on a specific behaviour.
        //  No other behaviour can overrhide during the temporary lock.
        // Use for temporary transitions like jumping, entering/exiting aiming mode, etc.
        public void LockTempBehaviour(int behaviourCode)
        {
            if (behaviourLocked == 0)
            {
                behaviourLocked = behaviourCode;
            }
        }

        // Attempt to unlock the current locked behaviour.
        // Use after a temporary transition ends.
        public void UnlockTempBehaviour(int behaviourCode)
        {
            if(behaviourLocked == behaviourCode)
            {
                behaviourLocked = 0;
            }
        }

        // Check if any other behaviour is temporary locked.
        public bool GetTempLockStatus(int behaviourCodeIgnoreSelf = 0)
        {
            return (behaviourLocked != 0 && behaviourLocked != behaviourCodeIgnoreSelf);
        }

        // Check if player is sprinting.
        public virtual bool IsSprinting()
        {
            return sprint && IsMoving() && CanSprint();
        }

        // Check if the player is moving.
        public bool IsMoving()
        {
            return (h != 0)|| (v != 0);
        }

        // Set the last player direction of facing.
        public void SetLastDirection(Vector3 direction)
        {
            lastDirection = direction;
        }

        // Put the player on a standing up position based on last direction faced.
        public void Repositioning()
        {
            if(lastDirection != Vector3.zero)
            {
                lastDirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation (lastDirection);
                Quaternion newRotation = Quaternion.Slerp(rBody.rotation, targetRotation, turnSmoothing);
                rBody.MoveRotation (newRotation);
            }
        }

        // Check if player can sprint (all behaviours must allow).
        public bool CanSprint()
        {
            return true;
        }

    }

}
