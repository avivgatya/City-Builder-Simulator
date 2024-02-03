using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    private void Start()
    {
        StartCoroutine("Operation");
    }
    private IEnumerator Operation()
    {
        float angleTarget = 90;
        float xTarget=target.position.x;
        float yTarget=target.position.y;
        //
        float angleSpinned = 0;

        while (angleSpinned<angleTarget || transform.position.x<xTarget || transform.position.y<yTarget)//תמשיך כל עוד
        {
           // angleSpinned+= transform.Rotate(0, 0, amount);
            
            yield return null;
        }
    }
}
