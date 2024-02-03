using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Vector3 dragOrigin;
    private float minPositionX=-10;
    private float maxPositionX=20;
    private float minPositionY=-5;
    private float maxPositionY=5;
    float zoom = 2,min=0.5f,max=8;
    void Update()
    {
        PanCamera();
        Zoom();
    }
    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 difference =dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
            float x=cam.transform.position.x;
            float y=cam.transform.position.y;
            float z=cam.transform.position.z;
            if (x<minPositionX)
                x = minPositionX;
            if(x>maxPositionX)
                x=maxPositionX;
            if(y<minPositionY)
                y = minPositionY;
            if (y > maxPositionY)
                y = maxPositionY;
                cam.transform.position=new Vector3(x,y,z);
        }
    }
    private void Zoom()
    {
        if (cam.orthographic)
        {
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoom;
            if (cam.orthographicSize < min)
                cam.orthographicSize = min;
            if (cam.orthographicSize > max)
                cam.orthographicSize = max;
        }
        else
        {
            cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * zoom;
        }
    }
}

