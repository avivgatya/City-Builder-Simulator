using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMark : MonoBehaviour
{
    //this code teach us how the system work with the red tile, how it choose the position
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        int x = (int)(Math.Round(mousePosition.x));
        int y = (int)(Math.Round(mousePosition.y));
        if ((MyGrid.RefToMyGrid).CheckPositionInsideMap(-y, x)&&Part.DoesPartFollowNowMouse())
            transform.position = new Vector3(x, y, mousePosition.z+1);
        else
            transform.position = new Vector3(-20, 20, mousePosition.z);
    }
}

