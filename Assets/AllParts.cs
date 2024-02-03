using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllParts : MonoBehaviour
{
    private static List<GameObject> allPrefabs= new List<GameObject>();
    public static void AddObjectToList(GameObject g)
    {
        allPrefabs.Add(g);
    }
    public static void ResetList()
    {
        foreach (GameObject go in allPrefabs)
        Destroy(go);
        allPrefabs = new List<GameObject>();
    }
}

