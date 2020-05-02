using UnityEngine;


public class MultiplayerMonoBehavior : MonoBehaviour
{
    protected void LOG (string l) {
        Debug.Log("[LOG:" + gameObject.name + "] " + l);
    }
}
