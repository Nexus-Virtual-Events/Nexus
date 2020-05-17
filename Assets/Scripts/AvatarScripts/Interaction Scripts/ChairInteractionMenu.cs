using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChairInteractionMenu : MonoBehaviour
{
    public void ExitMenu()
    {
        Destroy(transform.gameObject);
    }

    public void Update(){
        if(Input.GetKey(KeyCode.S)){
            Invoke("SitDown", 0.05f);
        }
    }


    public void SitDown()
    {
        GameObject localAvatar = ActionRouter.GetLocalAvatar();
        localAvatar.GetComponent<ThirdPersonUserControl>().sit = false;
        Transform chair = ActionRouter.GetCurrentChair().transform;
        localAvatar.transform.position = chair.position + chair.rotation * new Vector3(0f, -0.45f, 0f);
        localAvatar.transform.rotation = chair.rotation * Quaternion.Euler(90f, 0, 0);
        Destroy(transform.gameObject);
        localAvatar.GetComponent<ThirdPersonUserControl>().sit = true;
        localAvatar.GetComponent<ThirdPersonUserControl>().positionBeforeSitting = localAvatar.transform.position;

    }
}
