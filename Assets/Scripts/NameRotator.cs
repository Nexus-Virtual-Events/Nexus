using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameRotator : MonoBehaviour
{
    public float offsetx;
    public float offsety;
    public float offsetz;

    public Vector3 offsetcamera;


    private Vector3 parentOffset;
    // Start is called before the first frame update
    void Start()
    {
        //parentOffset = transform.position - transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.parent.position;
        //transform.LookAt(2 * transform.position - (Camera.main.transform.position+offsetcamera));
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
Camera.main.transform.rotation * Vector3.up);
    }
}
