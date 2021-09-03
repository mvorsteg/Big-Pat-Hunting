using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

[ExecuteInEditMode]
public class SetPosToAnim : MonoBehaviour 
{
    public bool printRotation;

    private void OnValidate()
    {
        if (printRotation)
        {
            PrintRot(transform);
        }
    }

    private static void PrintRot(Transform me)
    {
        Debug.Log(me.name + " x: " + me.localEulerAngles.x + " y: " + me.localEulerAngles.y + " z: " + me.localEulerAngles.z);
        for (int i = 0; i < me.childCount; i++)
        {
            PrintRot(me.GetChild(i));
        }
    }
}