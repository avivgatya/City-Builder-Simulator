using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePanel : MonoBehaviour
{
    //this code update the UI car panel with all the data
    [SerializeField] private new Text name;
    [SerializeField] private Text gender;
    [SerializeField] private Text age;
    [SerializeField] private Text destination;
    [SerializeField] private GameObject panelObject;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Image panelImage;
    private Transform carTransform;
    private static UpdatePanel refToScript;
    private static bool following = false;
    private int idImFollowing;
    
    private void Start()
    {
        refToScript = GetComponent<UpdatePanel>();
    }
    private void Update()
    {
        if(following)
        {
            Vector3 temp=carTransform.position;
            temp += new Vector3(-1.75f, 1, 0);
            transform.position = temp;
        }
    }

    public void UpdateAll(string name, string gender, int age,int id,string destination,Transform transformCar)
    {
        if (!following) 
        {
            panelObject.SetActive(true);
            this.name.text = name;
            this.gender.text = gender;
            if(gender=="male") panelImage.color=new Color32(124, 255, 245, 163);
            else panelImage.color = new Color32(255, 139, 195, 163);
            this.age.text = age.ToString();
            this.destination.text = destination;
            following = true;
            idImFollowing = id;
            carTransform = transformCar;
        }
        else
        {
            if (id == idImFollowing)
            {
                TurnPanelOff(-1);
            }
            else
            {
                this.name.text = name;
                this.gender.text = gender;
                if (gender == "male") panelImage.color = new Color32(124, 255, 245, 163);
                else panelImage.color = new Color32(255, 139, 195, 163);
                this.age.text = age.ToString();
                this.destination.text = destination;
                following = true;
                idImFollowing = id;
                carTransform = transformCar;
            }
        }
    }
    public void TurnPanelOff(int id)
    {
        Debug.Log("The id is: " + id);
        if (id == -1)
        {
            panelObject.SetActive(false);
            following = false;
            return;
        }
        if (id == idImFollowing&&following==true)
        {
            panelObject.SetActive(false);
            following = false;
        }
    }
    public static UpdatePanel GetRef() => refToScript;
    public static bool PanelOn() => following;
}
