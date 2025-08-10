using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float xSensitivity;
    public float ySensitivity;

    float xRotate;
    float yRotate;

    public Transform player;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * xSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * ySensitivity;

        yRotate += mouseX;
        xRotate -= mouseY;
        xRotate = Mathf.Clamp(xRotate, -90, 90);

        transform.rotation = Quaternion.Euler(xRotate, yRotate, 0);
        player.rotation = Quaternion.Euler(0, yRotate, 0);
        
    }
}
