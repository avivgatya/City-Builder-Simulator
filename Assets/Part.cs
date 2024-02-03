using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Part : MonoBehaviour
{
    [SerializeField] GameObject refToThisObject;
    static GameObject partThatFollowMouse; //part that doesn't put on map yet.
    static int cnt = 0;
    int id = 0;
    bool follow; //check if right now it follows or not
    bool createdFromLoad=false;
    [SerializeField] private JunctionLightManager refToJunctionLightManagerScript; //can be NULL!!!!
    [SerializeField] private GameObject refToCanvasOfCity; //can be NULL!!!!

    private bool alreadyPositioned = false;

    public static GameObject PartThatFollowMouse { get => partThatFollowMouse; set => partThatFollowMouse = value; }
    public int Id { get => id; set => id = value; }

    void Awake()
    {
        follow = true;
        PartThatFollowMouse = refToThisObject;
        cnt++;
        Id = cnt;
    }
    // Update is called once per frame
    void Update()
    {
        if (!createdFromLoad) //because if created from load nothing should follow mouse
        {
            if (follow) //if part right now follow mouse, it should be placed
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
                transform.position = mousePosition;
            }
            if (Input.GetButtonDown("Fire1")) //fire1 means pressed on mouse in Unity language
            {
                if (!alreadyPositioned)//after part position in the map it changes to the positioned.
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    { 
                        Vector3 currentPosition = transform.position;
                        int x = (int)(Math.Round(currentPosition.x));
                        int y = (int)(Math.Round(currentPosition.y));
                        if ((MyGrid.RefToMyGrid).CheckPositionInsideMap(-y, x)&& !EventSystem.current.IsPointerOverGameObject())
                        {
                            if (refToCanvasOfCity != null)
                                refToCanvasOfCity.SetActive(true); //showing the name of the city (it was off to prevent UI touch - check the upper if)

                            if ((MyGrid.RefToMyGrid).CheckPositionEmpty(-y, x))
                            {
                                alreadyPositioned = true;
                                follow = false;
                                if (refToJunctionLightManagerScript != null)
                                    refToJunctionLightManagerScript.StartTrafficLightInJunction();

                                if (PartThatFollowMouse == refToThisObject)
                                    PartThatFollowMouse = null;
                                (PartInstantiate.PartInstantiator).CreateCurrentPart(); //after put one part on map, we make another one so player no need to press agaain on button each tile he put
                                transform.position = new Vector3(x, y, -0.1f);
                                (MyGrid.RefToMyGrid).AddPartToMap(refToThisObject, this, -y, x);
                            }
                        }
                        else
                        {
                            Debug.Log("Position is not ok!");
                        }
                    }
                }
                
            }
        }
        else
        {
            if (refToCanvasOfCity != null)
                refToCanvasOfCity.SetActive(true); //showing the name of the city (it was off to prevent UI touch - check the upper if)
        }
    }
    public override string ToString()
    {
        return $"{Id}";
    }
    public static void DestoryPartThatFollowMouse() //in case you press on x for example
    {
        Debug.Log("Check destory");
        if (PartThatFollowMouse != null)
            Destroy(PartThatFollowMouse);
    }
    public static bool DoesPartFollowNowMouse() => PartThatFollowMouse != null;

    public void UpdatePositionFromCreateMap(Vector3 newPosition)
    {
        
        follow = false;
        createdFromLoad = true;
        alreadyPositioned = true;
        transform.position = newPosition;
        int x = (int)(Math.Round(newPosition.x));
        int y = (int)(Math.Round(newPosition.y));
        if (PartThatFollowMouse == refToThisObject)
            PartThatFollowMouse = null;
        if (refToJunctionLightManagerScript != null)
            refToJunctionLightManagerScript.StartTrafficLightInJunction();
        (MyGrid.RefToMyGrid).AddPartToMap(refToThisObject, this, -y, x);
        Debug.Log($"My position is:{transform.position.x},{transform.position.y}");
    }


}
