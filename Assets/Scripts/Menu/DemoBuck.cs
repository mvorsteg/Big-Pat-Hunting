using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoBuck : MonoBehaviour
{
    [SerializeField]
    private int init = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetInteger("init", init);
    }
}
