using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rndVehicleImg : MonoBehaviour 
{ 
    public Sprite[] vehicles= new Sprite[8];
    
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        float res = Random.Range(0f, 1f);
        if(res<0.1f)
            spriteRenderer.sprite = vehicles[0];
        else if (res<0.2f)
            spriteRenderer.sprite = vehicles[1];
        else if (res < 0.3f)
            spriteRenderer.sprite = vehicles[2];
        else if (res < 0.4f)
            spriteRenderer.sprite = vehicles[3];
        else if (res < 0.5f)
            spriteRenderer.sprite = vehicles[4];
        else if (res < 0.6f)
            spriteRenderer.sprite = vehicles[5];
        else if (res < 0.7f)
            spriteRenderer.sprite = vehicles[6];
        else if (res < 0.8f)
            spriteRenderer.sprite = vehicles[7];
    }
}
