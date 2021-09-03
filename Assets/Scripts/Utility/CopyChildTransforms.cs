using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CopyChildTransforms : MonoBehaviour
{
    public bool copyRotation = false;
    public GameObject other;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        if (copyRotation)
        {
            CopyRotationRecursive(transform, other.transform);
            copyRotation = false;
        }
    }

    private static void CopyRotationRecursive(Transform me, Transform other)
    {
        me.rotation = other.rotation;
        if (me.childCount == other.childCount)
        {
            for (int i = 0; i < other.transform.childCount; i++)
            {
                CopyRotationRecursive(me.GetChild(i), other.GetChild(i));
            }
        }
    }
}
