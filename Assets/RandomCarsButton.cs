using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomCarsButton : MonoBehaviour
{
    [SerializeField] private Image imageOfButton;
    private bool green = false;

    public void ChangeColor()
    {
        if (green)
        {
            imageOfButton.color = Color.white;
            green = false;
        }
        else
        {
            green = true;
            imageOfButton.color = Color.green;
        }

    }

}
