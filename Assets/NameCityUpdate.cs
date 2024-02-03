using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameCityUpdate : MonoBehaviour
{
    [SerializeField] private Text name;
    // Start is called before the first frame update
    public void UpdateName(string newName)
    {
        name.text = newName;
    }
}
