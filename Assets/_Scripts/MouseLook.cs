using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    [SerializeField] float mouseScensitivity = 100;
    [SerializeField] Transform cameraTrans = null;

    float xRotate = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mouse = Vector3.zero;
        mouse.x = Input.GetAxis("Mouse X");
        mouse.y = Input.GetAxis("Mouse Y");

        mouse *= mouseScensitivity * Time.fixedDeltaTime;


        xRotate -= mouse.y;
        xRotate = Mathf.Clamp(xRotate, -90, 90);

        cameraTrans.localRotation = Quaternion.Euler(xRotate, 0, 0);
        transform.Rotate(Vector3.up * mouse.x);
    }
}
