using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarPanel : MonoBehaviour
{
    private string nameOfDriver;
    private string genderOfDriver;
    private int ageOfDriver;
    static private int allIDs=0;
    private int driverID;
    [SerializeField] private Transform transformOfTheCar;
    private string OriEnd="";
    private void Start()
    {
        ageOfDriver = Random.Range(18, 90);
        genderOfDriver = Random.value < 0.5f ? "male" : "female";
        nameOfDriver = GenerateRandomAmericanName();
        allIDs++;
        driverID = allIDs; 
    }
    private string GenerateRandomAmericanName()
    {
        string[] maleNames = { "James", "John", "Robert", "Michael", "William", "David", "Joseph", "Charles", "Thomas", "Daniel" };
        string[] femaleNames = { "Mary", "Jennifer", "Linda", "Patricia", "Elizabeth", "Susan", "Jessica", "Sarah", "Karen", "Nancy" };
        string[] surnames = {"Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor",
            "Anderson", "Thomas", "Jackson", "White", "Harris", "Clark", "Lewis", "Young", "Walker", "Hall"};
        string[] names =genderOfDriver=="male" ? maleNames : femaleNames;
        string firstName = names[Mathf.FloorToInt(Random.value * names.Length)];
        string lastName = surnames[Mathf.FloorToInt(Random.value * surnames.Length)];
        return firstName + " " + lastName;
    }
    public void UpdateUI()
    {
        UpdatePanel.GetRef().UpdateAll(nameOfDriver, genderOfDriver, ageOfDriver,driverID,OriEnd, transformOfTheCar);
    }
    public int GetCarId() => driverID;
    public void UpdateOriginEnd(string origin, string end) => OriEnd = origin + " > " + end;
}
