using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private string wazeInstructions; //string that represents that commands car should folloe
    private bool drive = false; //boolean value that helps with drive and stop functions
    private float velocity=0.25f;
    
    //those position are allow the car to spawn in correct position in the relevant city.
    Vector3 positionUpRight=new Vector3(0.14f,0,0);
    Vector3 positionUpLeft = new Vector3(0.06f, 0, 0);
    Vector3 positionLeftRight=new Vector3(0, 0.14f, 0);
    Vector3 positionLeftLeft=new Vector3(0, 0.06f, 0); 
    Vector3 positionDownRight=new Vector3(-0.14f,0 , 0);
    Vector3 positionDownLeft=new Vector3(-0.06f,0 , 0);
    Vector3 positionRightRight= new Vector3(0, -0.14f, 0);   
    Vector3 positionRightLeft= new Vector3(0, -0.06f, 0);
    public string WazeInstructions { get => wazeInstructions; set => wazeInstructions = value; }

    [SerializeField] private CarPanel carPanel; //allow car to communicate with the UI pannel
    private Vector3 positionToFix; //fixer system
    private Transform[] allPositionsOfFixers=new Transform[8];//fixer system

    public void StartMovingByInstructions(string newInstructions) //here that car start reading the "Waze" instructions to choose the next move
    {
        drive = true;
        WazeInstructions = newInstructions;
        switch (WazeInstructions[0])
        {
            case 'u': //case u for up which means north.
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                if (WazeInstructions[1] != 'c')
                {
                    if (WazeInstructions[1] == 'r' || WazeInstructions[1] == 'u')
                        transform.position += positionUpRight;
                    else if(WazeInstructions[1] == 'l')
                        transform.position += positionUpLeft;
                }
                else
                    transform.position += positionUpRight;
              
                break;
            case 'd': //down - south
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                if (WazeInstructions[1] != 'c')
                {
                    if (WazeInstructions[1] == 'l' || WazeInstructions[1] == 'd')
                        transform.position += positionDownRight;
                    else if (WazeInstructions[1] == 'r')
                        transform.position += positionDownLeft;
                }
                else
                    transform.position += positionDownRight;
                break;

            case 'r': //right - east
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                if (WazeInstructions[1] != 'c')
                {
                    if (WazeInstructions[1] == 'd' || WazeInstructions[1] == 'r')
                        transform.position += positionRightRight;
                    else if (WazeInstructions[1] == 'u')
                        transform.position += positionRightLeft;
                }
                else
                    transform.position += positionRightRight;
                break;
            case 'l': //left - west
                if (WazeInstructions[1] != 'c')
                {
                    if (WazeInstructions[1] == 'l' || WazeInstructions[1] == 'u')
                        transform.position += positionLeftRight;
                    else if (WazeInstructions[1] == 'd')
                        transform.position += positionLeftLeft;
                }
                else
                    transform.position += positionLeftRight;
                break;
        }
        WazeInstructions = WazeInstructions.Remove(0, 1);
    }
    private void Update() //this function runs every 1 frame and allows the car to drive every frame.
    {
        if (drive)
            transform.Translate(-velocity * Time.deltaTime, 0, 0);
    }
    public void Stop() => drive = false;
    public void Drive() => drive = true;
    public void TurnRightRightAndDrive() //right right meand the car drive to the right, to the right part of the road
    {
        drive = true;
        StartCoroutine("TurningRightRightProccess");
    }
    public void TurnRightLeftAndDrive() // rijght left means the car turn right but to the left part
    {
        drive = true;
        StartCoroutine("TurningRightLeftProccess");
    }

    public void TurnLeftLeftAndDrive() //turn left to the left part of the road
    {
        drive = true;
        StartCoroutine("TurningLeftLeftProccess");
    }
    public void TurnLeftRightAndDrive()// turn left to the right part of the road
    {
        drive = true;
        StartCoroutine("TurningLeftRightProccess");
    }
    private IEnumerator TurningRightRightProccess()
    {
        StopCoroutine("StoppingProccess");
        float anglesSpinned = 0;
        float target = 90;
        string whereCameFrom = "";
        int zValue = (int)transform.eulerAngles.z;
        Debug.Log("z value is: " + zValue);
        zValue = zValue % 360;
        if (zValue <= 0) zValue += 360;
        switch (zValue)
        {
            case 360: //Car drive from east
                whereCameFrom = "east";
                break;
            case 90://Car drive from north
                whereCameFrom = "north";
                break;
            case 180://Car drive from west
                whereCameFrom = "west";
                break;
            case 270://Car drive from south
                whereCameFrom = "south";
                break;
        }
        Debug.Log("Came from " + whereCameFrom);
        while (anglesSpinned<target)
        {
            float amount = -69f * Time.deltaTime; //perfect
            transform.Rotate(0, 0, amount);
            anglesSpinned -= amount;
            yield return null;
        }
        switch (whereCameFrom)
        {
            case "east": //Car drive from east-
                positionToFix = allPositionsOfFixers[2].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 270);
                transform.position = new Vector3(positionToFix.x, transform.position.y, transform.position.z);
                break;
            case "north"://Car drive from north
                Debug.Log("right-right come from north");
                positionToFix = allPositionsOfFixers[4].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                transform.position = new Vector3(transform.position.x, positionToFix.y, transform.position.z);
                break;
            case "west"://Car drive from west
                positionToFix = allPositionsOfFixers[6].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90);
                transform.position = new Vector3(positionToFix.x, transform.position.y, transform.position.z);
                break;
            case "south"://Car drive from south
                positionToFix = allPositionsOfFixers[0].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180);
                transform.position = new Vector3(transform.position.x, positionToFix.y, transform.position.z);
                break;
        }
    }
    private IEnumerator TurningRightLeftProccess()
    {
        string whereCameFrom = "";
        int zValue = (int)transform.eulerAngles.z;
        Debug.Log("z value is: " + zValue);
        zValue = zValue % 360;
        if (zValue <= 0) zValue += 360;
        switch (zValue)
        {
            case 360: //Car drive from east
                whereCameFrom = "east";
                break;
            case 90://Car drive from north
                whereCameFrom = "north";
                break;
            case 180://Car drive from west
                whereCameFrom = "west";
                break;
            case 270://Car drive from south
                whereCameFrom = "south";
                break;
        }
        // Debug.Log("Drive Right function!");
        StopCoroutine("StoppingProccess");
        float anglesSpinned = 0;
        float target = 90;
        while (anglesSpinned < target)
        {
            float amount = -48f * Time.deltaTime;
            transform.Rotate(0, 0, amount);
            anglesSpinned -= amount;
            yield return null;
        }
        switch (whereCameFrom)
        {
            case "east": //Car drive from east-
                positionToFix = allPositionsOfFixers[3].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 270);
                transform.position = new Vector3(positionToFix.x, transform.position.y, transform.position.z);
                break;
            case "north"://Car drive from north
                positionToFix = allPositionsOfFixers[5].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                transform.position = new Vector3(transform.position.x, positionToFix.y, transform.position.z);
                break;
            case "west"://Car drive from west
                positionToFix = allPositionsOfFixers[7].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90);
                transform.position = new Vector3(positionToFix.x, transform.position.y, transform.position.z);
                break;
            case "south"://Car drive from south
                positionToFix = allPositionsOfFixers[1].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180);
                transform.position = new Vector3(transform.position.x, positionToFix.y, transform.position.z);
                break;
        }
    }
    private IEnumerator TurningLeftLeftProccess()
    {
        string whereCameFrom = "";
        int zValue = (int)transform.eulerAngles.z;
        Debug.Log("z value is: " + zValue);
        zValue = zValue % 360;
        if (zValue <= 0) zValue += 360;
        switch (zValue)
        {
            case 360: //Car drive from east
                whereCameFrom = "east";
                break;
            case 90://Car drive from north
                whereCameFrom = "north";
                break;
            case 180://Car drive from west
                whereCameFrom = "west";
                break;
            case 270://Car drive from south
                whereCameFrom = "south";
                break;
        }
        StopCoroutine("StoppingProccess");
        float anglesSpinned = 0;
        float target = 90;

        while (anglesSpinned < target)
        {
            float amount = +34.5f * Time.deltaTime;
            transform.Rotate(0, 0, amount);
            anglesSpinned += amount;
            yield return null;
        }
        switch (whereCameFrom)
        {
            case "east": //Car drive from east-
                positionToFix = allPositionsOfFixers[7].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90);
                transform.position = new Vector3(positionToFix.x, transform.position.y, transform.position.z);
                break;
            case "north"://Car drive from north
                positionToFix = allPositionsOfFixers[1].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180);
                transform.position = new Vector3(transform.position.x, positionToFix.y, transform.position.z);
                break;
            case "west"://Car drive from west
                positionToFix = allPositionsOfFixers[3].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 270);
                transform.position = new Vector3(positionToFix.x, transform.position.y, transform.position.z);
                break;
            case "south"://Car drive from south
                positionToFix = allPositionsOfFixers[5].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                transform.position = new Vector3(transform.position.x, positionToFix.y, transform.position.z);
                break;
        }
    }
    private IEnumerator TurningLeftRightProccess()
    {
        string whereCameFrom = "";
        int zValue = (int)transform.eulerAngles.z;
        Debug.Log("z value is: " + zValue);
        zValue = zValue % 360;
        if (zValue <= 0) zValue += 360;
        switch (zValue)
        {
            case 360: //Car drive from east
                whereCameFrom = "east";
                break;
            case 90://Car drive from north
                whereCameFrom = "north";
                break;
            case 180://Car drive from west
                whereCameFrom = "west";
                break;
            case 270://Car drive from south
                whereCameFrom = "south";
                break;
        }

        StopCoroutine("StoppingProccess");
        float anglesSpinned = 0;
        float target = 90;

        while (anglesSpinned < target)
        {
            float amount = 28f * Time.deltaTime;
            transform.Rotate(0, 0, amount);
            anglesSpinned += amount;
            yield return null;
        }
        //6 0 2 4
        switch (whereCameFrom)
        {
            case "east": //Car drive from east-
                positionToFix = allPositionsOfFixers[6].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90);
                transform.position = new Vector3(positionToFix.x, transform.position.y, transform.position.z);
                break;
            case "north"://Car drive from north
                positionToFix = allPositionsOfFixers[0].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180);
                transform.position = new Vector3(transform.position.x, positionToFix.y, transform.position.z);
                break;
            case "west"://Car drive from west
                positionToFix = allPositionsOfFixers[2].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 270);
                transform.position = new Vector3(positionToFix.x, transform.position.y, transform.position.z);
                break;
            case "south"://Car drive from south
                positionToFix = allPositionsOfFixers[4].position;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                transform.position = new Vector3(transform.position.x, positionToFix.y, transform.position.z);
                break;
        }
    }
    public char PopTheNextInstruction() //read the next waze instruction and delete it (pop)
    {
        char c= WazeInstructions[0];
        WazeInstructions = WazeInstructions.Remove(0, 1);
        return c;
    }
    public char GetNextInstruction()=> WazeInstructions[0]; //only get the instruction without pope
    //functions that allow reference to delegates.
    public TrafficIsGreenNow_Delegate DriveRef()=>new TrafficIsGreenNow_Delegate(Drive);
    public TrafficIsGreenNow_Delegate RightRightDriveRef()=>new TrafficIsGreenNow_Delegate(TurnRightRightAndDrive);
    public TrafficIsGreenNow_Delegate RightLeftDriveRef()=>new TrafficIsGreenNow_Delegate(TurnRightLeftAndDrive);
    public TrafficIsGreenNow_Delegate LeftRightDriveRef() => new TrafficIsGreenNow_Delegate(TurnLeftRightAndDrive);
    public TrafficIsGreenNow_Delegate LeftLeftDriveRef()=>new TrafficIsGreenNow_Delegate(TurnLeftLeftAndDrive);
    public void DestroyMe() //when car arrive city
    {
        int tempID = carPanel.GetCarId();
        UpdatePanel.GetRef().TurnPanelOff(tempID);
        Destroy(gameObject);
    }
    public void StopTurningWhenEnterFix() //this happend when the car pass the junction and arrive in the fixer position, it should stop turning
    {
        StopCoroutine("TurningRightRightProccess");
        StopCoroutine("TurningRightLeftProccess");
        StopCoroutine("TurningLeftLeftProccess");
        StopCoroutine("TurningLeftRightProccess");
    }
    public void SetPositionsFixers(Transform []arr)
    {
        allPositionsOfFixers = arr;
    }
}

