using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartInstantiate : MonoBehaviour
{
    //this is technicall script that allow instantiate the part to the map when hovering on the map and click
    public GameObject junction;
    public GameObject horizontal;
    public GameObject vertical;
    public GameObject city;
    private static PartInstantiate partInstantiator;
    string currentPart;
    Camera cam;
    public static PartInstantiate PartInstantiator { get => partInstantiator; set => partInstantiator = value; }
    public void Awake()
    {
        currentPart = "junction";
        cam = Camera.main;
        PartInstantiator = this;
    }
    public void FollowMouseDestory() => Part.DestoryPartThatFollowMouse();
    public void CreateAndUpdateCurrentPart(string newCurrentPart)
    {
        Part.DestoryPartThatFollowMouse();
        this.currentPart=newCurrentPart;
        CreateCurrentPart();
    }
    public void CreateCurrentPart()
    {
        CreateSpecificPart(currentPart);
    }
    private void CreateSpecificPart(string typeOfPart)
    {
        Vector3 worldPoint = Input.mousePosition;
        worldPoint.z = Mathf.Abs(cam.transform.position.z);
        //worldPoint.z = 11f;
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(worldPoint);
        mouseWorldPosition = new Vector3(mouseWorldPosition.x,mouseWorldPosition.y,-1f);

        switch(typeOfPart)
        {
            case "junction":
                Instantiate(junction, mouseWorldPosition, Quaternion.identity);
                break;
            case "horizontal":
                Instantiate(horizontal, mouseWorldPosition, Quaternion.identity);
                break;
            case "vertical":
                Instantiate(vertical, mouseWorldPosition, Quaternion.identity);
                break;
            case "city":
                Debug.Log("Create city");
                Instantiate(city, mouseWorldPosition, Quaternion.identity);
                break;
        }
    }
}
