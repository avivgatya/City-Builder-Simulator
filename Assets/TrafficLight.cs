using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    //this code manage the individual traffic light, everything that happend here is visually
    [SerializeField] private SpriteRenderer redLight;
    [SerializeField] private SpriteRenderer yellowLight;
    [SerializeField] private SpriteRenderer greenLight;
    [SerializeField] string startColor;
    private static Color redOff=new Color32(87,21,21,255);
    private static Color yellowOff = new Color32(113,73,17, 255);
    private static Color greenOff = new Color32(29, 51, 15, 255);

    private int delay = 1;
    public void RedToGreen() //the traffic light get commands for the manager, one of the for example is move to green color
    {
        yellowLight.color = Color.yellow;
        greenLight.color = greenOff;
        Invoke("GreenOn", delay);
    }
    public void GreenOn()
    {
        redLight.color = redOff;
        yellowLight.color = yellowOff;
        greenLight.color= Color.green;
    }
    public void GreenToRed()
    {
        greenLight.color = greenOff;
        StartCoroutine("GreenFlashingBeforeChange");
    }

    private IEnumerator GreenFlashingBeforeChange() // this function allow the green to flashing before changing
    {
        int cnt = 1;
        while(cnt<7)
        {
            greenLight.color = cnt % 2 == 1 ? greenOff : Color.green;
            cnt++;
            yield return new WaitForSeconds(0.6f);
        }
        if(cnt==7)
        {
            greenLight.color = greenOff;
            yellowLight.color = Color.yellow;
            cnt++;
            yield return new WaitForSeconds(3f);
        }
        if(cnt==8)
        {
            redLight.color = Color.red;
            yellowLight.color= yellowOff;
            yield return 0;
        }
    }
}
