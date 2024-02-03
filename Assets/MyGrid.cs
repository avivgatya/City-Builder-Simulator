using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid: MonoBehaviour
{
    GameObject [,] mapOfObjects;
    Part[,] mapOfScripts;
    char[,] symbolsInMap; //j,h,v,c - this is the main map
    static MyGrid refToMyGrid;
    private int width = 17; //width of map
    private int height= 7; //height of map
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject truckPrefab;
    [SerializeField] private GameObject busPrefab;
    private char[,] ReqursionVisitedArr;

    private bool infinity = false;
    public static MyGrid RefToMyGrid { get => refToMyGrid; set => refToMyGrid = value; }
    private List<string> cityNames = new List<string>(); //all cities namesa
    public struct Index // there is list of cities that stored in struct, for 2 positions
    {
        public int cityRow;
        public int cityCol;
        public Index(int r, int c) { cityRow = r; cityCol = c; }
    }

    private List<Index> allCitiesInMap = new List<Index>(); // this it list of all cities I created in the map
    private List<string> citiesOrder = new List<string>(); // full explanation in the function

    private void Start() 
    {
        CityNameGenerator(); // function that generate cities names
        refToMyGrid = this;
        mapOfObjects = new GameObject[height,width];
        mapOfScripts = new Part[height,width];
        symbolsInMap = new char[height,width];
    }

    public void AddPartToMap(GameObject newObj,Part newPart,int row,int col) // in this case after we put the part in the map it should update the symbols map
    {
        AllParts.AddObjectToList(newObj);
        if(newObj.CompareTag("City"))
        {
            allCitiesInMap.Add(new Index(row,col));
            string tempName = GetCityName();
            citiesOrder.Add(tempName);
            newObj.GetComponent<NameCityUpdate>().UpdateName(tempName);
            symbolsInMap[row, col] = 'c';
        }
        else if(newObj.CompareTag("horizontal"))
            symbolsInMap[row, col] = 'h';
        else if(newObj.CompareTag("vertical"))
            symbolsInMap[row, col] = 'v';
        else if (newObj.CompareTag("junction"))
            symbolsInMap[row, col] = 'j';
        mapOfObjects[row,col]=newObj;
        mapOfScripts[row,col]=newPart;
        string temp = "";
        for (int i = 0; i < symbolsInMap.GetLength(0); i++)
        {
            for (int j = 0; j < symbolsInMap.GetLength(1); j++)
                temp += symbolsInMap[i, j]=='\0'?'0': symbolsInMap[i, j];
            temp += '\n';
        }
    }
    public bool CheckPositionInsideMap(int row, int col) => ((row< mapOfObjects.GetLength(0)&&row>=0) && (col< mapOfObjects.GetLength(1)&&col>=0)); //check its ok
    public bool CheckPositionEmpty(int row, int col) => mapOfObjects[row, col] == null; // prevent put part on part
    public void PrintToConsoleAllParts() //only for debugging
    {
        string result = "";
        for (int i = 0; i < mapOfScripts.GetLength(0); i++)
        {
            for (int j = 0; j < mapOfScripts.GetLength(1); j++)
            {
                if (mapOfScripts[i, j] == null)
                    result += "O ";
                else result += mapOfScripts[i, j].ToString()+" ";
            }
            result += "\n";
        }
    }

    [System.Obsolete] //for random function
    public void CreateRandomCar() // this is one of the most important functions, here we create the car with all the relevant data
    {
        ReqursionVisitedArr=new char[height,width];
        Random.seed = (int)System.DateTime.Now.Ticks; // for function
        int origin=-1;
        int end=-1;
        string track = "x";
        if (allCitiesInMap.Count > 1) //check if there are cities in map, if no cities, car will not create.
        {
            while (track == "x") // track is the way car should do
            {
                origin = Random.Range(0, allCitiesInMap.Count); // here we choose random city of the cities we placed on map
                end = origin; // at start end and origin are the same, so if there is only 1 city, it will be like it already arrived, until new end will be decided.

                while (end == origin) // this function make sure end and origin will not stay the same and gives the end other city, it will keep run infinity to make sure end !=origin
                { // this function will not really run for infinity because we checked in the upper part that there is more than 1 city, so there must be other option.
                    Random.seed = (int)System.DateTime.Now.Ticks;
                    end = Random.Range(0, allCitiesInMap.Count);
                }
                bool[,] visited = new bool[height, width]; //this is important part that prevent stack over flow, in the Algorythm of reqursion, we should always prevent Algorythm to travel to tiles it met before.
                //full explanation of the following line will be in the function, in short, this function check the shortest way from city A, to city B
                track=ShortestTrackFromAToB2('\0',visited, allCitiesInMap[origin].cityRow, allCitiesInMap[origin].cityCol, allCitiesInMap[end].cityRow, allCitiesInMap[end].cityCol, true);
            }
            //after the car knows the way it should do, we need to create it according to position and every data.
            Vector3 startPosition = new Vector3(allCitiesInMap[origin].cityCol, -allCitiesInMap[origin].cityRow, -0.2f);
            GameObject instanceOfCar;
            instanceOfCar = Instantiate(carPrefab, startPosition, Quaternion.identity);
            AllParts.AddObjectToList(instanceOfCar);
            Vector3[] positionsForWaze = CalculatePositionsForWaze(track, startPosition);
            instanceOfCar.GetComponent<Car>().StartMovingByInstructions(track);
            instanceOfCar.GetComponent<CarPanel>().UpdateOriginEnd(citiesOrder[origin],citiesOrder[end]);
        }
    }
    public void InfinityCars() //this function allow create infinity random car every 1.5 seconds
    {
        if (!infinity)
            InvokeRepeating("CreateRandomCar", 0f, 1.5f);
        else
            CancelInvoke("CreateRandomCar");
        infinity = !infinity;
    }
    private Vector3[] CalculatePositionsForWaze(string track,Vector3 startPosition)
    {
        return new Vector3[5];
    }
    //this is the most important part of the calculations, here we build reqursion function that calculate th shortest way from city A to B
    private string ShortestTrackFromAToB2(char directionICameFrom,bool[,] visited, int ro, int co, int rd, int cd, bool firstIteration) //directions: r,l,u,d(right,left,up,down)
    {

        //prevent get out of map bounderies
        if (ro < 0 || ro >= height) return "outSideBounderies";
        if (co < 0 || co >= width) return "outSideBounderies";

        if (visited[ro, co]) return "visited"; //first of all, I check if i already visited this tile to prevent Stack over flow.
        visited[ro, co] = true; //If we got here, we have never been here before, so we need to make sure not to come here again
        bool[,] copy1 = visited.Clone() as bool[,]; //clones of the array for all the instances of the requirsion function
        bool[,] copy2 = visited.Clone() as bool[,];
        bool[,] copy3 = visited.Clone() as bool[,];
        bool[,] copy4 = visited.Clone() as bool[,];

        if (firstIteration == false && symbolsInMap[ro, co] == 'c')//means we found a city (in first iteration we find a city also so we prevent it)
        {
            if (ro == rd && co == cd) //means we found our destination city
            {
                return "c"; // for the waze commands
            }
            else
            {
                return "0"; //no found

            }
        }
        if (symbolsInMap[ro, co] == '\0') return "0"; //empty cell, no need to check this direction anymore
        string shortestWay = "x";
        string upInstructions = "";
        string leftInstructions = "";
        string rightInstructions = "";
        string downInstructions = "";
        //here we make the 4 instances of the reqursion
        if (directionICameFrom != 'u' && symbolsInMap[ro, co] != 'h')
        {
            if (symbolsInMap[ro, co] == 'j' || firstIteration) // if I am junction
                upInstructions += "u";

            upInstructions += ShortestTrackFromAToB2('d', copy1,ro - 1, co, rd, cd, false);
            if (upInstructions[upInstructions.Length - 1] == 'c')
                if (shortestWay == "x" || upInstructions.Length < shortestWay.Length)
                    shortestWay = upInstructions;
        }
        if (directionICameFrom != 'l' && symbolsInMap[ro, co] != 'v')
        {
            if (symbolsInMap[ro, co] == 'j' || firstIteration) // if I am junction
                leftInstructions += "l";
            leftInstructions += ShortestTrackFromAToB2('r', copy2,ro, co - 1, rd, cd, false);
            if (leftInstructions[leftInstructions.Length - 1] == 'c')
                if (shortestWay == "x" || leftInstructions.Length < shortestWay.Length)
                    shortestWay = leftInstructions;
        }
        if (directionICameFrom != 'r' && symbolsInMap[ro, co] != 'v')
        {
            if (symbolsInMap[ro, co] == 'j' || firstIteration) // if I am junction
                rightInstructions += "r";
            rightInstructions += ShortestTrackFromAToB2('l',copy3, ro, co + 1, rd, cd, false);
            if (rightInstructions[rightInstructions.Length - 1] == 'c')
                if (shortestWay == "x" || rightInstructions.Length < shortestWay.Length)
                    shortestWay = rightInstructions;
        }
        if (directionICameFrom != 'd' && symbolsInMap[ro, co] != 'h')
        {
            if (symbolsInMap[ro, co] == 'j' || firstIteration) // if I am junction
                downInstructions += "d";
            downInstructions += ShortestTrackFromAToB2('u',copy4, ro + 1, co, rd, cd, false);
            if (downInstructions[downInstructions.Length - 1] == 'c')
                if (shortestWay == "x" || downInstructions.Length < shortestWay.Length)
                    shortestWay = downInstructions;
        }
        return shortestWay;
    }
   
    private string GetCityName() //allow get city name
    { 
        int randomIndex = Random.Range(0, cityNames.Count - 1);
        if (cityNames.Count == 0) return "City";
        string cityName = cityNames[randomIndex];
        cityNames.RemoveAt(randomIndex);
        return cityName;
    }
    private void CityNameGenerator()
    {
        cityNames.Add("Tokyo");
        cityNames.Add("London");
        cityNames.Add("Paris");
        cityNames.Add("New York");
        cityNames.Add("Los Angeles");
        cityNames.Add("Chicago");
        cityNames.Add("Mexico City");
        cityNames.Add("Sao Paulo");
        cityNames.Add("Cairo");
        cityNames.Add("Madrid");
        cityNames.Add("Rome");
        cityNames.Add("Berlin");
        cityNames.Add("Milan");
        cityNames.Add("Munich");
    }
    public void ResetAll() // important operations should be done when reset the map
    {
        cityNames= new List<string>(); 
        CityNameGenerator();
        refToMyGrid = this;
        mapOfObjects = new GameObject[height, width];
        mapOfScripts = new Part[height, width];
        symbolsInMap = new char[height, width];
        allCitiesInMap = new List<Index>();
        citiesOrder = new List<string>();
        AllParts.ResetList();
        UpdatePanel.GetRef().TurnPanelOff(-1);
    }
    public char[,] OutputSymbolsInMap()=> symbolsInMap;

}
