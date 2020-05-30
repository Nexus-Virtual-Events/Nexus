using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 initialPosition;
    private Rigidbody rb;
    public int distance;
    public int kickForce;

    public int upForce;

    void Start()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Reset(){
        transform.position = initialPosition;
    }

    private void Kick(Vector3 kickerPosition){
        if(Vector3.Distance(kickerPosition, transform.position) < 1){
            rb.AddForce(Vector3.Normalize(transform.position - kickerPosition) * kickForce + Vector3.up * upForce);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            Kick(ActionRouter.GetLocalAvatar().transform.position);
        }
        if(Vector3.Distance(initialPosition, transform.position) > 20f){
            Reset();
        }
    }
}
