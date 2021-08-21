using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionObject : MonoBehaviour
{

    [SerializeField] GameObject projObject = null;
    [SerializeField] int defaultLayer = 0;
    [SerializeField] int projLayer = 10;
    [SerializeField] Rigidbody rigidbody = null;
    [SerializeField] Collider collider = null;

    Vector3 moveTarget;
    bool draging;

    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if(draging)
        {
            rigidbody.MovePosition(Vector3.Lerp(transform.position, moveTarget, 10 * Time.deltaTime));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    public void DragMove(Vector3 target)
    {
        draging = true;

        rigidbody.isKinematic = false;
        rigidbody.useGravity = false;
        rigidbody.freezeRotation = true;
        collider.isTrigger = false;

        moveTarget = target;

        projObject.layer = defaultLayer;
    }
    public void Rotate(float rot)
    {
        transform.Rotate(Vector3.up * rot * Time.deltaTime);
    }
    public void Fizzing()
    {
        draging = false;

        rigidbody.isKinematic = true;
        collider.isTrigger = true;
        projObject.layer = projLayer;
    }
}
