using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCollider : MonoBehaviour
{
    [SerializeField] private Car carScript;
    [SerializeField] private Transform transformOfCar;
    private JunctionLightManager refTolightManager;
    private bool OutOfTheCity = false;
    private int cntExit = 0;

    void OnTriggerEnter2D(Collider2D col)
    {
        char instruction;
        char nextInstruction;
        bool status;
        if (col.gameObject.tag == "fixer" || col.gameObject.tag == "car" ) return; //front collider dont care about colliding with itself (car) and with other fixers.
        if (col.gameObject.tag == "City") 
        {
            if (carScript.GetNextInstruction() == 'c' && OutOfTheCity) //means we arrived in our destination
                carScript.DestroyMe();
        }
        else
        {
            if (col.gameObject.tag == "vehicleCollider")
                carScript.Stop();
            else if (refTolightManager == null) //make sure we wont collide with the other side of the junction!
            {
                refTolightManager = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript();
                carScript.SetPositionsFixers(refTolightManager.GetArrayOfFixersPositions());
                instruction = carScript.PopTheNextInstruction(); //in any case of collider.
                nextInstruction = carScript.GetNextInstruction();
                //here we divide our work to 4, north east west and south are our colliders, and each collider connect the car functions to the TrafficLightManager delegates.
                switch (col.gameObject.tag)
                {
                    /////////////////////////EAST//////////////////////////////////
                    case "eastCollider":
                        if (instruction == 'u') // up in the table means right turn for the car
                        {
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(1);
                            if (!status)
                            {
                                carScript.Stop();
                                if(nextInstruction=='c' || nextInstruction=='u'|| nextInstruction == 'r')
                                    refTolightManager.TrafficLights_Delegates[0] += carScript.RightRightDriveRef();
                                else
                                    refTolightManager.TrafficLights_Delegates[0] += carScript.RightLeftDriveRef();
                                
                            }
                            else
                            {
                                if (nextInstruction == 'c' || nextInstruction == 'u' || nextInstruction == 'r')
                                    carScript.TurnRightRightAndDrive();
                                else
                                    carScript.TurnRightLeftAndDrive();
                            }
                        }
                        else if (instruction == 'l')
                        {
                            //instruction is left
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(1);
                            if (!status)
                            {
                                carScript.Stop();
                                refTolightManager.TrafficLights_Delegates[0] += carScript.DriveRef();
                            }
                            else
                            {
                                carScript.Drive();
                            }
                        }
                        else if (instruction == 'd')
                        {
                           //instruction is down
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(2);
                            if (!status)
                            {
                                carScript.Stop();
                                if (nextInstruction == 'l' || nextInstruction == 'd' || nextInstruction == 'c')
                                    refTolightManager.TrafficLights_Delegates[1] += carScript.LeftRightDriveRef();
                                else
                                    refTolightManager.TrafficLights_Delegates[1] += carScript.LeftLeftDriveRef();
                            }
                            else
                            {
                                if (nextInstruction == 'l' || nextInstruction == 'd' || nextInstruction == 'c')
                                    carScript.TurnLeftRightAndDrive();
                                else
                                    carScript.TurnLeftLeftAndDrive();
                            }
                        }
                        break;
                    /////////////////////////WEST//////////////////////////////////
                    case "westCollider":
                        if (instruction == 'u') // up in the table means right turn for the car
                        {
                           //instruction is up
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(5);
                            if (!status)
                            {
                                carScript.Stop();
                                if (nextInstruction == 'c' || nextInstruction == 'r' || nextInstruction == 'u')
                                    refTolightManager.TrafficLights_Delegates[4] += carScript.LeftRightDriveRef();
                                else
                                    refTolightManager.TrafficLights_Delegates[4] += carScript.LeftLeftDriveRef();
                            }
                            else
                            {
                                if (nextInstruction == 'c' || nextInstruction == 'r' || nextInstruction == 'u')
                                    carScript.TurnLeftRightAndDrive();
                                else
                                    carScript.TurnLeftLeftAndDrive();
                            }

                        }
                        else if (instruction == 'r')
                        {
                            //instruction is right
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(6);
                            if (!status)
                            {
                                carScript.Stop();
                                refTolightManager.TrafficLights_Delegates[5] += carScript.DriveRef();
                            }
                            else
                            {
                                carScript.Drive();
                            }
                        }
                        else if (instruction == 'd')
                        {
                           //instruction is down
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(6);
                            if (!status)
                            {
                                carScript.Stop();
                                if (nextInstruction == 'c' || nextInstruction == 'l' || nextInstruction == 'd')
                                    refTolightManager.TrafficLights_Delegates[5] += carScript.RightRightDriveRef();
                                else
                                    refTolightManager.TrafficLights_Delegates[5] += carScript.RightLeftDriveRef();
                            }
                            else
                            {
                                if (nextInstruction == 'c' || nextInstruction == 'l' || nextInstruction == 'd')
                                    carScript.TurnRightRightAndDrive();
                                else
                                    carScript.TurnRightLeftAndDrive();
                            }
                        }
                        //
                        break;
                        /////////////////////////NORTH//////////////////////////////////
                    case "northCollider":
                        //Entered north collider
                        if (instruction == 'l') // up in the table means right turn for the car
                        {
                           //instruction is left (car should turn right)
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(3);
                            if (!status)
                            {
                                carScript.Stop();
                                if (nextInstruction == 'c' || nextInstruction == 'u' || nextInstruction == 'l')
                                    refTolightManager.TrafficLights_Delegates[2] += carScript.RightRightDriveRef();
                                else
                                    refTolightManager.TrafficLights_Delegates[2] += carScript.RightLeftDriveRef();
                            }
                            else
                            {
                                if (nextInstruction == 'c' || nextInstruction == 'u' || nextInstruction == 'l')
                                    carScript.TurnRightRightAndDrive();
                                else
                                    carScript.TurnRightLeftAndDrive();

                            }

                        }
                        else if (instruction == 'r')
                        {
                           //instruction is right
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(4);
                            if (!status)
                            {
                                carScript.Stop();
                                if (nextInstruction == 'c' || nextInstruction == 'd' || nextInstruction == 'r')
                                    refTolightManager.TrafficLights_Delegates[3] += carScript.LeftRightDriveRef();
                                else
                                    refTolightManager.TrafficLights_Delegates[3] += carScript.LeftLeftDriveRef();
                            }
                            else
                            {
                                if (nextInstruction == 'c' || nextInstruction == 'd' || nextInstruction == 'r')
                                    carScript.TurnLeftRightAndDrive();
                                else
                                    carScript.TurnLeftLeftAndDrive();
                            }
                        }
                        else if (instruction == 'd')
                        {
                         //instruction is down
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(3);
                            if (!status)
                            {
                                carScript.Stop();
                                refTolightManager.TrafficLights_Delegates[2] += carScript.DriveRef();
                            }
                            else
                            {
                                carScript.Drive();
                            }
                        }
                        //
                        break;
                    /////////////////////////SOUTH//////////////////////////////////
                    case "southCollider":
                    //Entered south collider
                        if (instruction == 'l') // up in the table means right turn for the car
                        {
                           //instruction is left
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(7);
                            if (!status)
                            {
                                carScript.Stop();
                                if (nextInstruction == 'c' || nextInstruction == 'u' || nextInstruction == 'l')
                                    refTolightManager.TrafficLights_Delegates[6] += carScript.LeftRightDriveRef();
                                else
                                    refTolightManager.TrafficLights_Delegates[6] += carScript.LeftLeftDriveRef();
                            }
                            else
                            {
                                if (nextInstruction == 'c' || nextInstruction == 'u' || nextInstruction == 'l')
                                    carScript.TurnLeftRightAndDrive();
                                else
                                    carScript.TurnLeftLeftAndDrive();
                            }

                        }
                        else if (instruction == 'r')
                        {
                           //instruction is right
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(8);
                            if (!status)
                            {
                                carScript.Stop();
                                if (nextInstruction == 'c' || nextInstruction == 'd' || nextInstruction == 'r')
                                    refTolightManager.TrafficLights_Delegates[7] += carScript.RightRightDriveRef();
                                else
                                    refTolightManager.TrafficLights_Delegates[7] += carScript.RightLeftDriveRef();
                            }
                            else
                            {
                                if (nextInstruction == 'c' || nextInstruction == 'd' || nextInstruction == 'r')
                                    carScript.TurnRightRightAndDrive();
                                else
                                    carScript.TurnRightLeftAndDrive();

                            }
                        }
                        else if (instruction == 'u')
                        {
                           //instruction is up
                            status = col.GetComponent<junctionCollider>().GetTheJunctionManagerScript().GetStatusOfSpecificTrafficLight(8);
                            if (!status)
                            {
                                carScript.Stop();
                                refTolightManager.TrafficLights_Delegates[7] += carScript.DriveRef();
                            }
                            else
                            {
                                carScript.Drive();
                            }
                        }
                        break;
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D col) //this functionn allow the collide and check the object it collide with with the tag name so we can be sure we collided the correct object
    {
        if (col.gameObject.tag == "vehicleCollider")
            Invoke("StartDriveAfterNextCar", 0.2f);
        if (col.gameObject.tag == "City")
            OutOfTheCity = true;
        if (col.gameObject.tag == "eastCollider" || col.gameObject.tag == "westCollider" || col.gameObject.tag == "northCollider" || col.gameObject.tag == "southCollider")
        {
            cntExit++;
            if (cntExit == 2)
            {
                refTolightManager = null;
                cntExit = 0;
            }
        }
    }
    public void StartDriveAfterNextCar()=> carScript.Drive();
    public Transform GetCarTransform() => transformOfCar;
}
