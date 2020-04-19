using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagChairs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AddTagRecursively(transform, "Chair");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddTagRecursively(Transform trans, string tag)
    {
        trans.gameObject.tag = tag;
        if (trans.childCount > 0)
            foreach (Transform t in trans)
                AddTagRecursively(t, tag);
    }
}
