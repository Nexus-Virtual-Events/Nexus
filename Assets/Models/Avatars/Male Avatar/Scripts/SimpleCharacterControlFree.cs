//using UnityEngine;
//using System.Collections.Generic;
//using Normal.Realtime;

//public class AvatarController : MonoBehaviour
//{

//    private RealtimeView _realtimeView;
//    private RealtimeTransform _realtimeTransform;

//    public void Initialize(GameObject character)
//    {
//        m_animator = character.GetComponent<Animator>();
//        m_rigidBody = character.GetComponent<Rigidbody>();
//    }

//    private enum ControlMode
//    {
//        /// <summary>
//        /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
//        /// </summary>
//        Tank,
//        /// <summary>
//        /// Character freely moves in the chosen direction from the perspective of the camera
//        /// </summary>
//        Direct
//    }

//    [SerializeField] private float m_moveSpeed = 2;
//    [SerializeField] private float m_turnSpeed = 200;
//    [SerializeField] private float m_jumpForce = 4;

//    [SerializeField] private Animator m_animator;
//    [SerializeField] private Rigidbody m_rigidBody;

//    [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;

//    private float m_currentV = 0;
//    private float m_currentH = 0;

//    private readonly float m_interpolation = 10;
//    private readonly float m_walkScale = 0.33f;
//    private readonly float m_backwardsWalkScale = 0.16f;
//    private readonly float m_backwardRunScale = 0.66f;

//    private bool m_wasGrounded;
//    private Vector3 m_currentDirection = Vector3.zero;

//    private float m_jumpTimeStamp = 0;
//    private float m_minJumpInterval = 0.25f;

//    private bool m_isGrounded;

//    private List<Collider> m_collisions = new List<Collider>();

//    void Awake()
//    {
//        if(!m_animator) { gameObject.GetComponent<Animator>(); }
//        if(!m_rigidBody) { gameObject.GetComponent<Animator>(); }
//        _realtimeView = GetComponent<RealtimeView>();
//        _realtimeTransform = GetComponent<RealtimeTransform>();
//    }

//    private void OnCollisionEnter(Collision collision)
//    {

//        // If this CubePlayer prefab is not owned by this client, bail.
//        if (!_realtimeView.isOwnedLocally)
//            return;

//        // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
//        _realtimeTransform.RequestOwnership();

//        ContactPoint[] contactPoints = collision.contacts;
//        for(int i = 0; i < contactPoints.Length; i++)
//        {
//            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
//            {
//                if (!m_collisions.Contains(collision.collider)) {
//                    m_collisions.Add(collision.collider);
//                }
//                m_isGrounded = true;
//            }
//        }
//    }

//    private void OnCollisionStay(Collision collision)
//    {
//        // If this CubePlayer prefab is not owned by this client, bail.
//        if (!_realtimeView.isOwnedLocally)
//            return;

//        // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
//        _realtimeTransform.RequestOwnership();

//        ContactPoint[] contactPoints = collision.contacts;
//        bool validSurfaceNormal = false;
//        for (int i = 0; i < contactPoints.Length; i++)
//        {
//            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
//            {
//                validSurfaceNormal = true; break;
//            }
//        }

//        if(validSurfaceNormal)
//        {
//            m_isGrounded = true;
//            if (!m_collisions.Contains(collision.collider))
//            {
//                m_collisions.Add(collision.collider);
//            }
//        } else
//        {
//            if (m_collisions.Contains(collision.collider))
//            {
//                m_collisions.Remove(collision.collider);
//            }
//            if (m_collisions.Count == 0) { m_isGrounded = false; }
//        }
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        // If this CubePlayer prefab is not owned by this client, bail.
//        if (!_realtimeView.isOwnedLocally)
//            return;

//        // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
//        _realtimeTransform.RequestOwnership();

