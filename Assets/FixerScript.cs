using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixerScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "car")
        {
            Debug.Log("Entered Fixer!");
            col.gameObject.GetComponent<Transform>().position = transform.position;
            col.gameObject.GetComponent<Transform>().rotation = transform.rotation;
            col.gameObject.GetComponent<Car>().StopTurningWhenEnterFix();
        }
    }

}
