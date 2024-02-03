using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class junctionCollider : MonoBehaviour
{
    [SerializeField] private JunctionLightManager lightManager;
    public JunctionLightManager GetTheJunctionManagerScript() => lightManager;


}
