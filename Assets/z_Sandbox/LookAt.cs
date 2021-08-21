using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAt : MonoBehaviour
{

    [SerializeField] Transform target = null;

    void Start()
    {
        
    }
    void Update()
    {
        transform.LookAt(target);
    }
}
