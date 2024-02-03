using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackLineRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private static TrackLineRenderer instance;
    public static TrackLineRenderer Instance { get => instance; set => instance = value; }
    Vector3[] positions;
    bool turnedOn = false;
    Car carScript;
    string wazeInstructions;
    private void Start()
    {
        instance = this; 
    }
    private void Update()
    {
        if (turnedOn)
        {
            int currentPointIndex = 1;
            int cntPoints = 1;//also counting the current position of the car.
            foreach (char c in wazeInstructions)
                if (c != 'c')
                    cntPoints++;
            cntPoints++;//also counting the destination point of the city.
            positions = new Vector3[cntPoints];
            positions[0] = carScript.transform.position;
            lineRenderer.positionCount = cntPoints;
            float zRotation =  carScript.transform.rotation.eulerAngles.z;

            while(wazeInstructions.Length > 1)
            {
                char command=wazeInstructions[0];
                Vector2 currentPosition =new Vector2(0,0);
                Vector2 positionNextJunction=new Vector2(0,0);
                positions[currentPointIndex] = carScript.transform.position;
                currentPointIndex++;
                wazeInstructions = wazeInstructions.Remove(0, 1);
            }
            lineRenderer.SetPositions(positions);
        }
    } 
    private void TurnLineOn(Car carScript) // Car is car script
    {
        turnedOn = true;
        wazeInstructions = carScript.WazeInstructions;
        this.carScript = carScript;
        lineRenderer.enabled = true;

        
        

    }
    private void TurnLineOff()
    {
        turnedOn = false;
        lineRenderer.enabled = false;
    }
}
