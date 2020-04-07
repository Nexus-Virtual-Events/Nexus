using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    Camera cam;
    PlayerMotor motor;
    Rigidbody rb;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        Rigidbody rb = GetComponent<Rigidbody>();
        speed = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButtonDown(0)) {
        //   Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //   RaycastHit hit;
        //
        //   if(Physics.Raycast(ray, out hit)) {
        //     Debug.Log("Hit!");
        //     motor.MoveToPoint(hit.point);
        //
        //   }
        // }

        Vector3 pos = transform.position;

         if (Input.GetKey ("w")) {
             pos.x += speed * Time.deltaTime;
         }
         if (Input.GetKey ("s")) {
             pos.x -= speed * Time.deltaTime;
         }
         if (Input.GetKey ("d")) {
             pos.z -= speed * Time.deltaTime;
         }
         if (Input.GetKey ("a")) {
             pos.z += speed * Time.deltaTime;
         }


         transform.position = pos;
    }
}
