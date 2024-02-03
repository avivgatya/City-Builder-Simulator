using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class CreateMap : MonoBehaviour
{
    public GameObject junction;
    public GameObject horizontal;
    public GameObject vertical;
    public GameObject city;
    public MyGrid gridScript;
    [SerializeField] private InputField inputField;
    [SerializeField] private Dropdown dropdownField;
    private char[,] symbolsInMap = {
    { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' },
    { '0', '0', '0', '0', '0', '0', '0', '0', 'c', '0', '0', '0', '0', '0', '0', '0', '0' },
    { '0', '0', '0', '0', '0', '0', '0', '0', 'v', '0', '0', '0', '0', '0', '0', '0', '0' },
    { '0', '0', '0', '0', '0', 'c', 'h', 'h', 'j', 'h', 'h', 'c', '0', '0', '0', '0', '0' },
    { '0', '0', '0', '0', '0', '0', '0', '0', 'v', '0', '0', '0', '0', '0', '0', '0', '0' },
    { '0', '0', '0', '0', '0', '0', '0', '0', 'c', '0', '0', '0', '0', '0', '0', '0', '0' },
    { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' },
};
    private void Start()
    {
        dropdownField.ClearOptions();
    }
    public void CreateMapFunc() // here we create the map from the array which will update later.
    {
        for (int i = 0; i < symbolsInMap.GetLength(0); i++)
            for (int j = 0; j < symbolsInMap.GetLength(1); j++)
            {
                switch (symbolsInMap[i, j])
                {
                    case 'h':
                        GameObject tmp = Instantiate(horizontal, new Vector3(j, -i, -0.1f), Quaternion.identity);
                        tmp.GetComponent<Part>().UpdatePositionFromCreateMap(new Vector3(j, -i, -0.1f));
                        break;
                    case 'v':
                        GameObject tmp2 = Instantiate(vertical, new Vector3(j, -i, -0.1f), Quaternion.identity);
                        tmp2.GetComponent<Part>().UpdatePositionFromCreateMap(new Vector3(j, -i, -0.1f));
                        break;
                    case 'j':
                        GameObject tmp3 = Instantiate(junction, new Vector3(j, -i, -0.1f), Quaternion.identity);
                        tmp3.GetComponent<Part>().UpdatePositionFromCreateMap(new Vector3(j, -i, -0.1f));
                        break;
                    case 'c':
                        GameObject tmp4 = Instantiate(city, new Vector3(j, -i, -0.1f), Quaternion.identity);
                        tmp4.GetComponent<Part>().UpdatePositionFromCreateMap(new Vector3(j, -i, -0.1f));
                        break;
                }
            }
    }

    public void SaveMap() //take the 2d array to text
    {
        string nameOfMap = inputField.text;
        Debug.Log($"saving {nameOfMap}...");
        char[,] tmpSymbolsInMap = gridScript.OutputSymbolsInMap();
        File.WriteAllText(Application.dataPath + "/MyCustomMaps/" + nameOfMap+".txt",Char2DArrayaToStringArr(tmpSymbolsInMap));
    }
    public void LoadFilesNames() //for the UI in the top bar
    {
        dropdownField.ClearOptions();
        dropdownField.AddOptions(GetAllFileNamesInDirectory());
    }
    public void LoadMap() // load file from text
    {
        gridScript.ResetAll(); //before loading map we should resetall...
        string nameOfMap = dropdownField.options[dropdownField.value].text;
        Debug.Log("loading...");
        string tmp = File.ReadAllText(Application.dataPath + "/MyCustomMaps/" + nameOfMap);
        symbolsInMap = StringToChar2DArray(tmp);
        CreateMapFunc();
    }
    private string Char2DArrayaToStringArr(char[,] arr) // for the text store
    {
        int rows = arr.GetLength(0);
        int cols = arr.GetLength(1);

        string result = "";
        // Convert each element of the 2D array to a string
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result += arr[i, j];
            }
            // Add a new line at the end of each row
            result += "\n";
        }
        //Debug.Log("result is:\n" + result);

        return result;
    }
    private char[,] StringToChar2DArray(string str) //same as upper function
    {
        char[,] result = new char[7, 17];
        string[] allLines = str.Split('\n');
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 17; j++)
            {
                result[i, j] = (allLines[i][j]);
            } 
        }
        foreach (char c in result)
            Debug.Log(c);
        return result;
    }
    private List<string> GetAllFileNamesInDirectory() //for top bar UI
    {
         List<string> fileNamesList = new List<string>();
         string directoryPath= Application.dataPath + "/MyCustomMaps/";
        if (Directory.Exists(directoryPath))
        {
            string[] filePaths = Directory.GetFiles(directoryPath);

            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                if(fileName.Split('.').Length==2)
                    fileNamesList.Add(fileName);
            }

            // Display the list of file names (for demonstration purposes)
            foreach (string fileName in fileNamesList)
            {
                Debug.Log(fileName);
            }
        }
        else
        {
            Debug.LogError("Directory not found: " + directoryPath);
        }
        return fileNamesList;
    }
}
