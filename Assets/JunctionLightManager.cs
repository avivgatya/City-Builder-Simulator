using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TrafficIsGreenNow_Delegate();
public class JunctionLightManager : MonoBehaviour
{
    //all 8 traffic lights
    [SerializeField] private TrafficLight EastWestStraight;
    [SerializeField] private TrafficLight EastWestLeft;
    [SerializeField] private TrafficLight NorthSouthStraight;
    [SerializeField] private TrafficLight NorthSouthLeft;
    [SerializeField] private TrafficLight WestEastStraight;
    [SerializeField] private TrafficLight WestEastLeft;
    [SerializeField] private TrafficLight SouthNorthStraight;
    [SerializeField] private TrafficLight SouthNorthLeft;

    private int trafficStatus = 0;
    private char step = 'a';// in each status there are 2 steps: a and b and c.  a means we turning red off (green is blinking), b means green off but red is not yet green, and c is green

    private TrafficIsGreenNow_Delegate[] trafficLights_Delegates;
    [SerializeField] private Transform[] positionsOfFixers=new Transform[8]; //all fixers in the junction
    public TrafficIsGreenNow_Delegate[] TrafficLights_Delegates { get => trafficLights_Delegates; set => trafficLights_Delegates = value; }
    public void StartTrafficLightInJunction() //when juction is created some initialization
    {
        TrafficLights_Delegates = new TrafficIsGreenNow_Delegate[8]; // array of delegates
        InvokeRepeating("ChangeTrafficLight", 0f, 15f);
    }
    private void ChangeTrafficLight() //in this function some traffic lights become red (visually)
    {
        step = 'a';
        trafficStatus++;
        if (trafficStatus == 5) trafficStatus = 1;
        switch (trafficStatus)
        {
            case 1:
                EastWestLeft.GreenToRed();
                WestEastLeft.GreenToRed();
                break;
            case 2:
                NorthSouthStraight.GreenToRed();
                SouthNorthStraight.GreenToRed();
                break;
            case 3:
                EastWestStraight.GreenToRed();
                WestEastStraight.GreenToRed();
                break;
            case 4:
                NorthSouthLeft.GreenToRed();
                SouthNorthLeft.GreenToRed();
                break;
        }
        Invoke("ChangeTrafficLight_InvokeVersion", 7f);
    }
    private void ChangeTrafficLight_InvokeVersion() // after some of traffic light become red, others allow to become visually green
    {
        step = 'b';
        switch (trafficStatus)
        {
            case 1: //3 and 8 become green
                NorthSouthStraight.RedToGreen();
                SouthNorthStraight.RedToGreen();
                break;
            case 2://1 and 6 become green
                EastWestStraight.RedToGreen();
                WestEastStraight.RedToGreen();
                break;
            case 3://4 and 7
                NorthSouthLeft.RedToGreen();
                SouthNorthLeft.RedToGreen();
                break;
            case 4://2 and 5
                EastWestLeft.RedToGreen();
                WestEastLeft.RedToGreen();
                break;
        }
        Invoke("StepC", 2f);
    }
    private void StepC() //it takes 2 seconds to traffic light become green, and then delegated invoked and allow car to drive
    {
        step = 'c';
        switch (trafficStatus)
        {
            case 1: //3 and 8 become green
                if (TrafficLights_Delegates[2] != null)
                {
                    if (TrafficLights_Delegates[2] != null)
                    {
                        TrafficLights_Delegates[2].Invoke();
                    }
                        trafficLights_Delegates[2] = null;
                }
                if (TrafficLights_Delegates[7] != null)
                {
                    if(trafficLights_Delegates[7] != null)
                        TrafficLights_Delegates[7].Invoke();
                    trafficLights_Delegates[7] = null;
                }
                break;
            case 2://1 and 6 become green
                if (TrafficLights_Delegates[0] != null)
                {
                    if (trafficLights_Delegates[0] != null)
                        TrafficLights_Delegates[0].Invoke();
                    trafficLights_Delegates[0] = null;
                }
                if (TrafficLights_Delegates[5] != null)
                {
                    if (trafficLights_Delegates[5] != null)
                        TrafficLights_Delegates[5].Invoke();
                    trafficLights_Delegates[5] = null;
                }
                break;
            case 3://4 and 7
                if (TrafficLights_Delegates[3] != null)
                {
                    if (trafficLights_Delegates[3] != null)
                        TrafficLights_Delegates[3].Invoke();
                    trafficLights_Delegates[3] = null;
                }
                if (TrafficLights_Delegates[6] != null)
                {
                    if (trafficLights_Delegates[6] != null)
                        TrafficLights_Delegates[6].Invoke();
                    trafficLights_Delegates[6] = null;
                }

                break;
            case 4://2 and 5
                if (TrafficLights_Delegates[1] != null)
                {
                    if (trafficLights_Delegates[1] != null)
                        TrafficLights_Delegates[1].Invoke();
                    trafficLights_Delegates[1] = null;
                }
                if (TrafficLights_Delegates[4] != null)
                {
                    if (trafficLights_Delegates[4] != null)
                        TrafficLights_Delegates[4].Invoke();
                    trafficLights_Delegates[4] = null;
                }
                break;
        }
    }
    /* all trafficLights ID's according to their location.
    43||21
    __  __
    __  __
    56||78
     */
    public bool GetStatusOfSpecificTrafficLight(int num) //this function allow traffic manager know which traffic light instrested in which car
    {
        switch(num)
        {
            case 1: return trafficStatus == 2 && step=='c'? true : false;
            case 2: return trafficStatus == 4 && step == 'c' ? true : false; 
            case 3: return trafficStatus == 1 && step == 'c' ? true : false;
            case 4: return trafficStatus == 3 && step == 'c' ? true : false;
            case 5: return trafficStatus == 4 && step == 'c' ? true : false;
            case 6: return trafficStatus == 2 && step == 'c' ? true : false;
            case 7: return trafficStatus == 3 && step == 'c' ? true : false;
            case 8: return trafficStatus == 1 && step == 'c' ? true : false;
        }
        //should never get here, no need breaks in this switch, always return the in the relevant case!
        Debug.Log("If you see this you have problem in JunctionLightManager script in function: GetStatusOfSpecificTrafficLight");
        return false;
    }
    public Transform[] GetArrayOfFixersPositions() => positionsOfFixers;
}
