using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPut : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private void Start()
    {
        canvas.worldCamera = Camera.main;       
    }
}
