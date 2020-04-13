using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMenu : MonoBehaviour
{
    public void ExitMenu()
    {
        Destroy(transform.parent.gameObject);
    }
}
