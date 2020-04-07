using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float horizontal_speed = 2.0f;
    public float vertical_speed = 2.0f;

    public float yaw = 0.0f;
    public float pitch = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        yaw +=  horizontal_speed * Input.GetAxis("Mouse X");
        pitch -= vertical_speed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        
    }
}
