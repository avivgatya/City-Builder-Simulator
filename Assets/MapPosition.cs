using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.localScale.x/2-0.5f,-transform.localScale.y/2+0.5f,0);
    }

}