//        if (m_collisions.Contains(collision.collider))
//        {
//            m_collisions.Remove(collision.collider);
//        }
//        if (m_collisions.Count == 0) { m_isGrounded = false; }
//    }

//	void FixedUpdate ()
//    {
//        // If this CubePlayer prefab is not owned by this client, bail.
//        if (!_realtimeView.isOwnedLocally)
//            return;

//        // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
//        _realtimeTransform.RequestOwnership();

//        m_animator.SetBool("Grounded", m_isGrounded);

//        switch(m_controlMode)
//        {
//            case ControlMode.Direct:
//                DirectUpdate();
//                break;

//            case ControlMode.Tank:
//                TankUpdate();
//                break;

//            default:
//                Debug.LogError("Unsupported state");
//                break;
//        }

//        m_wasGrounded = m_isGrounded;
//    }

//    private void TankUpdate()
//    {
//        // If this CubePlayer prefab is not owned by this client, bail.
//        if (!_realtimeView.isOwnedLocally)
//            return;

//        // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
//        _realtimeTransform.RequestOwnership();

//        float v = Input.GetAxis("Vertical");
//        float h = Input.GetAxis("Horizontal");

//        bool walk = Input.GetKey(KeyCode.LeftShift);

//        if (v < 0) {
//            if (walk) { v *= m_backwardsWalkScale; }
//            else { v *= m_backwardRunScale; }
//        } else if(walk)
//        {
//            v *= m_walkScale;
//        }

//        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
//        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

//        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
//        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

//        m_animator.SetFloat("MoveSpeed", m_currentV);

//        JumpingAndLanding();
//    }

//    private void DirectUpdate()
//    {
//        // If this CubePlayer prefab is not owned by this client, bail.
//        if (!_realtimeView.isOwnedLocally)
//            return;

//        // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
//        _realtimeTransform.RequestOwnership();

//        float v = Input.GetAxis("Vertical");
//        float h = Input.GetAxis("Horizontal");

//        Transform camera = Camera.main.transform;

//        if (Input.GetKey(KeyCode.LeftShift))
//        {
//            v *= m_walkScale;
//            h *= m_walkScale;
//        }

//        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
//        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

//        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

//        float directionLength = direction.magnitude;
//        direction.y = 0;
//        direction = direction.normalized * directionLength;

//        if(direction != Vector3.zero)
//        {
//            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

//            transform.rotation = Quaternion.LookRotation(m_currentDirection);
//            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

//            m_animator.SetFloat("MoveSpeed", direction.magnitude);
//        }

//        JumpingAndLanding();
//    }

//    private void JumpingAndLanding()
//    {
//        // If this CubePlayer prefab is not owned by this client, bail.
//        if (!_realtimeView.isOwnedLocally)
//            return;

//        // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
//        _realtimeTransform.RequestOwnership();

//        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

//        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
//        {
//            m_jumpTimeStamp = Time.time;
//            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
//        }

//        if (!m_wasGrounded && m_isGrounded)
//        {
//            m_animator.SetTrigger("Land");
//        }

//        if (!m_isGrounded && m_wasGrounded)
//        {
//            m_animator.SetTrigger("Jump");
//        }
//    }
//}

using UnityEngine;
using Normal.Realtime;

namespace Normal.Realtime.Examples
{
    public class AvatarController : MonoBehaviour
    {
        public float speed = 5.0f;

        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        private void Update()
        {
            // If this CubePlayer prefab is not owned by this client, bail.
            if (!_realtimeView.isOwnedLocally)
                return;

            // Make sure we own the transform so that RealtimeTransform knows to use this client's transform to synchronize remote clients.
            _realtimeTransform.RequestOwnership();

            // Grab the x/y input from WASD / a controller
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            // Apply to the transform
            Vector3 localPosition = transform.localPosition;
            localPosition.x += x * speed * Time.deltaTime;
            localPosition.y += y * speed * Time.deltaTime;
            transform.localPosition = localPosition;
        }
    }
}

